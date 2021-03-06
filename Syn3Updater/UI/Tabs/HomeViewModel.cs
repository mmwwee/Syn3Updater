using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Cyanlabs.Syn3Updater.Helper;
using Cyanlabs.Syn3Updater.Model;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;

namespace Cyanlabs.Syn3Updater.UI.Tabs
{
    internal class HomeViewModel : LanguageAwareBaseViewModel
    {
        #region Constructors

        private ActionCommand _startButton;
        private ActionCommand _refreshUSB;
        private ActionCommand _regionInfo;
        public ActionCommand RefreshUSB => _refreshUSB ??= new ActionCommand(RefreshUsb);
        public ActionCommand RegionInfo => _regionInfo ??= new ActionCommand(RegionInfoAction);
        public ActionCommand StartButton => _startButton ??= new ActionCommand(StartAction);

        #endregion

        #region Properties & Fields

        private string _apiAppReleases, _apiMapReleases;
        private string _stringCompatibility, _stringReleasesJson, _stringMapReleasesJson, _stringDownloadJson, _stringMapDownloadJson;
        private Api.JsonReleases _jsonMapReleases, _jsonReleases;

        private string _driveLetter;

        public string DriveLetter
        {
            get => _driveLetter;
            set => SetProperty(ref _driveLetter, value);
        }

        private string _driveName;

        public string DriveName
        {
            get => _driveName;
            set => SetProperty(ref _driveName, value);
        }

        private string _driveFileSystem;

        public string DriveFileSystem
        {
            get => _driveFileSystem;
            set => SetProperty(ref _driveFileSystem, value);
        }

        private USBHelper.Drive _selectedDrive;

        public USBHelper.Drive SelectedDrive
        {
            get => _selectedDrive;
            set
            {
                SetProperty(ref _selectedDrive, value);
                UpdateDriveInfo();
            }
        }

        private SModel.SRegion _selectedRegion;

        public SModel.SRegion SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                SetProperty(ref _selectedRegion, value);
                if (value != null) UpdateSelectedRegion();
            }
        }

        private string _selectedRelease;

        public string SelectedRelease
        {
            get => _selectedRelease;
            set
            {
                SetProperty(ref _selectedRelease, value);
                if (value != null) UpdateSelectedRelease();
            }
        }

        private string _selectedMapVersion;

        public string SelectedMapVersion
        {
            get => _selectedMapVersion;
            set
            {
                SetProperty(ref _selectedMapVersion, value);
                if (value != null) UpdateSelectedMapVersion();
            }
        }

        private int _selectedMapVersionIndex;

        public int SelectedMapVersionIndex
        {
            get => _selectedMapVersionIndex;
            set => SetProperty(ref _selectedMapVersionIndex, value);
        }

        private int _selectedReleaseIndex;

        public int SelectedReleaseIndex
        {
            get => _selectedReleaseIndex;
            set => SetProperty(ref _selectedReleaseIndex, value);
        }

        private int _selectedRegionIndex;

        public int SelectedRegionIndex
        {
            get => _selectedRegionIndex;
            set => SetProperty(ref _selectedRegionIndex, value);
        }

        private ObservableCollection<USBHelper.Drive> _driveList;

        public ObservableCollection<USBHelper.Drive> DriveList
        {
            get => _driveList;
            set => SetProperty(ref _driveList, value);
        }

        private ObservableCollection<SModel.SRegion> _sRegions;

        public ObservableCollection<SModel.SRegion> SRegions
        {
            get => _sRegions;
            set => SetProperty(ref _sRegions, value);
        }

        private ObservableCollection<string> _sVersion;

        public ObservableCollection<string> SVersion
        {
            get => _sVersion;
            set => SetProperty(ref _sVersion, value);
        }

        private ObservableCollection<string> _sMapVersion;

        public ObservableCollection<string> SMapVersion
        {
            get => _sMapVersion;
            set => SetProperty(ref _sMapVersion, value);
        }

        private bool _sVersionsEnabled;

        public bool SVersionsEnabled
        {
            get => _sVersionsEnabled;
            set => SetProperty(ref _sVersionsEnabled, value);
        }

        private bool _sMapVersionsEnabled;

        public bool SMapVersionsEnabled
        {
            get => _sMapVersionsEnabled;
            set => SetProperty(ref _sMapVersionsEnabled, value);
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private Visibility _notesVisibility;

        public Visibility NotesVisibility
        {
            get => _notesVisibility;
            set => SetProperty(ref _notesVisibility, value);
        }

        private ObservableCollection<SModel.Ivsu> _ivsuList;

        public ObservableCollection<SModel.Ivsu> IvsuList
        {
            get => _ivsuList;
            set => SetProperty(ref _ivsuList, value);
        }

        private string _currentRegion;

        public string CurrentRegion
        {
            get => _currentRegion;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _currentRegion, value);
                    ApplicationManager.Instance.Settings.CurrentRegion = value;
                }
            }
        }

        private string _currentVersion;

        public string CurrentVersion
        {
            get => _currentVersion;
            set => SetProperty(ref _currentVersion, value);
        }

        private string _currentNav;

        public string CurrentNav
        {
            get => _currentNav;
            set => SetProperty(ref _currentNav, value);
        }

        private string _downloadLocation;

        public string DownloadLocation
        {
            get => _downloadLocation;
            set
            {
                if (value != null) SetProperty(ref _downloadLocation, value);
            }
        }

        private string _installMode;

        public string InstallMode
        {
            get => _installMode;
            set => SetProperty(ref _installMode, value);
        }

        private bool _startEnabled;

        public bool StartEnabled
        {
            get => _startEnabled;
            set => SetProperty(ref _startEnabled, value);
        }

        private Visibility _driveDetailsVisible;

        public Visibility DriveDetailsVisible
        {
            get => _driveDetailsVisible;
            set => SetProperty(ref _driveDetailsVisible, value);
        }

        #endregion

        #region Methods

        public void ReloadSettings()
        {
            NotesVisibility = Visibility.Hidden;
            CurrentNav = ApplicationManager.Instance.Settings.CurrentNav ? "Yes" : "No";
            CurrentRegion = ApplicationManager.Instance.Settings.CurrentRegion;
            CurrentVersion = ApplicationManager.Instance.SVersion;
            DownloadLocation = ApplicationManager.Instance.DownloadPath;
            SelectedMapVersionIndex = -1;
            SelectedReleaseIndex = -1;
            SelectedRegionIndex = -1;
            StartEnabled = false;
            IvsuList = new ObservableCollection<SModel.Ivsu>();
            InstallMode = "";
            RefreshUsb();
            SMapVersion = new ObservableCollection<string>();
            SVersion?.Clear();
            SMapVersion?.Clear();
            DriveDetailsVisible = SelectedDrive == null || SelectedDrive.Path == "" ? Visibility.Hidden : Visibility.Visible;
            ApplicationManager.Logger.Info($"Current Details - Region: {CurrentRegion} - Version: {CurrentVersion} - Navigation: {CurrentNav}");
        }

        public void Init()
        {
            SelectedRegionIndex = -1;
            ApplicationManager.Instance.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiSecret.Token);
            SRegions = new ObservableCollection<SModel.SRegion>
            {
                new SModel.SRegion {Code = "EU", Name = "Europe"},
                new SModel.SRegion {Code = "NA", Name = "United States, Canada & Mexico"},
                new SModel.SRegion {Code = "CN", Name = "China"},
                new SModel.SRegion {Code = "ANZ", Name = "Australia, New Zealand, South America, Turkey & Taiwan"},
                new SModel.SRegion {Code = "ROW", Name = "Middle East, Africa, India, Sri Lanka, Israel, South East Asia, Caribbean & Central America"}
            };
            SVersionsEnabled = false;
        }

        private void RegionInfoAction()
        {
            Process.Start(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en"
                ? "https://cyanlabs.net/api/Syn3Updater/region.php"
                : $"https://translate.google.co.uk/translate?hl=&sl=en&tl={Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName}&u=https%3A%2F%2Fcyanlabs.net%2Fapi%2FSyn3Updater%2Fregion.php");
        }

        private void RefreshUsb()
        {
            try
            {
                ObservableCollection<USBHelper.Drive> tmpDriveList = USBHelper.RefreshDevices(true);
                if (tmpDriveList.Count > 0) DriveList = tmpDriveList;
            }
            catch (XamlParseException e)
            {
                ModernWpf.MessageBox.Show(e.GetFullMessage(), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                ApplicationManager.Logger.Info("ERROR: " + e.GetFullMessage());
            }
            catch (UnauthorizedAccessException e)
            {
                ModernWpf.MessageBox.Show(e.GetFullMessage(), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                ApplicationManager.Logger.Info("ERROR: " + e.GetFullMessage());
            }
        }

        private void UpdateDriveInfo()
        {
            StartEnabled = SelectedRelease != null && SelectedRegion != null && SelectedMapVersion != null && SelectedDrive != null;
            if (SelectedDrive?.Name == LanguageManager.GetValue("Home.NoUSBDir"))
            {
                VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog {};
                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string destination = dialog.SelectedPath;
                    if (ApplicationManager.Instance.DownloadPath.Contains(destination))
                    {
                        ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.CancelDownloadIsFolder"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        ReloadSettings();
                    }
                    else
                    {
                        DriveLetter = destination + "\\";
                        DriveFileSystem = "";
                        DriveName = "";
                        ApplicationManager.Instance.DriveName = SelectedDrive?.Name;
                        DriveDetailsVisible = Visibility.Visible;
                    }
                }

            }
            else
            {
                USBHelper.DriveInfo driveInfo = USBHelper.UpdateDriveInfo(SelectedDrive);

                // Update app level vars
                ApplicationManager.Instance.DriveFileSystem = driveInfo.FileSystem;
                ApplicationManager.Instance.DrivePartitionType = driveInfo.PartitionType;
                ApplicationManager.Instance.DriveName = SelectedDrive?.Name;
                ApplicationManager.Instance.SkipFormat = driveInfo.SkipFormat;

                // Update local level vars
                DriveLetter = driveInfo.Letter;
                DriveFileSystem = driveInfo.PartitionType + " " + driveInfo.FileSystem;
                DriveName = driveInfo.Name;
                DriveDetailsVisible = driveInfo.Name == null ? Visibility.Hidden : Visibility.Visible;
            }
            
        }

        private void UpdateSelectedRegion()
        {
            NotesVisibility = Visibility.Hidden;
            if (SelectedRegion.Code != "")
            {
                IvsuList.Clear();
                SelectedMapVersion = null;
                SelectedRelease = null;
                SMapVersion.Clear();
                if (ApplicationManager.Instance.Settings.ShowAllReleases)
                {
                    _apiMapReleases = Api.MapReleasesConst.Replace("[published]", $"filter[licensekeys][_in]=v2,{ApplicationManager.Instance.Settings.LicenseKey}");
                    _apiAppReleases = Api.AppReleasesConst.Replace("[published]", $"filter[key][_in]=public,v2,{ApplicationManager.Instance.Settings.LicenseKey}");
                }
                else
                {
                    _apiMapReleases = Api.MapReleasesConst.Replace("[published]",
                        $"filter[status][_in]=published,private&filter[licensekeys][_in]=v2,{ApplicationManager.Instance.Settings.LicenseKey}");
                    _apiAppReleases = Api.AppReleasesConst.Replace("[published]",
                        $"filter[status][_in]=published,private&filter[key][_in]=public,v2,{ApplicationManager.Instance.Settings.LicenseKey}");
                }

                try
                {
                    HttpResponseMessage response = ApplicationManager.Instance.Client.GetAsync(_apiAppReleases).Result;
                    _stringReleasesJson = response.Content.ReadAsStringAsync().Result;
                }
                // ReSharper disable once RedundantCatchClause
                catch (WebException)
                {
                    throw;
                    //TODO Exception handling
                }

                if (!ApplicationManager.Instance.Settings.CurrentNav)
                {
                    SMapVersion.Add(LanguageManager.GetValue("String.NonNavAPIM"));
                }
                else
                {
                    if (ApplicationManager.Instance.Settings.CurrentVersion >= Api.ReformatVersion)
                        SMapVersion.Add(LanguageManager.GetValue("String.KeepExistingMaps"));
                }


                _jsonReleases = JsonConvert.DeserializeObject<Api.JsonReleases>(_stringReleasesJson);
                SVersion = new ObservableCollection<string>();

                foreach (Api.Data item in _jsonReleases.Releases)
                    if (item.Regions.Contains(SelectedRegion.Code))
                        SVersion.Add(item.Name);

                SVersionsEnabled = true;
                StartEnabled = SelectedRelease != null && SelectedRegion != null && SelectedMapVersion != null && SelectedDrive != null;
            }
        }

        private void UpdateSelectedRelease()
        {
            if (SelectedRelease != "")
            {
                SelectedMapVersion = null;
                IvsuList.Clear();
                foreach (Api.Data item in _jsonReleases.Releases)
                    if (item.Name == SelectedRelease)
                    {
                        _stringCompatibility = item.Version.Substring(0, 3);
                        if (item.Notes == null)
                        {
                            NotesVisibility = Visibility.Hidden;
                            continue;
                        }

                        NotesVisibility = Visibility.Visible;
                        Notes = item.Notes.Replace("\n", Environment.NewLine);
                    }

                _apiMapReleases = _apiMapReleases.Replace("[regionplaceholder]", $"filter[regions]={SelectedRegion.Code}&filter[compatibility][_contains]={_stringCompatibility}");

                HttpResponseMessage response = ApplicationManager.Instance.Client.GetAsync(_apiMapReleases).Result;
                _stringMapReleasesJson = response.Content.ReadAsStringAsync().Result;

                if (ApplicationManager.Instance.Settings.CurrentNav)
                {
                    SMapVersion.Clear();
                    SMapVersion.Add(LanguageManager.GetValue("String.NoMaps"));
                    if (ApplicationManager.Instance.Settings.CurrentNav)
                    {
                        if (ApplicationManager.Instance.Settings.CurrentVersion >= Api.ReformatVersion)
                            SMapVersion.Add(LanguageManager.GetValue("String.KeepExistingMaps"));
                    }
                    else
                    {
                        SMapVersion.Add(LanguageManager.GetValue("String.NonNavAPIM"));
                    }

                    _jsonMapReleases = JsonConvert.DeserializeObject<Api.JsonReleases>(_stringMapReleasesJson);
                    foreach (Api.Data item in _jsonMapReleases.Releases) SMapVersion.Add(item.Name);
                }

                SMapVersionsEnabled = true;
                StartEnabled = SelectedRelease != null && SelectedRegion != null && SelectedMapVersion != null && SelectedDrive != null;
            }
        }

        private void UpdateSelectedMapVersion()
        {
            if (SelectedMapVersion != "")
            {
                IvsuList.Clear();

                //LESS THAN 3.2
                if (ApplicationManager.Instance.Settings.CurrentVersion < Api.ReformatVersion)
                {
                    InstallMode = ApplicationManager.Instance.Settings.CurrentInstallMode == "autodetect" ? "reformat" : ApplicationManager.Instance.Settings.CurrentInstallMode;
                }

                //Above 3.2 and  Below 3.4.19274
                else if (ApplicationManager.Instance.Settings.CurrentVersion >= Api.ReformatVersion &&
                         ApplicationManager.Instance.Settings.CurrentVersion < Api.BlacklistedVersion)
                {
                    //Update Nav?
                    if (SelectedMapVersion == LanguageManager.GetValue("String.NoMaps") || SelectedMapVersion == LanguageManager.GetValue("String.NonNavAPIM") ||
                        SelectedMapVersion == LanguageManager.GetValue("String.KeepExistingMaps"))
                        InstallMode = ApplicationManager.Instance.Settings.CurrentInstallMode == "autodetect"
                            ? "autoinstall"
                            : ApplicationManager.Instance.Settings.CurrentInstallMode;
                    else
                        InstallMode = ApplicationManager.Instance.Settings.CurrentInstallMode == "autodetect"
                            ? "reformat"
                            : ApplicationManager.Instance.Settings.CurrentInstallMode;
                }

                //3.4.19274 or above
                else if (ApplicationManager.Instance.Settings.CurrentVersion >= Api.BlacklistedVersion)
                {
                    //Update Nav?
                    if (SelectedMapVersion == LanguageManager.GetValue("String.NoMaps") || SelectedMapVersion == LanguageManager.GetValue("String.NonNavAPIM") ||
                        SelectedMapVersion == LanguageManager.GetValue("String.KeepExistingMaps"))
                        InstallMode = ApplicationManager.Instance.Settings.CurrentInstallMode == "autodetect"
                            ? "autoinstall"
                            : ApplicationManager.Instance.Settings.CurrentInstallMode;
                    else
                        InstallMode = ApplicationManager.Instance.Settings.CurrentInstallMode == "autodetect"
                            ? "downgrade"
                            : ApplicationManager.Instance.Settings.CurrentInstallMode;
                }

                ApplicationManager.Instance.Action = "main";
                ApplicationManager.Instance.InstallMode = InstallMode;

                string appReleaseSingle = ApplicationManager.Instance.Settings.CurrentNav
                    ? Api.AppReleaseSingle.Replace("[navplaceholder]", "nav") + SelectedRelease
                    : Api.AppReleaseSingle.Replace("[navplaceholder]", "nonnav") + SelectedRelease;

                HttpResponseMessage response = ApplicationManager.Instance.Client.GetAsync(appReleaseSingle).Result;
                _stringDownloadJson = response.Content.ReadAsStringAsync().Result;

                response = ApplicationManager.Instance.Client.GetAsync(Api.MapReleaseSingle + SelectedMapVersion).Result;
                _stringMapDownloadJson = response.Content.ReadAsStringAsync().Result;

                Api.JsonReleases jsonIvsUs = JsonConvert.DeserializeObject<Api.JsonReleases>(_stringDownloadJson);
                Api.JsonReleases jsonMapIvsUs = JsonConvert.DeserializeObject<Api.JsonReleases>(_stringMapDownloadJson);

                foreach (Api.Ivsus item in jsonIvsUs.Releases[0].IvsusList)
                    if (item.Ivsu.Regions.Contains("ALL") || item.Ivsu.Regions.Contains(SelectedRegion.Code))
                    {

                        string fileName = FileHelper.url_to_filename(item.Ivsu.Url);
                        IvsuList.Add(new SModel.Ivsu
                        {
                            Type = item.Ivsu.Type, Name = item.Ivsu.Name, Version = item.Ivsu.Version,
                            Notes = item.Ivsu.Notes, Url = item.Ivsu.Url, Md5 = item.Ivsu.Md5, Selected = true,
                            FileName = fileName
                        });
                    }

                if (SelectedMapVersion != LanguageManager.GetValue("String.NoMaps") && SelectedMapVersion != LanguageManager.GetValue("String.NonNavAPIM") &&
                    SelectedMapVersion != LanguageManager.GetValue("String.KeepExistingMaps"))
                    foreach (Api.Ivsus item in jsonMapIvsUs.Releases[0].IvsusList)
                        if (item.MapIvsu.Regions.Contains("ALL") || item.MapIvsu.Regions.Contains(SelectedRegion.Code))
                        {
                            string fileName = FileHelper.url_to_filename(item.MapIvsu.Url);
                            IvsuList.Add(new SModel.Ivsu
                            {
                                Type = item.MapIvsu.Type, Name = item.MapIvsu.Name, Version = item.MapIvsu.Version,
                                Notes = item.MapIvsu.Notes, Url = item.MapIvsu.Url, Md5 = item.MapIvsu.Md5,
                                Selected = true, FileName = fileName
                            });
                        }

                StartEnabled = SelectedRelease != null && SelectedRegion != null && SelectedMapVersion != null && SelectedDrive != null;
            }
        }

        private void StartAction()
        {
            ApplicationManager.Logger.Info(
                $"USB Drive selected - Name: {ApplicationManager.Instance.DriveName} - FileSystem: {ApplicationManager.Instance.DriveFileSystem} - PartitionType: {ApplicationManager.Instance.DrivePartitionType} - Letter: {DriveLetter}");
            ApplicationManager.Instance.Ivsus.Clear();

            if (InstallMode == "downgrade")
            {
                Api.DowngradeApp = ApiHelper.GetSpecialIvsu(Api.GetDowngradeApp);
                ApplicationManager.Instance.Ivsus.Add(Api.DowngradeApp);

                Api.DowngradeTool = ApiHelper.GetSpecialIvsu(Api.GetDowngradeTool);
                ApplicationManager.Instance.Ivsus.Add(Api.DowngradeTool);
            }

            if (InstallMode == "reformat" || InstallMode == "downgrade")
            {
                Api.ReformatTool = ApiHelper.GetSpecialIvsu(Api.GetReformat);
                ApplicationManager.Instance.Ivsus.Add(Api.ReformatTool);
            }

            ApplicationManager.Instance.DownloadOnly = false;
            ApplicationManager.Instance.DriveLetter = DriveLetter;
            foreach (SModel.Ivsu item in IvsuList)
                if (item.Selected)
                {
                    if (item.Type == "APPS") ApplicationManager.Instance.AppsSelected = true;

                    ApplicationManager.Instance.Ivsus.Add(item);
                }

            bool canceldownload = false;
            //Install Mode is reformat or downgrade My20 warning
            if (InstallMode == "reformat" || InstallMode == "downgrade")
            {
                if (ModernWpf.MessageBox.Show(string.Format(LanguageManager.GetValue("MessageBox.CancelMy20"), InstallMode), "Syn3 Updater", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.CancelMy20Final"), "Syn3 Updater", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                        MessageBoxResult.Yes)
                        canceldownload = true;
                }
                else
                {
                    canceldownload = true;
                }
            }

            //Warn is users region is different to new selection
            if (SelectedRegion.Code != ApplicationManager.Instance.Settings.CurrentRegion)
                if (ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.CancelRegionMismatch"), "Syn3 Updater", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                    MessageBoxResult.Yes)
                    canceldownload = true;

            //Cancel no apps package selected
            if (ApplicationManager.Instance.AppsSelected == false && (InstallMode == "reformat" || InstallMode == "downgrade"))
            {
                ModernWpf.MessageBox.Show(LanguageManager.GetValue("MessageBox.CancelNoApps"), "Syn3 Updater", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                canceldownload = true;
            }

            if (!canceldownload && !SanityCheckHelper.CancelDownloadCheck(SelectedDrive))
            {
                if (ApplicationManager.Instance.DownloadOnly == false)
                {
                    ApplicationManager.Instance.DriveNumber = SelectedDrive.Path.Replace("Win32_DiskDrive.DeviceID=\"\\\\\\\\.\\\\PHYSICALDRIVE", "").Replace("\"", "");
                    ApplicationManager.Instance.DriveLetter = DriveLetter;
                }

                ApplicationManager.Instance.SelectedRegion = SelectedRegion.Code;
                ApplicationManager.Instance.SelectedRelease = SelectedRelease;
                ApplicationManager.Instance.SelectedMapVersion = SelectedMapVersion;
                ApplicationManager.Instance.IsDownloading = true;
                ApplicationManager.Logger.Info($@"Starting process ({SelectedRelease} - {SelectedRegion.Code} - {SelectedMapVersion})");
                StartEnabled = false;
                ApplicationManager.Instance.FireDownloadsTabEvent();
            }
        }

        #endregion
    }
}