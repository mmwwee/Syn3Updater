using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Cyanlabs.Syn3Updater.Helper;
using Cyanlabs.Syn3Updater.Model;

namespace Cyanlabs.Syn3Updater.UI.Tabs
{
    internal class DownloadViewModel : LanguageAwareBaseViewModel
    {
        #region Events

        public event EventHandler<EventArgs<int>> PercentageChanged;

        #endregion

        #region Constructors

        private ActionCommand _cancelButton;
        public ActionCommand CancelButton => _cancelButton ?? (_cancelButton = new ActionCommand(CancelAction));
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        #endregion

        #region Properties & Fields

        private int _currentProgress, _totalPercentage, _totalPercentageMax, _count;
        private string _downloadInfo, _downloadPercentage, _log, _selectedRelease, _selectedRegion, _selectedMapVersion, _progressBarSuffix, _installMode, _action;
        private bool _cancelButtonEnabled;
        private Task _downloadTask;
        private FileHelper _fileHelper;
        private CancellationToken _ct;

        private ObservableCollection<string> _downloadQueueList;

        public ObservableCollection<string> DownloadQueueList
        {
            get => _downloadQueueList;
            set => SetProperty(ref _downloadQueueList, value);
        }

        public bool CancelButtonEnabled
        {
            get => _cancelButtonEnabled;
            set => SetProperty(ref _cancelButtonEnabled, value);
        }

        public string InstallMode
        {
            get => _installMode;
            set => SetProperty(ref _installMode, value);
        }

        public int CurrentProgress
        {
            get => _currentProgress;
            set => SetProperty(ref _currentProgress, value);
        }

        public string DownloadPercentage
        {
            get => _downloadPercentage;
            set => SetProperty(ref _downloadPercentage, value);
        }

        public int TotalPercentage
        {
            get => _totalPercentage;
            set => SetProperty(ref _totalPercentage, value);
        }

        public int TotalPercentageMax
        {
            get => _totalPercentageMax;
            set => SetProperty(ref _totalPercentageMax, value);
        }

        public string DownloadInfo
        {
            get => _downloadInfo;
            set => SetProperty(ref _downloadInfo, value);
        }

        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }

        #endregion

        #region Methods

        public void Init()
        {
            if (!ApplicationManager.Instance.IsDownloading || _downloadTask != null && _downloadTask.Status.Equals(TaskStatus.Running)) return;
            Log = "";
            _selectedRelease = ApplicationManager.Instance.SelectedRelease;
            _selectedRegion = ApplicationManager.Instance.SelectedRegion;
            _selectedMapVersion = ApplicationManager.Instance.SelectedMapVersion;
            string text = $"Selected Region: {_selectedRegion} - Release: {_selectedRelease} - Map Version: {_selectedMapVersion}";
            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;

            InstallMode = ApplicationManager.Instance.InstallMode;
            _action = ApplicationManager.Instance.Action;

            text = $"Install Mode: {InstallMode}";
            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
            ApplicationManager.Logger.Info(text);

            CancelButtonEnabled = true;
            CurrentProgress = 0;

            PercentageChanged += DownloadPercentageChanged;

            DownloadQueueList = new ObservableCollection<string>();
            foreach (SModel.Ivsu item in ApplicationManager.Instance.Ivsus) DownloadQueueList.Add(item.Url);

            _ct = _tokenSource.Token;

            _fileHelper = new FileHelper(PercentageChanged);

            _downloadTask = Task.Run(DoDownload, _tokenSource.Token).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (t.Exception != null) Application.Current.Dispatcher.Invoke(() => ApplicationManager.Logger.CrashWindow(t.Exception.InnerExceptions.FirstOrDefault()));

                    CancelAction();
                }

                if (t.IsCompleted && t.IsFaulted == false) DownloadComplete();
            }, _tokenSource.Token);
        }

        private async Task DoDownload()
        {
            _count = 0;
            TotalPercentageMax = 100 * ApplicationManager.Instance.Ivsus.Count * (ApplicationManager.Instance.DownloadOnly ? 2 : 4);

            foreach (SModel.Ivsu item in ApplicationManager.Instance.Ivsus)
            {
                if (_ct.IsCancellationRequested)
                {
                    Log += "[" + DateTime.Now + "] " + "Process cancelled by user" + Environment.NewLine;
                    ApplicationManager.Logger.Info("Process cancelled by user");
                    return;
                }

                if (ValidateFile(item.Url, ApplicationManager.Instance.DownloadPath + item.FileName, item.Md5, false,true))
                {
                    string text = $"Validated: {item.FileName} (Skipping Download)";
                    Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                    ApplicationManager.Logger.Info(text);
                    _count++;
                }
                else
                {
                    if (_ct.IsCancellationRequested) return;
                    DownloadInfo = $"Downloading: {item.Url}";

                    Log += "[" + DateTime.Now + "] " + $"Downloading: {item.FileName}" + Environment.NewLine;
                    ApplicationManager.Logger.Info($"Downloading: {item.FileName}");

                    _progressBarSuffix = LanguageManager.GetValue("String.Downloaded");
                    try
                    {
                        string text = "";
                        for (int i = 1; i < 4; i++)
                        {
                            if (_ct.IsCancellationRequested) return;
                            if (i > 1)
                            {
                                text = $"Downloading (Attempt #{i}): {item.Url}";
                                DownloadInfo = text;
                                Log += "[" + DateTime.Now + "] " + $"Downloading: {item.FileName}" + Environment.NewLine;
                                ApplicationManager.Logger.Info($"Downloading: {item.FileName}");
                            }

                            try
                            {
                                await _fileHelper.DownloadFile(item.Url, ApplicationManager.Instance.DownloadPath + item.FileName, _ct, ApplicationManager.Instance.Client);
                            }
                            catch (HttpRequestException webException)
                            {
                                Application.Current.Dispatcher.Invoke(() => ModernWpf.MessageBox.Show(
                                    webException.GetFullMessage(), "Syn3 Updater",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation));
                                ApplicationManager.Logger.Info("ERROR: " + webException.GetFullMessage());
                                CancelAction();
                            }
                            catch (IOException ioException)
                            {
                                Application.Current.Dispatcher.Invoke(() => ModernWpf.MessageBox.Show(
                                    ioException.GetFullMessage(), "Syn3 Updater",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation));
                                ApplicationManager.Logger.Info("ERROR: " + ioException.GetFullMessage());
                                CancelAction();
                            }

                            if (ValidateFile(item.Url, ApplicationManager.Instance.DownloadPath + item.FileName, item.Md5, false))
                            {
                                text = $"Downloaded: {item.FileName}";
                                Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                                ApplicationManager.Logger.Info(text);
                                _count++;
                                break;
                            }

                            if (i == 3)
                            {
                                text = $"unable to validate {item.FileName} after 3 tries, ABORTING PROCESS!";
                                Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                                ApplicationManager.Logger.Info(text);

                                Application.Current.Dispatcher.Invoke(() =>
                                    ModernWpf.MessageBox.Show(string.Format(LanguageManager.GetValue("MessageBox.FailedToValidate3"), item.FileName), "Syn3 Updater",
                                        MessageBoxButton.OK, MessageBoxImage.Error));
                                CancelAction();
                                break;
                            }
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }

                Application.Current.Dispatcher.Invoke(() => { DownloadQueueList.Remove(item.Url); });
                _count++;
                PercentageChanged.Raise(this, 100);
            }

            Application.Current.Dispatcher.Invoke(() => { DownloadQueueList.Clear(); });
        }

        private void DownloadComplete()
        {
            if (_ct.IsCancellationRequested == false)
            {
                if (ApplicationManager.Instance.DownloadOnly)
                {
                    string text = "Process completed successfully (download only)";
                    Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                    ApplicationManager.Logger.Info(text);

                    DownloadInfo = LanguageManager.GetValue("String.Completed");
                    Application.Current.Dispatcher.Invoke(() =>
                        ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.DownloadOnlyComplete"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Information));
                    ApplicationManager.Instance.IsDownloading = false;
                    CancelAction();
                }
                else
                {
                    PrepareUsb();
                }
            }
        }


#pragma warning disable 1998
        private async Task DoCopy()
#pragma warning restore 1998
        {
            foreach (SModel.Ivsu item in ApplicationManager.Instance.Ivsus)
            {
                if (_ct.IsCancellationRequested)
                {
                    Log += "[" + DateTime.Now + "] " + "Process cancelled by user" + Environment.NewLine;
                    ApplicationManager.Logger.Info("Process cancelled by user");
                    return;
                }

                if (ValidateFile(ApplicationManager.Instance.DownloadPath + item.FileName, $@"{ApplicationManager.Instance.DriveLetter}\SyncMyRide\{item.FileName}", item.Md5,
                    true,true))
                {
                    string text = $"{item.FileName} exists and validated successfully, skipping copy";
                    Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                    ApplicationManager.Logger.Info(text);

                    _count++;
                }
                else
                {
                    if (_ct.IsCancellationRequested) return;

                    string text = $"Copying: {item.FileName}";
                    DownloadInfo = text;

                    Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                    ApplicationManager.Logger.Info(text);

                    _progressBarSuffix = LanguageManager.GetValue("String.Copied");

                    for (int i = 1; i < 4; i++)
                    {
                        if (_ct.IsCancellationRequested) return;
                        if (i > 1)
                        {
                            text = $"Copying (Attempt #{i}): {item.FileName}";
                            DownloadInfo = text;

                            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                            ApplicationManager.Logger.Info(text);
                        }

                        try
                        {
                            _fileHelper.CopyFile(ApplicationManager.Instance.DownloadPath + item.FileName,
                                $@"{ApplicationManager.Instance.DriveLetter}\SyncMyRide\{item.FileName}", _ct);
                        }
                        catch (HttpRequestException webException)
                        {
                            Application.Current.Dispatcher.Invoke(() => ModernWpf.MessageBox.Show(
                                webException.GetFullMessage(), "Syn3 Updater",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation));
                            ApplicationManager.Logger.Info("ERROR: " + webException.GetFullMessage());
                            CancelAction();
                        }
                        catch (IOException ioException)
                        {
                            Application.Current.Dispatcher.Invoke(() => ModernWpf.MessageBox.Show(
                                ioException.GetFullMessage(), "Syn3 Updater",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation));
                            ApplicationManager.Logger.Info("ERROR: " + ioException.GetFullMessage());
                            CancelAction();
                        }

                        if (ValidateFile(ApplicationManager.Instance.DownloadPath + item.FileName,
                            $@"{ApplicationManager.Instance.DriveLetter}\SyncMyRide\{item.FileName}", item.Md5, true))
                        {
                            text = $"Copied: {item.FileName}";
                            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                            ApplicationManager.Logger.Info(text);
                            _count++;
                            break;
                        }

                        if (i == 3)
                        {
                            text = $"unable to validate {item.FileName} after 3 tries, ABORTING PROCESS!";
                            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
                            ApplicationManager.Logger.Info(text);

                            Application.Current.Dispatcher.Invoke(() => ModernWpf.MessageBox.Show(
                                string.Format(LanguageManager.GetValue("MessageBox.FailedToValidate3"), item.FileName), "Syn3 Updater", MessageBoxButton.OK,
                                MessageBoxImage.Error));
                            CancelAction();
                            break;
                        }
                    }
                }

                Application.Current.Dispatcher.Invoke(() => DownloadQueueList.Remove(ApplicationManager.Instance.DownloadPath + item.FileName));
                _count++;
                PercentageChanged.Raise(this, 100);
            }
        }

        private void CopyComplete()
        {
            CancelButtonEnabled = false;
            string text;
            if (ApplicationManager.Instance.DownloadToFolder)
            {
                text = "ALL FILES DOWNLOADED AND COPIED TO THE SELECTED FOLDER SUCCESSFULLY!";
            }
            else
            {
                text = "ALL FILES DOWNLOADED AND COPIED TO THE USB DRIVE SUCCESSFULLY!";
            }
            
            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
            ApplicationManager.Logger.Info(text);

            DownloadInfo = LanguageManager.GetValue("String.Completed");
            ApplicationManager.Instance.IsDownloading = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                USBHelper.GenerateLog(Log, ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.UploadLog"), "Syn3 Updater", MessageBoxButton.YesNo,
                    MessageBoxImage.Information) == MessageBoxResult.Yes);

                if (_action == "main")
                {
                    if (ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.UpdateCurrentversion"), "Syn3 Updater", MessageBoxButton.YesNo,
                        MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        ApplicationManager.Instance.Settings.CurrentVersion =
                            Convert.ToInt32(ApplicationManager.Instance.SelectedRelease.Replace(".", "").Replace("Sync ", ""));
                        ApplicationManager.Instance.SVersion = ApplicationManager.Instance.SelectedRelease.Replace("Sync ", "");
                    }

                    if (ApplicationManager.Instance.DownloadToFolder)
                    {
                        ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.CompletedFolder"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                        Process.Start(ApplicationManager.Instance.DriveLetter);
                    }
                    else
                    {
                        ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.Completed"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                        Process.Start($"https://cyanlabs.net/tutorials/windows-automated-method-update-to-3-4/#{InstallMode}");
                    }
                    
                    ApplicationManager.Instance.FireHomeTabEvent();
                }
                else if (_action == "logutility")
                {
                    ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.LogUtilityComplete"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                    ApplicationManager.Instance.UtilityCreateLogStep1Complete = true;
                    ApplicationManager.Instance.FireUtilityTabEvent();
                }
                else if (_action == "gracenotesremoval" || _action == "voiceshrinker" || _action == "downgrade")
                {
                    ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.GenericUtilityComplete"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                    ApplicationManager.Instance.FireUtilityTabEvent();
                }
            });
            Reset();
        }

        private void CancelAction()
        {
            CancelButtonEnabled = false;
            ApplicationManager.Instance.IsDownloading = false;
            _tokenSource.Cancel();
            TotalPercentage = 0;
            CurrentProgress = 0;
            DownloadInfo = "";
            DownloadPercentage = "";
            Application.Current.Dispatcher.Invoke(() => { DownloadQueueList.Clear(); });
            ApplicationManager.Instance.AppsSelected = false;
            ApplicationManager.Instance.SkipFormat = false;
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
            ApplicationManager.Instance.FireHomeTabEvent();
        }

        private void Reset()
        {
            CancelButtonEnabled = false;
            ApplicationManager.Instance.IsDownloading = false;
            _tokenSource.Cancel();
            TotalPercentage = 0;
            CurrentProgress = 0;
            DownloadInfo = "";
            DownloadPercentage = "";
            Application.Current.Dispatcher.Invoke(() => { DownloadQueueList.Clear(); });
            ApplicationManager.Instance.AppsSelected = false;
            ApplicationManager.Instance.SkipFormat = false;
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }

        private void DownloadPercentageChanged(object sender, EventArgs<int> e)
        {
            DownloadPercentage = $"{e.Value}% {_progressBarSuffix}";
            CurrentProgress = e.Value;
            TotalPercentage = _count == 0 ? e.Value : _count * 100 + e.Value;
        }

        private void PrepareUsb()
        {
            if (ApplicationManager.Instance.DownloadToFolder)
            {
                Log += "[" + DateTime.Now + "] " + "Preparing selected directory (No USB Drive Selected)" + Environment.NewLine;
                ApplicationManager.Logger.Info("Preparing selected directory  (No USB Drive Selected)");
            }
            else
            {
                Log += "[" + DateTime.Now + "] " + "Preparing USB drive" + Environment.NewLine;
                ApplicationManager.Logger.Info("Preparing USB drive");
            }

            if (ApplicationManager.Instance.DownloadToFolder)
            {
                DirectoryInfo di = new DirectoryInfo(ApplicationManager.Instance.DriveLetter);
                foreach (FileInfo file in di.GetFiles()) file.Delete();
                foreach (DirectoryInfo dir in di.GetDirectories()) dir.Delete(true);
            } else {
                if (ApplicationManager.Instance.SkipFormat == false && ApplicationManager.Instance.DownloadOnly == false)
                {
                    Log += "[" + DateTime.Now + "] " + "Formatting USB drive" + Environment.NewLine;
                    ApplicationManager.Logger.Info("Formatting USB drive");
                    using (Process p = new Process())
                    {
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.FileName = @"diskpart.exe";
                        p.StartInfo.CreateNoWindow = true;

                        Log += "[" + DateTime.Now + "] " + "Re-creating partition table as MBR and formatting as ExFat on selected USB drive" + Environment.NewLine;
                        ApplicationManager.Logger.Info("Re-creating partition table as MBR and formatting as ExFat on selected USB drive");

                        p.Start();
                        p.StandardInput.WriteLine($"SELECT DISK={ApplicationManager.Instance.DriveNumber}");
                        p.StandardInput.WriteLine("CLEAN");
                        p.StandardInput.WriteLine("CONVERT MBR");
                        p.StandardInput.WriteLine("CREATE PARTITION PRIMARY");
                        p.StandardInput.WriteLine("FORMAT FS=EXFAT LABEL=\"CYANLABS\" QUICK");
                        p.StandardInput.WriteLine($"ASSIGN LETTER={ApplicationManager.Instance.DriveLetter.Replace(":", "")}");
                        p.StandardInput.WriteLine("EXIT");

                        p.WaitForExit();
                    }

                    Thread.Sleep(5000);
                }
            }
            
            if (_action == "main")
                switch (InstallMode)
                {
                    case "autoinstall":
                        CreateAutoInstall();
                        break;
                    case "downgrade":
                        CreateReformat();
                        break;
                    case "reformat":
                        CreateReformat();
                        break;
                }
            else if (_action == "logutility" || _action == "gracenotesremoval" || _action == "voiceshrinker" || _action == "downgrade") CreateAutoInstall();


            foreach (SModel.Ivsu item in ApplicationManager.Instance.Ivsus)
                Application.Current.Dispatcher.Invoke(() => DownloadQueueList.Add(ApplicationManager.Instance.DownloadPath + item.FileName));

            Directory.CreateDirectory($@"{ApplicationManager.Instance.DriveLetter}\SyncMyRide\");
            Task.Run(DoCopy, _tokenSource.Token).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (t.Exception != null) Application.Current.Dispatcher.Invoke(() => ApplicationManager.Logger.CrashWindow(t.Exception.InnerExceptions.FirstOrDefault()));

                    CancelAction();
                }

                if (t.IsCompleted && t.IsFaulted == false) CopyComplete();
            }, _tokenSource.Token);
        }

        private void CreateAutoInstall()
        {
            Log += "[" + DateTime.Now + "] " + "Generating Autoinstall.lst" + Environment.NewLine;
            ApplicationManager.Logger.Info("Generating Autoinstall.lst");

            string autoinstalllst =
                $@"; CyanLabs Syn3Updater 2.x - Autoinstall Mode - {_selectedRelease} {_selectedRegion}{Environment.NewLine}{Environment.NewLine}[SYNCGen3.0_ALL_PRODUCT]{Environment.NewLine}";

            string extrafiles = "";
            int baseint = 0, extraint = 0;
            foreach (SModel.Ivsu item in ApplicationManager.Instance.Ivsus)
                if (item.Type == @"APPS" || item.Type == @"VOICE" || item.Type == @"ENH_DAB" || item.Type == @"MAP_LICENSE" || item.Type == @"VOICE_NAV" ||
                    ApplicationManager.Instance.AppsSelected == false)
                {
                    baseint++;
                    autoinstalllst += $@"Item{baseint} = {item.Type} - {item.FileName}\rOpen{baseint} = SyncMyRide\{item.FileName}\r".Replace(@"\r", Environment.NewLine);
                }
                else if (ApplicationManager.Instance.AppsSelected)
                {
                    if (extrafiles == "") extrafiles = $@"[SYNCGen3.0_ALL]{Environment.NewLine}";
                    if (extraint == 10)
                    {
                        extraint = 0;
                        extrafiles += $@"Options = Delay,Include,Transaction{Environment.NewLine}[SYNCGen3.0_{ApplicationManager.Instance.SVersion}]{Environment.NewLine}";
                    }

                    extraint++;
                    extrafiles += $@"Item{extraint} = {item.Type} - {item.FileName}\rOpen{extraint} = SyncMyRide\{item.FileName}\r".Replace(@"\r", Environment.NewLine);
                }

            if (extrafiles != "") extrafiles += @"Options = Delay,Include,Transaction";
            autoinstalllst += $@"Options = AutoInstall{Environment.NewLine}";
            autoinstalllst += extrafiles;
            File.WriteAllText($@"{ApplicationManager.Instance.DriveLetter}\autoinstall.lst", autoinstalllst);
            File.Create($@"{ApplicationManager.Instance.DriveLetter}\DONTINDX.MSA");
        }

        private void CreateReformat()
        {
            Log += "[" + DateTime.Now + "] " + "Generating reformat.lst" + Environment.NewLine;
            ApplicationManager.Logger.Info("Generating reformat.lst");

            string reformatlst = "";
            int i = 0;
            foreach (SModel.Ivsu item in ApplicationManager.Instance.Ivsus)
            {
                if (InstallMode == @"reformat")
                {
                    if (item.Md5 == Api.ReformatTool.Md5) continue;
                }
                else if(InstallMode == @"downgrade")
                {
                    if (item.Md5 == Api.ReformatTool.Md5 || (item.Md5 == Api.DowngradeApp.Md5 && _selectedRelease != @"Sync 3.3.19052") || item.Md5 == Api.DowngradeTool.Md5) continue;
                }
                i++;
                reformatlst += $@"{item.Type}={item.FileName}";
                if (i != ApplicationManager.Instance.Ivsus.Count) reformatlst += Environment.NewLine;
            }

            File.WriteAllText($@"{ApplicationManager.Instance.DriveLetter}\reformat.lst", reformatlst);

            Log += "[" + DateTime.Now + "] " + "Generating autoinstall.lst" + Environment.NewLine;
            ApplicationManager.Logger.Info("Generating autoinstall.lst");

            string autoinstalllst =
                $@"; CyanLabs Syn3Updater 2.x - {InstallMode} Mode - {_selectedRelease} {_selectedRegion}{Environment.NewLine}{Environment.NewLine}[SYNCGen3.0_ALL_PRODUCT]{Environment.NewLine}";
            if (InstallMode == @"downgrade")
            {
                autoinstalllst +=
                    $@"Item1 = TOOL - {Api.DowngradeTool.FileName}\rOpen1 = SyncMyRide\{Api.DowngradeTool.FileName}\r".Replace(@"\r",
                        Environment.NewLine);
                autoinstalllst +=
                    $@"Item2 = APP - {Api.DowngradeApp.FileName}\rOpen2 = SyncMyRide\{Api.DowngradeApp.FileName}\r".Replace(@"\r",
                        Environment.NewLine);
                autoinstalllst += $@"Options = AutoInstall{Environment.NewLine}[SYNCGen3.0_ALL]{Environment.NewLine}";
                autoinstalllst +=
                    $@"Item1 = REFORMAT TOOL - {Api.ReformatTool.FileName}\rOpen1 = SyncMyRide\{Api.ReformatTool.FileName}\r".Replace(@"\r",
                        Environment.NewLine);
                autoinstalllst += $@"Options = AutoInstall,Include,Transaction{Environment.NewLine}";
            }
            else if (InstallMode == @"reformat")
            {
                autoinstalllst +=
                    $@"Item1 = REFORMAT TOOL  - {Api.ReformatTool.FileName}\rOpen1 = SyncMyRide\{Api.ReformatTool.FileName}\r".Replace(@"\r",
                        Environment.NewLine);
                autoinstalllst += @"Options = AutoInstall";
            }

            File.WriteAllText($@"{ApplicationManager.Instance.DriveLetter}\autoinstall.lst", autoinstalllst);
            File.Create($@"{ApplicationManager.Instance.DriveLetter}\DONTINDX.MSA");
        }

        private bool ValidateFile(string srcfile, string localfile, string md5, bool copy, bool existing = false)
        {
            string text = $"Validating: {Path.GetFileName(localfile)}";
            if (existing) text = $"Checking Existing File: {Path.GetFileName(localfile)}";
            DownloadInfo = text;
            Log += "[" + DateTime.Now + "] " + text + Environment.NewLine;
            ApplicationManager.Logger.Info(text);

            _progressBarSuffix = LanguageManager.GetValue("String.Validated");
            FileHelper.ValidateResult validateResult = _fileHelper.ValidateFile(srcfile, localfile, md5, copy, _ct, ApplicationManager.Instance.Client);

            if (validateResult.Message != "")
            {
                Log += "[" + DateTime.Now + "] " + validateResult.Message + Environment.NewLine;
                ApplicationManager.Logger.Info(validateResult.Message);
            }

            return validateResult.Result;
        }

        #endregion
    }
}