namespace PlaneAlerter {
	partial class SettingsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.senderEmailTextBox = new System.Windows.Forms.TextBox();
			this.aircraftListTextBox = new System.Windows.Forms.TextBox();
			this.VRSUsrTextBox = new System.Windows.Forms.TextBox();
			this.removalTimeoutTextBox = new System.Windows.Forms.NumericUpDown();
			this.smtpHostComboBox = new System.Windows.Forms.ComboBox();
			this.smtpHostPortTextBox = new System.Windows.Forms.NumericUpDown();
			this.smtpUsrTextBox = new System.Windows.Forms.TextBox();
			this.VRSPwdTextBox = new System.Windows.Forms.MaskedTextBox();
			this.smtpPwdTextBox = new System.Windows.Forms.MaskedTextBox();
			this.smtpSSLCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.runOnStartupCheckBox = new System.Windows.Forms.CheckBox();
			this.refreshLabel = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.latLabel = new System.Windows.Forms.Label();
			this.longLabel = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.soundAlertsCheckBox = new System.Windows.Forms.CheckBox();
			this.notificationsCheckBox = new System.Windows.Forms.CheckBox();
			this.startOnStartCheckBox = new System.Windows.Forms.CheckBox();
			this.gmailLink = new System.Windows.Forms.LinkLabel();
			this.label14 = new System.Windows.Forms.Label();
			this.filterDstCheckBox = new System.Windows.Forms.CheckBox();
			this.filterAltCheckBox = new System.Windows.Forms.CheckBox();
			this.ignoreModeSCheckBox = new System.Windows.Forms.CheckBox();
			this.saveSettingsButton = new System.Windows.Forms.Button();
			this.emailGroupBox = new System.Windows.Forms.GroupBox();
			this.radarGroupBox = new System.Windows.Forms.GroupBox();
			this.ignoreDistTextBox = new System.Windows.Forms.NumericUpDown();
			this.longTextBox = new System.Windows.Forms.NumericUpDown();
			this.ignoreAltTextBox = new System.Windows.Forms.NumericUpDown();
			this.latTextBox = new System.Windows.Forms.NumericUpDown();
			this.timeoutTextBox = new System.Windows.Forms.NumericUpDown();
			this.radarURLTextBox = new System.Windows.Forms.TextBox();
			this.programGroupBox = new System.Windows.Forms.GroupBox();
			this.centreLatLngRadioButton = new System.Windows.Forms.RadioButton();
			this.centreAircraftRadioButton = new System.Windows.Forms.RadioButton();
			this.refreshTextBox = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.removalTimeoutTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.smtpHostPortTextBox)).BeginInit();
			this.emailGroupBox.SuspendLayout();
			this.radarGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ignoreDistTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.longTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreAltTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.latTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timeoutTextBox)).BeginInit();
			this.programGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.refreshTextBox)).BeginInit();
			this.SuspendLayout();
			// 
			// senderEmailTextBox
			// 
			this.senderEmailTextBox.Location = new System.Drawing.Point(142, 22);
			this.senderEmailTextBox.Name = "senderEmailTextBox";
			this.senderEmailTextBox.Size = new System.Drawing.Size(178, 20);
			this.senderEmailTextBox.TabIndex = 0;
			// 
			// aircraftListTextBox
			// 
			this.aircraftListTextBox.Location = new System.Drawing.Point(142, 22);
			this.aircraftListTextBox.Name = "aircraftListTextBox";
			this.aircraftListTextBox.Size = new System.Drawing.Size(178, 20);
			this.aircraftListTextBox.TabIndex = 1;
			// 
			// VRSUsrTextBox
			// 
			this.VRSUsrTextBox.Location = new System.Drawing.Point(142, 100);
			this.VRSUsrTextBox.Name = "VRSUsrTextBox";
			this.VRSUsrTextBox.Size = new System.Drawing.Size(178, 20);
			this.VRSUsrTextBox.TabIndex = 3;
			// 
			// removalTimeoutTextBox
			// 
			this.removalTimeoutTextBox.Location = new System.Drawing.Point(245, 45);
			this.removalTimeoutTextBox.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
			this.removalTimeoutTextBox.Name = "removalTimeoutTextBox";
			this.removalTimeoutTextBox.Size = new System.Drawing.Size(78, 20);
			this.removalTimeoutTextBox.TabIndex = 4;
			// 
			// smtpHostComboBox
			// 
			this.smtpHostComboBox.FormattingEnabled = true;
			this.smtpHostComboBox.Location = new System.Drawing.Point(142, 48);
			this.smtpHostComboBox.Name = "smtpHostComboBox";
			this.smtpHostComboBox.Size = new System.Drawing.Size(178, 21);
			this.smtpHostComboBox.TabIndex = 5;
			this.smtpHostComboBox.SelectedValueChanged += new System.EventHandler(this.smtpHostComboBox_SelectedValueChanged);
			// 
			// smtpHostPortTextBox
			// 
			this.smtpHostPortTextBox.Location = new System.Drawing.Point(245, 75);
			this.smtpHostPortTextBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.smtpHostPortTextBox.Name = "smtpHostPortTextBox";
			this.smtpHostPortTextBox.Size = new System.Drawing.Size(75, 20);
			this.smtpHostPortTextBox.TabIndex = 6;
			// 
			// smtpUsrTextBox
			// 
			this.smtpUsrTextBox.Location = new System.Drawing.Point(142, 101);
			this.smtpUsrTextBox.Name = "smtpUsrTextBox";
			this.smtpUsrTextBox.Size = new System.Drawing.Size(178, 20);
			this.smtpUsrTextBox.TabIndex = 7;
			// 
			// VRSPwdTextBox
			// 
			this.VRSPwdTextBox.Location = new System.Drawing.Point(142, 126);
			this.VRSPwdTextBox.Name = "VRSPwdTextBox";
			this.VRSPwdTextBox.PasswordChar = '*';
			this.VRSPwdTextBox.Size = new System.Drawing.Size(178, 20);
			this.VRSPwdTextBox.TabIndex = 8;
			// 
			// smtpPwdTextBox
			// 
			this.smtpPwdTextBox.Location = new System.Drawing.Point(142, 127);
			this.smtpPwdTextBox.Name = "smtpPwdTextBox";
			this.smtpPwdTextBox.PasswordChar = '*';
			this.smtpPwdTextBox.Size = new System.Drawing.Size(178, 20);
			this.smtpPwdTextBox.TabIndex = 9;
			// 
			// smtpSSLCheckBox
			// 
			this.smtpSSLCheckBox.AutoSize = true;
			this.smtpSSLCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.smtpSSLCheckBox.Location = new System.Drawing.Point(6, 153);
			this.smtpSSLCheckBox.Name = "smtpSSLCheckBox";
			this.smtpSSLCheckBox.Size = new System.Drawing.Size(79, 17);
			this.smtpSSLCheckBox.TabIndex = 10;
			this.smtpSSLCheckBox.Text = "SMTP SSL";
			this.toolTip.SetToolTip(this.smtpSSLCheckBox, "Use SSL for the SMTP server. This is usually enabled for gmail and popular server" +
        "s");
			this.smtpSSLCheckBox.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Email to send from";
			this.toolTip.SetToolTip(this.label1, "This should be a valid email on the smtp server specified");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "AircraftList.json Url";
			this.toolTip.SetToolTip(this.label2, "This should be http://*domain/ip*/VirtualRadar/AircraftList.json");
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 0;
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 100;
			this.toolTip.UseAnimation = false;
			this.toolTip.UseFading = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 103);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124, 13);
			this.label3.TabIndex = 13;
			this.label3.Text = "Username for VRS Login";
			this.toolTip.SetToolTip(this.label3, "Leave blank for unpassworded servers");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 129);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(122, 13);
			this.label4.TabIndex = 14;
			this.label4.Text = "Password for VRS Login";
			this.toolTip.SetToolTip(this.label4, "Leave blank for unpassworded servers");
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 47);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(157, 13);
			this.label5.TabIndex = 15;
			this.label5.Text = "Aircraft Removal Timeout (secs)";
			this.toolTip.SetToolTip(this.label5, "This is how long it takes for it to remove a plane after signal is lost.");
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 51);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(62, 13);
			this.label6.TabIndex = 16;
			this.label6.Text = "SMTP Host";
			this.toolTip.SetToolTip(this.label6, "This is the server that the email is hosted on");
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 77);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(84, 13);
			this.label7.TabIndex = 17;
			this.label7.Text = "SMTP Host Port";
			this.toolTip.SetToolTip(this.label7, "This is the port that the SMTP server uses");
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 104);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 13);
			this.label8.TabIndex = 18;
			this.label8.Text = "SMTP Username";
			this.toolTip.SetToolTip(this.label8, "The username to login. This is usually the sending email address.");
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 130);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(86, 13);
			this.label9.TabIndex = 19;
			this.label9.Text = "SMTP Password";
			this.toolTip.SetToolTip(this.label9, "The password to login.");
			// 
			// runOnStartupCheckBox
			// 
			this.runOnStartupCheckBox.AutoSize = true;
			this.runOnStartupCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.runOnStartupCheckBox.Location = new System.Drawing.Point(6, 71);
			this.runOnStartupCheckBox.Name = "runOnStartupCheckBox";
			this.runOnStartupCheckBox.Size = new System.Drawing.Size(100, 17);
			this.runOnStartupCheckBox.TabIndex = 21;
			this.runOnStartupCheckBox.Text = "Run On Startup";
			this.toolTip.SetToolTip(this.runOnStartupCheckBox, "Run PlaneAlerter when this user logs in");
			this.runOnStartupCheckBox.UseVisualStyleBackColor = true;
			// 
			// refreshLabel
			// 
			this.refreshLabel.AutoSize = true;
			this.refreshLabel.Location = new System.Drawing.Point(8, 21);
			this.refreshLabel.Name = "refreshLabel";
			this.refreshLabel.Size = new System.Drawing.Size(107, 13);
			this.refreshLabel.TabIndex = 23;
			this.refreshLabel.Text = "Check Interval (secs)";
			this.toolTip.SetToolTip(this.refreshLabel, "The time in seconds between each check");
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 77);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(61, 13);
			this.label10.TabIndex = 16;
			this.label10.Text = "Radar URL";
			this.toolTip.SetToolTip(this.label10, "This should be http://*domain/ip*/VirtualRadar/\r\nThis URL is used for the radar l" +
        "ink in the alert emails");
			// 
			// latLabel
			// 
			this.latLabel.AutoSize = true;
			this.latLabel.Location = new System.Drawing.Point(8, 153);
			this.latLabel.Name = "latLabel";
			this.latLabel.Size = new System.Drawing.Size(45, 13);
			this.latLabel.TabIndex = 17;
			this.latLabel.Text = "Latitude";
			this.toolTip.SetToolTip(this.latLabel, "Latitude used for distance measurement");
			// 
			// longLabel
			// 
			this.longLabel.AutoSize = true;
			this.longLabel.Location = new System.Drawing.Point(185, 153);
			this.longLabel.Name = "longLabel";
			this.longLabel.Size = new System.Drawing.Size(54, 13);
			this.longLabel.TabIndex = 22;
			this.longLabel.Text = "Longitude";
			this.toolTip.SetToolTip(this.longLabel, "Longitude used for distance measurement");
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 50);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(193, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "AircraftList.json Request Timeout (secs)";
			this.toolTip.SetToolTip(this.label11, "Request timeout for aircraftlist.json. Increasing this allows longer aircraft lis" +
        "ts to not time out");
			// 
			// soundAlertsCheckBox
			// 
			this.soundAlertsCheckBox.AutoSize = true;
			this.soundAlertsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.soundAlertsCheckBox.Location = new System.Drawing.Point(6, 94);
			this.soundAlertsCheckBox.Name = "soundAlertsCheckBox";
			this.soundAlertsCheckBox.Size = new System.Drawing.Size(119, 17);
			this.soundAlertsCheckBox.TabIndex = 26;
			this.soundAlertsCheckBox.Text = "Play Sound on Alert";
			this.toolTip.SetToolTip(this.soundAlertsCheckBox, "Play a sound on alert");
			this.soundAlertsCheckBox.UseVisualStyleBackColor = true;
			// 
			// notificationsCheckBox
			// 
			this.notificationsCheckBox.AutoSize = true;
			this.notificationsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.notificationsCheckBox.Location = new System.Drawing.Point(172, 94);
			this.notificationsCheckBox.Name = "notificationsCheckBox";
			this.notificationsCheckBox.Size = new System.Drawing.Size(148, 17);
			this.notificationsCheckBox.TabIndex = 27;
			this.notificationsCheckBox.Text = "Show Notification on Alert";
			this.toolTip.SetToolTip(this.notificationsCheckBox, "Show a notification on alert");
			this.notificationsCheckBox.UseVisualStyleBackColor = true;
			// 
			// startOnStartCheckBox
			// 
			this.startOnStartCheckBox.AutoSize = true;
			this.startOnStartCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.startOnStartCheckBox.Location = new System.Drawing.Point(147, 71);
			this.startOnStartCheckBox.Name = "startOnStartCheckBox";
			this.startOnStartCheckBox.Size = new System.Drawing.Size(173, 17);
			this.startOnStartCheckBox.TabIndex = 28;
			this.startOnStartCheckBox.Text = "Start Checker on Program Start";
			this.toolTip.SetToolTip(this.startOnStartCheckBox, "Start checking for condition matches as soon as PlaneAlerter starts");
			this.startOnStartCheckBox.UseVisualStyleBackColor = true;
			// 
			// gmailLink
			// 
			this.gmailLink.AutoSize = true;
			this.gmailLink.Location = new System.Drawing.Point(136, 154);
			this.gmailLink.Name = "gmailLink";
			this.gmailLink.Size = new System.Drawing.Size(184, 13);
			this.gmailLink.TabIndex = 20;
			this.gmailLink.TabStop = true;
			this.gmailLink.Text = "Enable this thing if sending with Gmail";
			this.toolTip.SetToolTip(this.gmailLink, resources.GetString("gmailLink.ToolTip"));
			this.gmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gmailLink_LinkClicked);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(8, 119);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(81, 13);
			this.label14.TabIndex = 29;
			this.label14.Text = "Centre maps on";
			this.toolTip.SetToolTip(this.label14, "Centre maps on aircraft or provided lat/lng");
			// 
			// filterDstCheckBox
			// 
			this.filterDstCheckBox.AutoSize = true;
			this.filterDstCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.filterDstCheckBox.Location = new System.Drawing.Point(6, 204);
			this.filterDstCheckBox.Name = "filterDstCheckBox";
			this.filterDstCheckBox.Size = new System.Drawing.Size(171, 17);
			this.filterDstCheckBox.TabIndex = 30;
			this.filterDstCheckBox.Text = "Ignore aircraft further than (km)";
			this.toolTip.SetToolTip(this.filterDstCheckBox, "Aircraft further than this distance will not be returned by VRS, this can be usef" +
        "ul for reducing bandwidth. Enabling this and altitude filtering at the same time" +
        " with the VRS 3.0.0 beta may not work.");
			this.filterDstCheckBox.UseVisualStyleBackColor = true;
			this.filterDstCheckBox.CheckedChanged += new System.EventHandler(this.filterDstCheckBox_CheckedChanged);
			// 
			// filterAltCheckBox
			// 
			this.filterAltCheckBox.AutoSize = true;
			this.filterAltCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.filterAltCheckBox.Location = new System.Drawing.Point(6, 177);
			this.filterAltCheckBox.Name = "filterAltCheckBox";
			this.filterAltCheckBox.Size = new System.Drawing.Size(162, 17);
			this.filterAltCheckBox.TabIndex = 31;
			this.filterAltCheckBox.Text = "Ignore aircraft higher than (ft)";
			this.toolTip.SetToolTip(this.filterAltCheckBox, "Aircraft higher than this altitude will not be returned by VRS, this can be usefu" +
        "l for reducing bandwidth. Enabling this and distance filtering at the same time " +
        "with the VRS 3.0.0 beta may not work.");
			this.filterAltCheckBox.UseVisualStyleBackColor = true;
			this.filterAltCheckBox.CheckedChanged += new System.EventHandler(this.filterAltCheckBox_CheckedChanged);
			// 
			// ignoreModeSCheckBox
			// 
			this.ignoreModeSCheckBox.AutoSize = true;
			this.ignoreModeSCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ignoreModeSCheckBox.Location = new System.Drawing.Point(153, 229);
			this.ignoreModeSCheckBox.Name = "ignoreModeSCheckBox";
			this.ignoreModeSCheckBox.Size = new System.Drawing.Size(167, 17);
			this.ignoreModeSCheckBox.TabIndex = 32;
			this.ignoreModeSCheckBox.Text = "Ignore aircraft without position";
			this.toolTip.SetToolTip(this.ignoreModeSCheckBox, "VRS ignores aircraft without positions when filtering by distance. Disabling this" +
        " requests aircraft without position separately.");
			this.ignoreModeSCheckBox.UseVisualStyleBackColor = true;
			// 
			// saveSettingsButton
			// 
			this.saveSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveSettingsButton.Location = new System.Drawing.Point(573, 319);
			this.saveSettingsButton.Name = "saveSettingsButton";
			this.saveSettingsButton.Size = new System.Drawing.Size(103, 23);
			this.saveSettingsButton.TabIndex = 20;
			this.saveSettingsButton.Text = "Save Settings";
			this.saveSettingsButton.UseVisualStyleBackColor = true;
			this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
			// 
			// emailGroupBox
			// 
			this.emailGroupBox.Controls.Add(this.gmailLink);
			this.emailGroupBox.Controls.Add(this.label1);
			this.emailGroupBox.Controls.Add(this.senderEmailTextBox);
			this.emailGroupBox.Controls.Add(this.smtpHostComboBox);
			this.emailGroupBox.Controls.Add(this.label9);
			this.emailGroupBox.Controls.Add(this.smtpHostPortTextBox);
			this.emailGroupBox.Controls.Add(this.label8);
			this.emailGroupBox.Controls.Add(this.smtpUsrTextBox);
			this.emailGroupBox.Controls.Add(this.label7);
			this.emailGroupBox.Controls.Add(this.smtpPwdTextBox);
			this.emailGroupBox.Controls.Add(this.label6);
			this.emailGroupBox.Controls.Add(this.smtpSSLCheckBox);
			this.emailGroupBox.Location = new System.Drawing.Point(12, 165);
			this.emailGroupBox.Name = "emailGroupBox";
			this.emailGroupBox.Size = new System.Drawing.Size(329, 179);
			this.emailGroupBox.TabIndex = 22;
			this.emailGroupBox.TabStop = false;
			this.emailGroupBox.Text = "Email";
			// 
			// radarGroupBox
			// 
			this.radarGroupBox.Controls.Add(this.ignoreModeSCheckBox);
			this.radarGroupBox.Controls.Add(this.filterAltCheckBox);
			this.radarGroupBox.Controls.Add(this.filterDstCheckBox);
			this.radarGroupBox.Controls.Add(this.ignoreDistTextBox);
			this.radarGroupBox.Controls.Add(this.longTextBox);
			this.radarGroupBox.Controls.Add(this.ignoreAltTextBox);
			this.radarGroupBox.Controls.Add(this.longLabel);
			this.radarGroupBox.Controls.Add(this.latTextBox);
			this.radarGroupBox.Controls.Add(this.timeoutTextBox);
			this.radarGroupBox.Controls.Add(this.latLabel);
			this.radarGroupBox.Controls.Add(this.label10);
			this.radarGroupBox.Controls.Add(this.radarURLTextBox);
			this.radarGroupBox.Controls.Add(this.label2);
			this.radarGroupBox.Controls.Add(this.aircraftListTextBox);
			this.radarGroupBox.Controls.Add(this.VRSUsrTextBox);
			this.radarGroupBox.Controls.Add(this.VRSPwdTextBox);
			this.radarGroupBox.Controls.Add(this.label3);
			this.radarGroupBox.Controls.Add(this.label4);
			this.radarGroupBox.Controls.Add(this.label11);
			this.radarGroupBox.Location = new System.Drawing.Point(347, 12);
			this.radarGroupBox.Name = "radarGroupBox";
			this.radarGroupBox.Size = new System.Drawing.Size(329, 254);
			this.radarGroupBox.TabIndex = 23;
			this.radarGroupBox.TabStop = false;
			this.radarGroupBox.Text = "Radar";
			// 
			// ignoreDistTextBox
			// 
			this.ignoreDistTextBox.DecimalPlaces = 2;
			this.ignoreDistTextBox.Location = new System.Drawing.Point(245, 203);
			this.ignoreDistTextBox.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
			this.ignoreDistTextBox.Name = "ignoreDistTextBox";
			this.ignoreDistTextBox.Size = new System.Drawing.Size(75, 20);
			this.ignoreDistTextBox.TabIndex = 28;
			// 
			// longTextBox
			// 
			this.longTextBox.DecimalPlaces = 4;
			this.longTextBox.Location = new System.Drawing.Point(245, 151);
			this.longTextBox.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.longTextBox.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
			this.longTextBox.Name = "longTextBox";
			this.longTextBox.Size = new System.Drawing.Size(75, 20);
			this.longTextBox.TabIndex = 23;
			// 
			// ignoreAltTextBox
			// 
			this.ignoreAltTextBox.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.ignoreAltTextBox.Location = new System.Drawing.Point(245, 177);
			this.ignoreAltTextBox.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.ignoreAltTextBox.Name = "ignoreAltTextBox";
			this.ignoreAltTextBox.Size = new System.Drawing.Size(75, 20);
			this.ignoreAltTextBox.TabIndex = 29;
			this.ignoreAltTextBox.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			// 
			// latTextBox
			// 
			this.latTextBox.DecimalPlaces = 4;
			this.latTextBox.Location = new System.Drawing.Point(59, 151);
			this.latTextBox.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
			this.latTextBox.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
			this.latTextBox.Name = "latTextBox";
			this.latTextBox.Size = new System.Drawing.Size(75, 20);
			this.latTextBox.TabIndex = 21;
			// 
			// timeoutTextBox
			// 
			this.timeoutTextBox.Location = new System.Drawing.Point(245, 48);
			this.timeoutTextBox.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.timeoutTextBox.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.timeoutTextBox.Name = "timeoutTextBox";
			this.timeoutTextBox.Size = new System.Drawing.Size(75, 20);
			this.timeoutTextBox.TabIndex = 24;
			this.timeoutTextBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// radarURLTextBox
			// 
			this.radarURLTextBox.Location = new System.Drawing.Point(142, 74);
			this.radarURLTextBox.Name = "radarURLTextBox";
			this.radarURLTextBox.Size = new System.Drawing.Size(178, 20);
			this.radarURLTextBox.TabIndex = 15;
			// 
			// programGroupBox
			// 
			this.programGroupBox.Controls.Add(this.centreLatLngRadioButton);
			this.programGroupBox.Controls.Add(this.centreAircraftRadioButton);
			this.programGroupBox.Controls.Add(this.label14);
			this.programGroupBox.Controls.Add(this.startOnStartCheckBox);
			this.programGroupBox.Controls.Add(this.notificationsCheckBox);
			this.programGroupBox.Controls.Add(this.soundAlertsCheckBox);
			this.programGroupBox.Controls.Add(this.refreshLabel);
			this.programGroupBox.Controls.Add(this.refreshTextBox);
			this.programGroupBox.Controls.Add(this.label5);
			this.programGroupBox.Controls.Add(this.removalTimeoutTextBox);
			this.programGroupBox.Controls.Add(this.runOnStartupCheckBox);
			this.programGroupBox.Location = new System.Drawing.Point(12, 12);
			this.programGroupBox.Name = "programGroupBox";
			this.programGroupBox.Size = new System.Drawing.Size(329, 147);
			this.programGroupBox.TabIndex = 24;
			this.programGroupBox.TabStop = false;
			this.programGroupBox.Text = "General";
			// 
			// centreLatLngRadioButton
			// 
			this.centreLatLngRadioButton.AutoSize = true;
			this.centreLatLngRadioButton.Location = new System.Drawing.Point(212, 117);
			this.centreLatLngRadioButton.Name = "centreLatLngRadioButton";
			this.centreLatLngRadioButton.Size = new System.Drawing.Size(108, 17);
			this.centreLatLngRadioButton.TabIndex = 31;
			this.centreLatLngRadioButton.Text = "Provided Lat/Lng";
			this.centreLatLngRadioButton.UseVisualStyleBackColor = true;
			// 
			// centreAircraftRadioButton
			// 
			this.centreAircraftRadioButton.AutoSize = true;
			this.centreAircraftRadioButton.Checked = true;
			this.centreAircraftRadioButton.Location = new System.Drawing.Point(150, 117);
			this.centreAircraftRadioButton.Name = "centreAircraftRadioButton";
			this.centreAircraftRadioButton.Size = new System.Drawing.Size(58, 17);
			this.centreAircraftRadioButton.TabIndex = 30;
			this.centreAircraftRadioButton.TabStop = true;
			this.centreAircraftRadioButton.Text = "Aircraft";
			this.centreAircraftRadioButton.UseVisualStyleBackColor = true;
			// 
			// refreshTextBox
			// 
			this.refreshTextBox.Location = new System.Drawing.Point(245, 19);
			this.refreshTextBox.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.refreshTextBox.Name = "refreshTextBox";
			this.refreshTextBox.Size = new System.Drawing.Size(78, 20);
			this.refreshTextBox.TabIndex = 22;
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(688, 354);
			this.Controls.Add(this.programGroupBox);
			this.Controls.Add(this.radarGroupBox);
			this.Controls.Add(this.emailGroupBox);
			this.Controls.Add(this.saveSettingsButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "SettingsForm";
			this.Text = "Configure Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.removalTimeoutTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.smtpHostPortTextBox)).EndInit();
			this.emailGroupBox.ResumeLayout(false);
			this.emailGroupBox.PerformLayout();
			this.radarGroupBox.ResumeLayout(false);
			this.radarGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ignoreDistTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.longTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreAltTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.latTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timeoutTextBox)).EndInit();
			this.programGroupBox.ResumeLayout(false);
			this.programGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.refreshTextBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox senderEmailTextBox;
		private System.Windows.Forms.TextBox aircraftListTextBox;
		private System.Windows.Forms.TextBox VRSUsrTextBox;
		private System.Windows.Forms.NumericUpDown removalTimeoutTextBox;
		private System.Windows.Forms.ComboBox smtpHostComboBox;
		private System.Windows.Forms.NumericUpDown smtpHostPortTextBox;
		private System.Windows.Forms.TextBox smtpUsrTextBox;
		private System.Windows.Forms.MaskedTextBox VRSPwdTextBox;
		private System.Windows.Forms.MaskedTextBox smtpPwdTextBox;
		private System.Windows.Forms.CheckBox smtpSSLCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button saveSettingsButton;
		private System.Windows.Forms.CheckBox runOnStartupCheckBox;
		private System.Windows.Forms.GroupBox emailGroupBox;
		private System.Windows.Forms.GroupBox radarGroupBox;
		private System.Windows.Forms.GroupBox programGroupBox;
		private System.Windows.Forms.Label refreshLabel;
		private System.Windows.Forms.NumericUpDown refreshTextBox;
		private System.Windows.Forms.LinkLabel gmailLink;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox radarURLTextBox;
		private System.Windows.Forms.NumericUpDown latTextBox;
		private System.Windows.Forms.Label latLabel;
		private System.Windows.Forms.NumericUpDown longTextBox;
		private System.Windows.Forms.Label longLabel;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.NumericUpDown timeoutTextBox;
		private System.Windows.Forms.CheckBox notificationsCheckBox;
		private System.Windows.Forms.CheckBox soundAlertsCheckBox;
		private System.Windows.Forms.CheckBox startOnStartCheckBox;
        private System.Windows.Forms.NumericUpDown ignoreAltTextBox;
        private System.Windows.Forms.NumericUpDown ignoreDistTextBox;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.RadioButton centreLatLngRadioButton;
		private System.Windows.Forms.RadioButton centreAircraftRadioButton;
		private System.Windows.Forms.CheckBox filterAltCheckBox;
		private System.Windows.Forms.CheckBox filterDstCheckBox;
		private System.Windows.Forms.CheckBox ignoreModeSCheckBox;
	}
}