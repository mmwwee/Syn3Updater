﻿namespace Syn3Updater.Forms
{
    partial class FrmSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetup));
            this.btnSetupContinue = new System.Windows.Forms.Button();
            this.grpDownload = new System.Windows.Forms.GroupBox();
            this.txtDownloadPath = new System.Windows.Forms.TextBox();
            this.btnChangeDownloadDirectory = new System.Windows.Forms.Button();
            this.grpExistingDetails = new System.Windows.Forms.GroupBox();
            this.txtCurrentSyncVersion = new System.Windows.Forms.MaskedTextBox();
            this.chkCurrentSyncNav = new System.Windows.Forms.CheckBox();
            this.lblCurrentSyncVersion = new System.Windows.Forms.Label();
            this.lblCurrentSyncRegion = new System.Windows.Forms.Label();
            this.cmbCurrentSyncRegion = new System.Windows.Forms.ComboBox();
            this.lblWarning1 = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnLogo = new System.Windows.Forms.PictureBox();
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.cmbLocale = new System.Windows.Forms.ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.folderDownloads = new System.Windows.Forms.FolderBrowserDialog();
            this.grpAdvanced = new System.Windows.Forms.GroupBox();
            this.chkAllReleases = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOverride = new System.Windows.Forms.ComboBox();
            this.lblReleaseKey = new System.Windows.Forms.Label();
            this.grpPurchase = new System.Windows.Forms.GroupBox();
            this.lblPurchaseInfo = new System.Windows.Forms.LinkLabel();
            this.btnPurchaseActivate = new System.Windows.Forms.Button();
            this.txtReleaseKey = new System.Windows.Forms.TextBox();
            this.grpDownload.SuspendLayout();
            this.grpExistingDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogo)).BeginInit();
            this.panelTitleBar.SuspendLayout();
            this.grpAdvanced.SuspendLayout();
            this.grpPurchase.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetupContinue
            // 
            resources.ApplyResources(this.btnSetupContinue, "btnSetupContinue");
            this.btnSetupContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnSetupContinue.FlatAppearance.BorderSize = 0;
            this.btnSetupContinue.Name = "btnSetupContinue";
            this.btnSetupContinue.UseVisualStyleBackColor = false;
            this.btnSetupContinue.Click += new System.EventHandler(this.btnSetupContinue_Click);
            // 
            // grpDownload
            // 
            this.grpDownload.Controls.Add(this.txtDownloadPath);
            this.grpDownload.Controls.Add(this.btnChangeDownloadDirectory);
            this.grpDownload.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpDownload, "grpDownload");
            this.grpDownload.Name = "grpDownload";
            this.grpDownload.TabStop = false;
            // 
            // txtDownloadPath
            // 
            this.txtDownloadPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.txtDownloadPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDownloadPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Syn3Updater.Properties.Settings.Default, "DownloadPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtDownloadPath.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtDownloadPath, "txtDownloadPath");
            this.txtDownloadPath.Name = "txtDownloadPath";
            this.txtDownloadPath.ReadOnly = true;
            this.txtDownloadPath.Text = global::Syn3Updater.Properties.Settings.Default.DownloadPath;
            // 
            // btnChangeDownloadDirectory
            // 
            this.btnChangeDownloadDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.btnChangeDownloadDirectory.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnChangeDownloadDirectory, "btnChangeDownloadDirectory");
            this.btnChangeDownloadDirectory.ForeColor = System.Drawing.Color.White;
            this.btnChangeDownloadDirectory.Name = "btnChangeDownloadDirectory";
            this.btnChangeDownloadDirectory.UseVisualStyleBackColor = false;
            this.btnChangeDownloadDirectory.Click += new System.EventHandler(this.btnChangeDownloadDirectory_Click);
            // 
            // grpExistingDetails
            // 
            this.grpExistingDetails.Controls.Add(this.txtCurrentSyncVersion);
            this.grpExistingDetails.Controls.Add(this.chkCurrentSyncNav);
            this.grpExistingDetails.Controls.Add(this.lblCurrentSyncVersion);
            this.grpExistingDetails.Controls.Add(this.lblCurrentSyncRegion);
            this.grpExistingDetails.Controls.Add(this.cmbCurrentSyncRegion);
            this.grpExistingDetails.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpExistingDetails, "grpExistingDetails");
            this.grpExistingDetails.Name = "grpExistingDetails";
            this.grpExistingDetails.TabStop = false;
            // 
            // txtCurrentSyncVersion
            // 
            this.txtCurrentSyncVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.txtCurrentSyncVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrentSyncVersion.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Syn3Updater.Properties.Settings.Default, "CurrentSyncVersion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.txtCurrentSyncVersion, "txtCurrentSyncVersion");
            this.txtCurrentSyncVersion.ForeColor = System.Drawing.Color.White;
            this.txtCurrentSyncVersion.Name = "txtCurrentSyncVersion";
            this.txtCurrentSyncVersion.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // chkCurrentSyncNav
            // 
            this.chkCurrentSyncNav.Checked = global::Syn3Updater.Properties.Settings.Default.CurrentSyncNav;
            this.chkCurrentSyncNav.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCurrentSyncNav.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Syn3Updater.Properties.Settings.Default, "CurrentSyncNav", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.chkCurrentSyncNav, "chkCurrentSyncNav");
            this.chkCurrentSyncNav.Name = "chkCurrentSyncNav";
            this.chkCurrentSyncNav.UseVisualStyleBackColor = true;
            // 
            // lblCurrentSyncVersion
            // 
            this.lblCurrentSyncVersion.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCurrentSyncVersion, "lblCurrentSyncVersion");
            this.lblCurrentSyncVersion.Name = "lblCurrentSyncVersion";
            // 
            // lblCurrentSyncRegion
            // 
            this.lblCurrentSyncRegion.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCurrentSyncRegion, "lblCurrentSyncRegion");
            this.lblCurrentSyncRegion.Name = "lblCurrentSyncRegion";
            // 
            // cmbCurrentSyncRegion
            // 
            this.cmbCurrentSyncRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.cmbCurrentSyncRegion.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Syn3Updater.Properties.Settings.Default, "CurrentSyncRegion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbCurrentSyncRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbCurrentSyncRegion, "cmbCurrentSyncRegion");
            this.cmbCurrentSyncRegion.ForeColor = System.Drawing.Color.White;
            this.cmbCurrentSyncRegion.FormattingEnabled = true;
            this.cmbCurrentSyncRegion.Items.AddRange(new object[] {
            resources.GetString("cmbCurrentSyncRegion.Items"),
            resources.GetString("cmbCurrentSyncRegion.Items1"),
            resources.GetString("cmbCurrentSyncRegion.Items2"),
            resources.GetString("cmbCurrentSyncRegion.Items3"),
            resources.GetString("cmbCurrentSyncRegion.Items4")});
            this.cmbCurrentSyncRegion.Name = "cmbCurrentSyncRegion";
            this.cmbCurrentSyncRegion.Text = global::Syn3Updater.Properties.Settings.Default.CurrentSyncRegion;
            this.cmbCurrentSyncRegion.SelectedIndexChanged += new System.EventHandler(this.cmbCurrentSyncRegion_SelectedIndexChanged);
            // 
            // lblWarning1
            // 
            this.lblWarning1.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.lblWarning1.BackColor = System.Drawing.Color.DarkRed;
            this.lblWarning1.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            resources.ApplyResources(this.lblWarning1, "lblWarning1");
            this.lblWarning1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblWarning1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.lblWarning1.Name = "lblWarning1";
            this.lblWarning1.VisitedLinkColor = System.Drawing.Color.Silver;
            this.lblWarning1.Click += new System.EventHandler(this.lblWarning1_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnWindowControls_MouseLeave);
            this.btnClose.MouseHover += new System.EventHandler(this.btnClose_MouseHover);
            // 
            // btnLogo
            // 
            this.btnLogo.BackColor = System.Drawing.Color.Transparent;
            this.btnLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogo.Image = global::Syn3Updater.Properties.Resources.logo;
            resources.ApplyResources(this.btnLogo, "btnLogo");
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.TabStop = false;
            this.btnLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.panelTitleBar.Controls.Add(this.cmbLocale);
            this.panelTitleBar.Controls.Add(this.btnLogo);
            this.panelTitleBar.Controls.Add(this.btnClose);
            this.panelTitleBar.Controls.Add(this.lblTitle);
            resources.ApplyResources(this.panelTitleBar, "panelTitleBar");
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // cmbLocale
            // 
            this.cmbLocale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.cmbLocale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbLocale, "cmbLocale");
            this.cmbLocale.ForeColor = System.Drawing.Color.White;
            this.cmbLocale.FormattingEnabled = true;
            this.cmbLocale.Items.AddRange(new object[] {
            resources.GetString("cmbLocale.Items"),
            resources.GetString("cmbLocale.Items1"),
            resources.GetString("cmbLocale.Items2"),
            resources.GetString("cmbLocale.Items3"),
            resources.GetString("cmbLocale.Items4"),
            resources.GetString("cmbLocale.Items5")});
            this.cmbLocale.Name = "cmbLocale";
            this.cmbLocale.SelectedIndexChanged += new System.EventHandler(this.cmbLocale_SelectedIndexChanged);
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // folderDownloads
            // 
            this.folderDownloads.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // grpAdvanced
            // 
            this.grpAdvanced.Controls.Add(this.chkAllReleases);
            this.grpAdvanced.Controls.Add(this.label1);
            this.grpAdvanced.Controls.Add(this.cmbOverride);
            this.grpAdvanced.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpAdvanced, "grpAdvanced");
            this.grpAdvanced.Name = "grpAdvanced";
            this.grpAdvanced.TabStop = false;
            // 
            // chkAllReleases
            // 
            this.chkAllReleases.Checked = global::Syn3Updater.Properties.Settings.Default.ShowAllReleases;
            this.chkAllReleases.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Syn3Updater.Properties.Settings.Default, "ShowAllReleases", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.chkAllReleases, "chkAllReleases");
            this.chkAllReleases.Name = "chkAllReleases";
            this.chkAllReleases.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // cmbOverride
            // 
            this.cmbOverride.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.cmbOverride.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Syn3Updater.Properties.Settings.Default, "ForcedInstallMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbOverride, "cmbOverride");
            this.cmbOverride.ForeColor = System.Drawing.Color.White;
            this.cmbOverride.FormattingEnabled = true;
            this.cmbOverride.Items.AddRange(new object[] {
            resources.GetString("cmbOverride.Items"),
            resources.GetString("cmbOverride.Items1"),
            resources.GetString("cmbOverride.Items2"),
            resources.GetString("cmbOverride.Items3")});
            this.cmbOverride.Name = "cmbOverride";
            this.cmbOverride.Text = global::Syn3Updater.Properties.Settings.Default.ForcedInstallMode;
            this.cmbOverride.SelectedIndexChanged += new System.EventHandler(this.cmbOverride_SelectedIndexChanged);
            // 
            // lblReleaseKey
            // 
            resources.ApplyResources(this.lblReleaseKey, "lblReleaseKey");
            this.lblReleaseKey.Name = "lblReleaseKey";
            // 
            // grpPurchase
            // 
            this.grpPurchase.Controls.Add(this.lblPurchaseInfo);
            this.grpPurchase.Controls.Add(this.btnPurchaseActivate);
            this.grpPurchase.Controls.Add(this.txtReleaseKey);
            this.grpPurchase.Controls.Add(this.lblReleaseKey);
            this.grpPurchase.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpPurchase, "grpPurchase");
            this.grpPurchase.Name = "grpPurchase";
            this.grpPurchase.TabStop = false;
            // 
            // lblPurchaseInfo
            // 
            this.lblPurchaseInfo.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.lblPurchaseInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblPurchaseInfo.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            resources.ApplyResources(this.lblPurchaseInfo, "lblPurchaseInfo");
            this.lblPurchaseInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblPurchaseInfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.lblPurchaseInfo.Name = "lblPurchaseInfo";
            this.lblPurchaseInfo.VisitedLinkColor = System.Drawing.Color.Silver;
            // 
            // btnPurchaseActivate
            // 
            resources.ApplyResources(this.btnPurchaseActivate, "btnPurchaseActivate");
            this.btnPurchaseActivate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnPurchaseActivate.FlatAppearance.BorderSize = 0;
            this.btnPurchaseActivate.Name = "btnPurchaseActivate";
            this.btnPurchaseActivate.UseVisualStyleBackColor = false;
            // 
            // txtReleaseKey
            // 
            this.txtReleaseKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(33)))), ((int)(((byte)(38)))));
            this.txtReleaseKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReleaseKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Syn3Updater.Properties.Settings.Default, "LicenseKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtReleaseKey.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtReleaseKey, "txtReleaseKey");
            this.txtReleaseKey.Name = "txtReleaseKey";
            this.txtReleaseKey.ReadOnly = true;
            this.txtReleaseKey.Text = global::Syn3Updater.Properties.Settings.Default.LicenseKey;
            // 
            // FrmSetup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.Controls.Add(this.grpPurchase);
            this.Controls.Add(this.grpAdvanced);
            this.Controls.Add(this.lblWarning1);
            this.Controls.Add(this.grpDownload);
            this.Controls.Add(this.grpExistingDetails);
            this.Controls.Add(this.btnSetupContinue);
            this.Controls.Add(this.panelTitleBar);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSetup";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FrmSetup_Shown);
            this.grpDownload.ResumeLayout(false);
            this.grpDownload.PerformLayout();
            this.grpExistingDetails.ResumeLayout(false);
            this.grpExistingDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogo)).EndInit();
            this.panelTitleBar.ResumeLayout(false);
            this.grpAdvanced.ResumeLayout(false);
            this.grpAdvanced.PerformLayout();
            this.grpPurchase.ResumeLayout(false);
            this.grpPurchase.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSetupContinue;
        private System.Windows.Forms.GroupBox grpDownload;
        private System.Windows.Forms.TextBox txtDownloadPath;
        private System.Windows.Forms.Button btnChangeDownloadDirectory;
        private System.Windows.Forms.GroupBox grpExistingDetails;
        private System.Windows.Forms.CheckBox chkCurrentSyncNav;
        private System.Windows.Forms.Label lblCurrentSyncVersion;
        private System.Windows.Forms.Label lblCurrentSyncRegion;
        private System.Windows.Forms.ComboBox cmbCurrentSyncRegion;
        private System.Windows.Forms.LinkLabel lblWarning1;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnLogo;
        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.MaskedTextBox txtCurrentSyncVersion;
        private System.Windows.Forms.FolderBrowserDialog folderDownloads;
        private System.Windows.Forms.ComboBox cmbLocale;
        private System.Windows.Forms.GroupBox grpAdvanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbOverride;
        private System.Windows.Forms.CheckBox chkAllReleases;
        private System.Windows.Forms.TextBox txtReleaseKey;
        private System.Windows.Forms.Label lblReleaseKey;
        private System.Windows.Forms.GroupBox grpPurchase;
        private System.Windows.Forms.LinkLabel lblPurchaseInfo;
        private System.Windows.Forms.Button btnPurchaseActivate;
    }
}