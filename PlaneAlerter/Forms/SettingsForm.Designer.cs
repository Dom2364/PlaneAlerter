namespace PlaneAlerter.Forms {
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
			this.filterReceiverCheckBox = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.flashWindowCheckBox = new System.Windows.Forms.CheckBox();
			this.saveSettingsButton = new System.Windows.Forms.Button();
			this.emailGroupBox = new System.Windows.Forms.GroupBox();
			this.radarGroupBox = new System.Windows.Forms.GroupBox();
			this.trailsAgeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.refreshReceiversButton = new System.Windows.Forms.Button();
			this.receiverComboBox = new System.Windows.Forms.ComboBox();
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.removalTimeoutTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.smtpHostPortTextBox)).BeginInit();
			this.emailGroupBox.SuspendLayout();
			this.radarGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trailsAgeNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreDistTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.longTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreAltTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.latTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timeoutTextBox)).BeginInit();
			this.programGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.refreshTextBox)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// senderEmailTextBox
			// 
			this.senderEmailTextBox.Location = new System.Drawing.Point(166, 25);
			this.senderEmailTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.senderEmailTextBox.Name = "senderEmailTextBox";
			this.senderEmailTextBox.Size = new System.Drawing.Size(207, 23);
			this.senderEmailTextBox.TabIndex = 0;
			// 
			// aircraftListTextBox
			// 
			this.aircraftListTextBox.Location = new System.Drawing.Point(166, 25);
			this.aircraftListTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.aircraftListTextBox.Name = "aircraftListTextBox";
			this.aircraftListTextBox.Size = new System.Drawing.Size(207, 23);
			this.aircraftListTextBox.TabIndex = 1;
			// 
			// VRSUsrTextBox
			// 
			this.VRSUsrTextBox.Location = new System.Drawing.Point(166, 115);
			this.VRSUsrTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.VRSUsrTextBox.Name = "VRSUsrTextBox";
			this.VRSUsrTextBox.Size = new System.Drawing.Size(207, 23);
			this.VRSUsrTextBox.TabIndex = 3;
			// 
			// removalTimeoutTextBox
			// 
			this.removalTimeoutTextBox.Location = new System.Drawing.Point(286, 52);
			this.removalTimeoutTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.removalTimeoutTextBox.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
			this.removalTimeoutTextBox.Name = "removalTimeoutTextBox";
			this.removalTimeoutTextBox.Size = new System.Drawing.Size(91, 23);
			this.removalTimeoutTextBox.TabIndex = 4;
			// 
			// smtpHostComboBox
			// 
			this.smtpHostComboBox.FormattingEnabled = true;
			this.smtpHostComboBox.Location = new System.Drawing.Point(166, 55);
			this.smtpHostComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.smtpHostComboBox.Name = "smtpHostComboBox";
			this.smtpHostComboBox.Size = new System.Drawing.Size(207, 23);
			this.smtpHostComboBox.TabIndex = 5;
			this.smtpHostComboBox.SelectedValueChanged += new System.EventHandler(this.smtpHostComboBox_SelectedValueChanged);
			// 
			// smtpHostPortTextBox
			// 
			this.smtpHostPortTextBox.Location = new System.Drawing.Point(286, 87);
			this.smtpHostPortTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.smtpHostPortTextBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.smtpHostPortTextBox.Name = "smtpHostPortTextBox";
			this.smtpHostPortTextBox.Size = new System.Drawing.Size(88, 23);
			this.smtpHostPortTextBox.TabIndex = 6;
			// 
			// smtpUsrTextBox
			// 
			this.smtpUsrTextBox.Location = new System.Drawing.Point(166, 117);
			this.smtpUsrTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.smtpUsrTextBox.Name = "smtpUsrTextBox";
			this.smtpUsrTextBox.Size = new System.Drawing.Size(207, 23);
			this.smtpUsrTextBox.TabIndex = 7;
			// 
			// VRSPwdTextBox
			// 
			this.VRSPwdTextBox.Location = new System.Drawing.Point(166, 145);
			this.VRSPwdTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.VRSPwdTextBox.Name = "VRSPwdTextBox";
			this.VRSPwdTextBox.PasswordChar = '*';
			this.VRSPwdTextBox.Size = new System.Drawing.Size(207, 23);
			this.VRSPwdTextBox.TabIndex = 8;
			// 
			// smtpPwdTextBox
			// 
			this.smtpPwdTextBox.Location = new System.Drawing.Point(166, 147);
			this.smtpPwdTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.smtpPwdTextBox.Name = "smtpPwdTextBox";
			this.smtpPwdTextBox.PasswordChar = '*';
			this.smtpPwdTextBox.Size = new System.Drawing.Size(207, 23);
			this.smtpPwdTextBox.TabIndex = 9;
			// 
			// smtpSSLCheckBox
			// 
			this.smtpSSLCheckBox.AutoSize = true;
			this.smtpSSLCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.smtpSSLCheckBox.Location = new System.Drawing.Point(7, 177);
			this.smtpSSLCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.smtpSSLCheckBox.Name = "smtpSSLCheckBox";
			this.smtpSSLCheckBox.Size = new System.Drawing.Size(77, 19);
			this.smtpSSLCheckBox.TabIndex = 10;
			this.smtpSSLCheckBox.Text = "SMTP SSL";
			this.toolTip.SetToolTip(this.smtpSSLCheckBox, "Use SSL for the SMTP server. This is usually enabled for gmail and popular server" +
        "s");
			this.smtpSSLCheckBox.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 29);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 15);
			this.label1.TabIndex = 11;
			this.label1.Text = "Email to send from";
			this.toolTip.SetToolTip(this.label1, "This should be a valid email on the smtp server specified");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 29);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 15);
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
			this.label3.Location = new System.Drawing.Point(7, 119);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(134, 15);
			this.label3.TabIndex = 13;
			this.label3.Text = "Username for VRS Login";
			this.toolTip.SetToolTip(this.label3, "Leave blank for unpassworded servers");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 149);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 15);
			this.label4.TabIndex = 14;
			this.label4.Text = "Password for VRS Login";
			this.toolTip.SetToolTip(this.label4, "Leave blank for unpassworded servers");
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 54);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(175, 15);
			this.label5.TabIndex = 15;
			this.label5.Text = "Aircraft Removal Timeout (secs)";
			this.toolTip.SetToolTip(this.label5, "This is how long it takes for it to remove a plane after signal is lost.");
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(7, 59);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 15);
			this.label6.TabIndex = 16;
			this.label6.Text = "SMTP Host";
			this.toolTip.SetToolTip(this.label6, "This is the server that the email is hosted on");
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 89);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 15);
			this.label7.TabIndex = 17;
			this.label7.Text = "SMTP Host Port";
			this.toolTip.SetToolTip(this.label7, "This is the port that the SMTP server uses");
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(7, 120);
			this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(93, 15);
			this.label8.TabIndex = 18;
			this.label8.Text = "SMTP Username";
			this.toolTip.SetToolTip(this.label8, "The username to login. This is usually the sending email address.");
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(7, 150);
			this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(90, 15);
			this.label9.TabIndex = 19;
			this.label9.Text = "SMTP Password";
			this.toolTip.SetToolTip(this.label9, "The password to login.");
			// 
			// runOnStartupCheckBox
			// 
			this.runOnStartupCheckBox.AutoSize = true;
			this.runOnStartupCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.runOnStartupCheckBox.Location = new System.Drawing.Point(7, 82);
			this.runOnStartupCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.runOnStartupCheckBox.Name = "runOnStartupCheckBox";
			this.runOnStartupCheckBox.Size = new System.Drawing.Size(107, 19);
			this.runOnStartupCheckBox.TabIndex = 21;
			this.runOnStartupCheckBox.Text = "Run On Startup";
			this.toolTip.SetToolTip(this.runOnStartupCheckBox, "Run PlaneAlerter when this user logs in");
			this.runOnStartupCheckBox.UseVisualStyleBackColor = true;
			// 
			// refreshLabel
			// 
			this.refreshLabel.AutoSize = true;
			this.refreshLabel.Location = new System.Drawing.Point(8, 24);
			this.refreshLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.refreshLabel.Name = "refreshLabel";
			this.refreshLabel.Size = new System.Drawing.Size(115, 15);
			this.refreshLabel.TabIndex = 23;
			this.refreshLabel.Text = "Check Interval (secs)";
			this.toolTip.SetToolTip(this.refreshLabel, "The time in seconds between each check");
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(7, 89);
			this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(61, 15);
			this.label10.TabIndex = 16;
			this.label10.Text = "Radar URL";
			this.toolTip.SetToolTip(this.label10, "This should be http://*domain/ip*/VirtualRadar/\r\nThis URL is used for the radar l" +
        "ink in the alert emails");
			// 
			// latLabel
			// 
			this.latLabel.AutoSize = true;
			this.latLabel.Location = new System.Drawing.Point(9, 177);
			this.latLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.latLabel.Name = "latLabel";
			this.latLabel.Size = new System.Drawing.Size(50, 15);
			this.latLabel.TabIndex = 17;
			this.latLabel.Text = "Latitude";
			this.toolTip.SetToolTip(this.latLabel, "Latitude used for distance measurement");
			// 
			// longLabel
			// 
			this.longLabel.AutoSize = true;
			this.longLabel.Location = new System.Drawing.Point(216, 177);
			this.longLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.longLabel.Name = "longLabel";
			this.longLabel.Size = new System.Drawing.Size(61, 15);
			this.longLabel.TabIndex = 22;
			this.longLabel.Text = "Longitude";
			this.toolTip.SetToolTip(this.longLabel, "Longitude used for distance measurement");
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(7, 58);
			this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(214, 15);
			this.label11.TabIndex = 25;
			this.label11.Text = "AircraftList.json Request Timeout (secs)";
			this.toolTip.SetToolTip(this.label11, "Request timeout for aircraftlist.json. Increasing this allows longer aircraft lis" +
        "ts to not time out");
			// 
			// soundAlertsCheckBox
			// 
			this.soundAlertsCheckBox.AutoSize = true;
			this.soundAlertsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.soundAlertsCheckBox.Location = new System.Drawing.Point(8, 20);
			this.soundAlertsCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.soundAlertsCheckBox.Name = "soundAlertsCheckBox";
			this.soundAlertsCheckBox.Size = new System.Drawing.Size(85, 19);
			this.soundAlertsCheckBox.TabIndex = 26;
			this.soundAlertsCheckBox.Text = "Play Sound";
			this.toolTip.SetToolTip(this.soundAlertsCheckBox, "Play a sound on alert");
			this.soundAlertsCheckBox.UseVisualStyleBackColor = true;
			// 
			// notificationsCheckBox
			// 
			this.notificationsCheckBox.AutoSize = true;
			this.notificationsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.notificationsCheckBox.Location = new System.Drawing.Point(125, 20);
			this.notificationsCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.notificationsCheckBox.Name = "notificationsCheckBox";
			this.notificationsCheckBox.Size = new System.Drawing.Size(121, 19);
			this.notificationsCheckBox.TabIndex = 27;
			this.notificationsCheckBox.Text = "Show Notification";
			this.toolTip.SetToolTip(this.notificationsCheckBox, "Show a notification on alert");
			this.notificationsCheckBox.UseVisualStyleBackColor = true;
			// 
			// startOnStartCheckBox
			// 
			this.startOnStartCheckBox.AutoSize = true;
			this.startOnStartCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.startOnStartCheckBox.Location = new System.Drawing.Point(184, 82);
			this.startOnStartCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.startOnStartCheckBox.Name = "startOnStartCheckBox";
			this.startOnStartCheckBox.Size = new System.Drawing.Size(189, 19);
			this.startOnStartCheckBox.TabIndex = 28;
			this.startOnStartCheckBox.Text = "Start Checker on Program Start";
			this.toolTip.SetToolTip(this.startOnStartCheckBox, "Start checking for condition matches as soon as PlaneAlerter starts");
			this.startOnStartCheckBox.UseVisualStyleBackColor = true;
			// 
			// gmailLink
			// 
			this.gmailLink.AutoSize = true;
			this.gmailLink.Location = new System.Drawing.Point(159, 178);
			this.gmailLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.gmailLink.Name = "gmailLink";
			this.gmailLink.Size = new System.Drawing.Size(210, 15);
			this.gmailLink.TabIndex = 20;
			this.gmailLink.TabStop = true;
			this.gmailLink.Text = "Enable this thing if sending with Gmail";
			this.toolTip.SetToolTip(this.gmailLink, resources.GetString("gmailLink.ToolTip"));
			this.gmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gmailLink_LinkClicked);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(7, 112);
			this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(91, 15);
			this.label14.TabIndex = 29;
			this.label14.Text = "Centre maps on";
			this.toolTip.SetToolTip(this.label14, "Centre maps on aircraft or provided lat/lng");
			// 
			// filterDstCheckBox
			// 
			this.filterDstCheckBox.AutoSize = true;
			this.filterDstCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.filterDstCheckBox.Location = new System.Drawing.Point(7, 235);
			this.filterDstCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.filterDstCheckBox.Name = "filterDstCheckBox";
			this.filterDstCheckBox.Size = new System.Drawing.Size(194, 19);
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
			this.filterAltCheckBox.Location = new System.Drawing.Point(7, 204);
			this.filterAltCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.filterAltCheckBox.Name = "filterAltCheckBox";
			this.filterAltCheckBox.Size = new System.Drawing.Size(183, 19);
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
			this.ignoreModeSCheckBox.Location = new System.Drawing.Point(7, 265);
			this.ignoreModeSCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.ignoreModeSCheckBox.Name = "ignoreModeSCheckBox";
			this.ignoreModeSCheckBox.Size = new System.Drawing.Size(190, 19);
			this.ignoreModeSCheckBox.TabIndex = 32;
			this.ignoreModeSCheckBox.Text = "Ignore aircraft without position";
			this.toolTip.SetToolTip(this.ignoreModeSCheckBox, "VRS ignores aircraft without positions when filtering by distance. Disabling this" +
        " requests aircraft without position separately.");
			this.ignoreModeSCheckBox.UseVisualStyleBackColor = true;
			// 
			// filterReceiverCheckBox
			// 
			this.filterReceiverCheckBox.AutoSize = true;
			this.filterReceiverCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.filterReceiverCheckBox.Checked = true;
			this.filterReceiverCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.filterReceiverCheckBox.Location = new System.Drawing.Point(7, 294);
			this.filterReceiverCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.filterReceiverCheckBox.Name = "filterReceiverCheckBox";
			this.filterReceiverCheckBox.Size = new System.Drawing.Size(112, 19);
			this.filterReceiverCheckBox.TabIndex = 34;
			this.filterReceiverCheckBox.Text = "Filter by receiver";
			this.toolTip.SetToolTip(this.filterReceiverCheckBox, "Filter to only check aircraft from a specific receiver, this can be useful for re" +
        "ducing bandwidth");
			this.filterReceiverCheckBox.UseVisualStyleBackColor = true;
			this.filterReceiverCheckBox.CheckedChanged += new System.EventHandler(this.filterReceiverCheckBox_CheckedChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(7, 324);
			this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(208, 15);
			this.label12.TabIndex = 37;
			this.label12.Text = "Trail Update Frequency (no. of checks)";
			this.toolTip.SetToolTip(this.label12, resources.GetString("label12.ToolTip"));
			// 
			// flashWindowCheckBox
			// 
			this.flashWindowCheckBox.AutoSize = true;
			this.flashWindowCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.flashWindowCheckBox.Location = new System.Drawing.Point(273, 20);
			this.flashWindowCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.flashWindowCheckBox.Name = "flashWindowCheckBox";
			this.flashWindowCheckBox.Size = new System.Drawing.Size(100, 19);
			this.flashWindowCheckBox.TabIndex = 28;
			this.flashWindowCheckBox.Text = "Flash Window";
			this.toolTip.SetToolTip(this.flashWindowCheckBox, "Flash the window on alert");
			this.flashWindowCheckBox.UseVisualStyleBackColor = true;
			// 
			// saveSettingsButton
			// 
			this.saveSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveSettingsButton.Location = new System.Drawing.Point(668, 401);
			this.saveSettingsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.saveSettingsButton.Name = "saveSettingsButton";
			this.saveSettingsButton.Size = new System.Drawing.Size(120, 27);
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
			this.emailGroupBox.Location = new System.Drawing.Point(14, 218);
			this.emailGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailGroupBox.Name = "emailGroupBox";
			this.emailGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailGroupBox.Size = new System.Drawing.Size(384, 207);
			this.emailGroupBox.TabIndex = 22;
			this.emailGroupBox.TabStop = false;
			this.emailGroupBox.Text = "Email";
			// 
			// radarGroupBox
			// 
			this.radarGroupBox.Controls.Add(this.label12);
			this.radarGroupBox.Controls.Add(this.trailsAgeNumericUpDown);
			this.radarGroupBox.Controls.Add(this.refreshReceiversButton);
			this.radarGroupBox.Controls.Add(this.filterReceiverCheckBox);
			this.radarGroupBox.Controls.Add(this.receiverComboBox);
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
			this.radarGroupBox.Location = new System.Drawing.Point(405, 14);
			this.radarGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.radarGroupBox.Name = "radarGroupBox";
			this.radarGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.radarGroupBox.Size = new System.Drawing.Size(384, 360);
			this.radarGroupBox.TabIndex = 23;
			this.radarGroupBox.TabStop = false;
			this.radarGroupBox.Text = "Radar";
			// 
			// trailsAgeNumericUpDown
			// 
			this.trailsAgeNumericUpDown.Location = new System.Drawing.Point(286, 322);
			this.trailsAgeNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.trailsAgeNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.trailsAgeNumericUpDown.Name = "trailsAgeNumericUpDown";
			this.trailsAgeNumericUpDown.Size = new System.Drawing.Size(88, 23);
			this.trailsAgeNumericUpDown.TabIndex = 36;
			this.trailsAgeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// refreshReceiversButton
			// 
			this.refreshReceiversButton.Location = new System.Drawing.Point(147, 290);
			this.refreshReceiversButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.refreshReceiversButton.Name = "refreshReceiversButton";
			this.refreshReceiversButton.Size = new System.Drawing.Size(65, 27);
			this.refreshReceiversButton.TabIndex = 35;
			this.refreshReceiversButton.Text = "Refresh";
			this.refreshReceiversButton.UseVisualStyleBackColor = true;
			this.refreshReceiversButton.Click += new System.EventHandler(this.refreshReceiversButton_Click);
			// 
			// receiverComboBox
			// 
			this.receiverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.receiverComboBox.FormattingEnabled = true;
			this.receiverComboBox.Location = new System.Drawing.Point(219, 291);
			this.receiverComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.receiverComboBox.Name = "receiverComboBox";
			this.receiverComboBox.Size = new System.Drawing.Size(153, 23);
			this.receiverComboBox.TabIndex = 33;
			// 
			// ignoreDistTextBox
			// 
			this.ignoreDistTextBox.DecimalPlaces = 2;
			this.ignoreDistTextBox.Location = new System.Drawing.Point(286, 234);
			this.ignoreDistTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.ignoreDistTextBox.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
			this.ignoreDistTextBox.Name = "ignoreDistTextBox";
			this.ignoreDistTextBox.Size = new System.Drawing.Size(88, 23);
			this.ignoreDistTextBox.TabIndex = 28;
			// 
			// longTextBox
			// 
			this.longTextBox.DecimalPlaces = 4;
			this.longTextBox.Location = new System.Drawing.Point(286, 174);
			this.longTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
			this.longTextBox.Size = new System.Drawing.Size(88, 23);
			this.longTextBox.TabIndex = 23;
			// 
			// ignoreAltTextBox
			// 
			this.ignoreAltTextBox.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.ignoreAltTextBox.Location = new System.Drawing.Point(286, 204);
			this.ignoreAltTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.ignoreAltTextBox.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.ignoreAltTextBox.Name = "ignoreAltTextBox";
			this.ignoreAltTextBox.Size = new System.Drawing.Size(88, 23);
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
			this.latTextBox.Location = new System.Drawing.Point(69, 174);
			this.latTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
			this.latTextBox.Size = new System.Drawing.Size(88, 23);
			this.latTextBox.TabIndex = 21;
			// 
			// timeoutTextBox
			// 
			this.timeoutTextBox.Location = new System.Drawing.Point(286, 55);
			this.timeoutTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
			this.timeoutTextBox.Size = new System.Drawing.Size(88, 23);
			this.timeoutTextBox.TabIndex = 24;
			this.timeoutTextBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// radarURLTextBox
			// 
			this.radarURLTextBox.Location = new System.Drawing.Point(166, 85);
			this.radarURLTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.radarURLTextBox.Name = "radarURLTextBox";
			this.radarURLTextBox.Size = new System.Drawing.Size(207, 23);
			this.radarURLTextBox.TabIndex = 15;
			// 
			// programGroupBox
			// 
			this.programGroupBox.Controls.Add(this.centreLatLngRadioButton);
			this.programGroupBox.Controls.Add(this.centreAircraftRadioButton);
			this.programGroupBox.Controls.Add(this.label14);
			this.programGroupBox.Controls.Add(this.startOnStartCheckBox);
			this.programGroupBox.Controls.Add(this.refreshLabel);
			this.programGroupBox.Controls.Add(this.refreshTextBox);
			this.programGroupBox.Controls.Add(this.label5);
			this.programGroupBox.Controls.Add(this.removalTimeoutTextBox);
			this.programGroupBox.Controls.Add(this.runOnStartupCheckBox);
			this.programGroupBox.Location = new System.Drawing.Point(14, 14);
			this.programGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.programGroupBox.Name = "programGroupBox";
			this.programGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.programGroupBox.Size = new System.Drawing.Size(384, 141);
			this.programGroupBox.TabIndex = 24;
			this.programGroupBox.TabStop = false;
			this.programGroupBox.Text = "General";
			// 
			// centreLatLngRadioButton
			// 
			this.centreLatLngRadioButton.AutoSize = true;
			this.centreLatLngRadioButton.Location = new System.Drawing.Point(259, 110);
			this.centreLatLngRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.centreLatLngRadioButton.Name = "centreLatLngRadioButton";
			this.centreLatLngRadioButton.Size = new System.Drawing.Size(116, 19);
			this.centreLatLngRadioButton.TabIndex = 31;
			this.centreLatLngRadioButton.Text = "Provided Lat/Lng";
			this.centreLatLngRadioButton.UseVisualStyleBackColor = true;
			// 
			// centreAircraftRadioButton
			// 
			this.centreAircraftRadioButton.AutoSize = true;
			this.centreAircraftRadioButton.Checked = true;
			this.centreAircraftRadioButton.Location = new System.Drawing.Point(187, 110);
			this.centreAircraftRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.centreAircraftRadioButton.Name = "centreAircraftRadioButton";
			this.centreAircraftRadioButton.Size = new System.Drawing.Size(64, 19);
			this.centreAircraftRadioButton.TabIndex = 30;
			this.centreAircraftRadioButton.TabStop = true;
			this.centreAircraftRadioButton.Text = "Aircraft";
			this.centreAircraftRadioButton.UseVisualStyleBackColor = true;
			// 
			// refreshTextBox
			// 
			this.refreshTextBox.Location = new System.Drawing.Point(286, 22);
			this.refreshTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.refreshTextBox.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.refreshTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.refreshTextBox.Name = "refreshTextBox";
			this.refreshTextBox.Size = new System.Drawing.Size(91, 23);
			this.refreshTextBox.TabIndex = 22;
			this.refreshTextBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.flashWindowCheckBox);
			this.groupBox1.Controls.Add(this.soundAlertsCheckBox);
			this.groupBox1.Controls.Add(this.notificationsCheckBox);
			this.groupBox1.Location = new System.Drawing.Point(14, 161);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.groupBox1.Size = new System.Drawing.Size(384, 51);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Alert Actions";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(803, 441);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.programGroupBox);
			this.Controls.Add(this.radarGroupBox);
			this.Controls.Add(this.emailGroupBox);
			this.Controls.Add(this.saveSettingsButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
			((System.ComponentModel.ISupportInitialize)(this.trailsAgeNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreDistTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.longTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreAltTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.latTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timeoutTextBox)).EndInit();
			this.programGroupBox.ResumeLayout(false);
			this.programGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.refreshTextBox)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
		private System.Windows.Forms.ComboBox receiverComboBox;
		private System.Windows.Forms.CheckBox filterReceiverCheckBox;
		private System.Windows.Forms.Button refreshReceiversButton;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.NumericUpDown trailsAgeNumericUpDown;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox flashWindowCheckBox;
	}
}