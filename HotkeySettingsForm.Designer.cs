namespace HotkeyWidget {
    partial class HotkeySettingsForm {
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
            this.comboBoxAction = new System.Windows.Forms.ComboBox();
            this.textBoxParam1 = new System.Windows.Forms.TextBox();
            this.textBoxParam2 = new System.Windows.Forms.TextBox();
            this.buttonChange = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCmd = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIcon = new System.Windows.Forms.TextBox();
            this.labelIcon = new System.Windows.Forms.Label();
            this.labelBackground = new System.Windows.Forms.Label();
            this.textBoxBackground = new System.Windows.Forms.TextBox();
            this.textBoxBackgroundColor = new System.Windows.Forms.TextBox();
            this.buttonIcon = new System.Windows.Forms.Button();
            this.buttonBackground = new System.Windows.Forms.Button();
            this.labelBgColor = new System.Windows.Forms.Label();
            this.labelHotkey = new System.Windows.Forms.Label();
            this.textBoxHotkeyMod = new System.Windows.Forms.TextBox();
            this.buttonHotkeyRecord = new System.Windows.Forms.Button();
            this.comboBoxHotkey = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxScreen = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxAction
            // 
            this.comboBoxAction.FormattingEnabled = true;
            this.comboBoxAction.Location = new System.Drawing.Point(71, 12);
            this.comboBoxAction.Name = "comboBoxAction";
            this.comboBoxAction.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAction.TabIndex = 0;
            // 
            // textBoxParam1
            // 
            this.textBoxParam1.Location = new System.Drawing.Point(71, 39);
            this.textBoxParam1.Name = "textBoxParam1";
            this.textBoxParam1.Size = new System.Drawing.Size(121, 20);
            this.textBoxParam1.TabIndex = 1;
            // 
            // textBoxParam2
            // 
            this.textBoxParam2.Location = new System.Drawing.Point(71, 65);
            this.textBoxParam2.Name = "textBoxParam2";
            this.textBoxParam2.Size = new System.Drawing.Size(121, 20);
            this.textBoxParam2.TabIndex = 2;
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(198, 264);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(75, 23);
            this.buttonChange.TabIndex = 3;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(198, 38);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFile.TabIndex = 4;
            this.buttonOpenFile.Text = "File...";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Action";
            // 
            // labelCmd
            // 
            this.labelCmd.AutoSize = true;
            this.labelCmd.Location = new System.Drawing.Point(11, 42);
            this.labelCmd.Name = "labelCmd";
            this.labelCmd.Size = new System.Drawing.Size(54, 13);
            this.labelCmd.TabIndex = 6;
            this.labelCmd.Text = "Command";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Param";
            // 
            // textBoxIcon
            // 
            this.textBoxIcon.Location = new System.Drawing.Point(71, 91);
            this.textBoxIcon.Name = "textBoxIcon";
            this.textBoxIcon.Size = new System.Drawing.Size(121, 20);
            this.textBoxIcon.TabIndex = 8;
            // 
            // labelIcon
            // 
            this.labelIcon.AutoSize = true;
            this.labelIcon.Location = new System.Drawing.Point(11, 95);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(28, 13);
            this.labelIcon.TabIndex = 9;
            this.labelIcon.Text = "Icon";
            // 
            // labelBackground
            // 
            this.labelBackground.AutoSize = true;
            this.labelBackground.Location = new System.Drawing.Point(12, 120);
            this.labelBackground.Name = "labelBackground";
            this.labelBackground.Size = new System.Drawing.Size(41, 13);
            this.labelBackground.TabIndex = 10;
            this.labelBackground.Text = "Backg.";
            // 
            // textBoxBackground
            // 
            this.textBoxBackground.Location = new System.Drawing.Point(71, 117);
            this.textBoxBackground.Name = "textBoxBackground";
            this.textBoxBackground.Size = new System.Drawing.Size(121, 20);
            this.textBoxBackground.TabIndex = 11;
            // 
            // textBoxBackgroundColor
            // 
            this.textBoxBackgroundColor.Location = new System.Drawing.Point(71, 143);
            this.textBoxBackgroundColor.Name = "textBoxBackgroundColor";
            this.textBoxBackgroundColor.Size = new System.Drawing.Size(121, 20);
            this.textBoxBackgroundColor.TabIndex = 12;
            // 
            // buttonIcon
            // 
            this.buttonIcon.Location = new System.Drawing.Point(198, 90);
            this.buttonIcon.Name = "buttonIcon";
            this.buttonIcon.Size = new System.Drawing.Size(75, 23);
            this.buttonIcon.TabIndex = 13;
            this.buttonIcon.Text = "File...";
            this.buttonIcon.UseVisualStyleBackColor = true;
            this.buttonIcon.Click += new System.EventHandler(this.buttonIcon_Click);
            // 
            // buttonBackground
            // 
            this.buttonBackground.Location = new System.Drawing.Point(198, 115);
            this.buttonBackground.Name = "buttonBackground";
            this.buttonBackground.Size = new System.Drawing.Size(75, 23);
            this.buttonBackground.TabIndex = 14;
            this.buttonBackground.Text = "File...";
            this.buttonBackground.UseVisualStyleBackColor = true;
            this.buttonBackground.Click += new System.EventHandler(this.buttonBackground_Click);
            // 
            // labelBgColor
            // 
            this.labelBgColor.AutoSize = true;
            this.labelBgColor.Location = new System.Drawing.Point(11, 146);
            this.labelBgColor.Name = "labelBgColor";
            this.labelBgColor.Size = new System.Drawing.Size(41, 13);
            this.labelBgColor.TabIndex = 15;
            this.labelBgColor.Text = "Bg. Clr.";
            // 
            // labelHotkey
            // 
            this.labelHotkey.AutoSize = true;
            this.labelHotkey.Location = new System.Drawing.Point(11, 171);
            this.labelHotkey.Name = "labelHotkey";
            this.labelHotkey.Size = new System.Drawing.Size(41, 13);
            this.labelHotkey.TabIndex = 16;
            this.labelHotkey.Text = "Hotkey";
            // 
            // textBoxHotkeyMod
            // 
            this.textBoxHotkeyMod.Location = new System.Drawing.Point(71, 168);
            this.textBoxHotkeyMod.Name = "textBoxHotkeyMod";
            this.textBoxHotkeyMod.Size = new System.Drawing.Size(121, 20);
            this.textBoxHotkeyMod.TabIndex = 17;
            // 
            // buttonHotkeyRecord
            // 
            this.buttonHotkeyRecord.Location = new System.Drawing.Point(198, 168);
            this.buttonHotkeyRecord.Name = "buttonHotkeyRecord";
            this.buttonHotkeyRecord.Size = new System.Drawing.Size(75, 47);
            this.buttonHotkeyRecord.TabIndex = 18;
            this.buttonHotkeyRecord.Text = "Record";
            this.buttonHotkeyRecord.UseVisualStyleBackColor = true;
            this.buttonHotkeyRecord.Click += new System.EventHandler(this.buttonHotkeyRecord_Click);
            // 
            // comboBoxHotkey
            // 
            this.comboBoxHotkey.FormattingEnabled = true;
            this.comboBoxHotkey.Location = new System.Drawing.Point(71, 195);
            this.comboBoxHotkey.Name = "comboBoxHotkey";
            this.comboBoxHotkey.Size = new System.Drawing.Size(121, 21);
            this.comboBoxHotkey.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Screen";
            // 
            // comboBoxScreen
            // 
            this.comboBoxScreen.Enabled = false;
            this.comboBoxScreen.FormattingEnabled = true;
            this.comboBoxScreen.Location = new System.Drawing.Point(71, 222);
            this.comboBoxScreen.Name = "comboBoxScreen";
            this.comboBoxScreen.Size = new System.Drawing.Size(121, 21);
            this.comboBoxScreen.TabIndex = 21;
            // 
            // HotkeySettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 299);
            this.Controls.Add(this.comboBoxScreen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxHotkey);
            this.Controls.Add(this.buttonHotkeyRecord);
            this.Controls.Add(this.textBoxHotkeyMod);
            this.Controls.Add(this.labelHotkey);
            this.Controls.Add(this.labelBgColor);
            this.Controls.Add(this.buttonBackground);
            this.Controls.Add(this.buttonIcon);
            this.Controls.Add(this.textBoxBackgroundColor);
            this.Controls.Add(this.textBoxBackground);
            this.Controls.Add(this.labelBackground);
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.textBoxIcon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCmd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.textBoxParam2);
            this.Controls.Add(this.textBoxParam1);
            this.Controls.Add(this.comboBoxAction);
            this.KeyPreview = true;
            this.Name = "HotkeySettingsForm";
            this.Text = "HotkeySettingsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxAction;
        private System.Windows.Forms.TextBox textBoxParam1;
        private System.Windows.Forms.TextBox textBoxParam2;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCmd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIcon;
        private System.Windows.Forms.Label labelIcon;
        private System.Windows.Forms.Label labelBackground;
        private System.Windows.Forms.TextBox textBoxBackground;
        private System.Windows.Forms.TextBox textBoxBackgroundColor;
        private System.Windows.Forms.Button buttonIcon;
        private System.Windows.Forms.Button buttonBackground;
        private System.Windows.Forms.Label labelBgColor;
        private System.Windows.Forms.Label labelHotkey;
        private System.Windows.Forms.TextBox textBoxHotkeyMod;
        private System.Windows.Forms.Button buttonHotkeyRecord;
        private System.Windows.Forms.ComboBox comboBoxHotkey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxScreen;
    }
}