using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotkeyWidget {
    public partial class HotkeySettingsForm : Form {

        HotkeyWidgetInstance parent_instance;
        List<Keys> KeyList = new List<Keys>();
        List<Guid> ScreenList = new List<Guid>();

        public HotkeySettingsForm(HotkeyWidgetInstance parent_instance) {
            InitializeComponent();

            this.parent_instance = parent_instance;
            
            comboBoxHotkey.Enabled = false;
            textBoxHotkeyMod.Enabled = false;
            buttonHotkeyRecord.Enabled = false;

            comboBoxAction.Items.Add("Execute");
            comboBoxAction.Items.Add("Brightness Up");
            comboBoxAction.Items.Add("Brightness Down");
            comboBoxAction.Items.Add("Volume Up");
            comboBoxAction.Items.Add("Volume Down");
            comboBoxAction.Items.Add("Volume Mute");
            comboBoxAction.Items.Add("Screenshot");
            comboBoxAction.Items.Add("Hotkey");
            comboBoxAction.Items.Add("Change Screen");
            comboBoxAction.SelectedIndexChanged += ComboBoxAction_SelectedIndexChanged;

            switch(parent_instance.ActionType) {
                case HotkeyWidgetInstance.Action.Execute: comboBoxAction.SelectedIndex = 0; break;
                case HotkeyWidgetInstance.Action.BrightnessUp: comboBoxAction.SelectedIndex = 1; break;
                case HotkeyWidgetInstance.Action.BrightnessDown: comboBoxAction.SelectedIndex = 2; break;
                case HotkeyWidgetInstance.Action.MediaFunction:
                    switch(parent_instance.MediaFunctionAction) {
                        case HotkeyWidgetInstance.MediaAction.APPCOMMAND_VOLUME_UP: comboBoxAction.SelectedIndex = 3; break;
                        case HotkeyWidgetInstance.MediaAction.APPCOMMAND_VOLUME_DOWN: comboBoxAction.SelectedIndex = 4; break;
                        case HotkeyWidgetInstance.MediaAction.APPCOMMAND_VOLUME_MUTE: comboBoxAction.SelectedIndex = 5; break;
                    }
                    break;
                case HotkeyWidgetInstance.Action.Screenshot: comboBoxAction.SelectedIndex = 6; break;
                case HotkeyWidgetInstance.Action.Hotkey:
                    comboBoxAction.SelectedIndex = 7;
                    comboBoxHotkey.Enabled = true;
                    textBoxHotkeyMod.Enabled = true;
                    buttonHotkeyRecord.Enabled = true;
                    break;
                case HotkeyWidgetInstance.Action.ChangeScreen:
                    comboBoxAction.SelectedIndex = 8;
                    comboBoxScreen.Enabled = true;
                    break;

            }

            if(parent_instance.Param.Length > 0) {
                if(comboBoxAction.SelectedIndex == 7) {
                    textBoxParam1.Text = parent_instance.HotkeyTarget;
                } else {
                    textBoxParam1.Text = parent_instance.Param[0];
                }
            }

            if(parent_instance.Param.Length > 1) {
                textBoxParam2.Text = parent_instance.Param[1];
            }

            textBoxIcon.Text = parent_instance.icon_path;
            textBoxBackground.Text = parent_instance.background_path;
            textBoxBackgroundColor.Text = ColorTranslator.ToHtml(parent_instance.BackColor);
            textBoxHotkeyMod.Text = "";
            foreach(Keys key in parent_instance.HotkeyModifiers) {
                switch(key) {
                    case Keys.ControlKey: textBoxHotkeyMod.Text += "CTRL "; break;
                    case Keys.Menu: textBoxHotkeyMod.Text += "ALT "; break;
                    case Keys.ShiftKey: textBoxHotkeyMod.Text += "SHFT "; break;
                }
            }
            var keys = Enum.GetValues(typeof(Keys));
            foreach(Keys key in keys) {
                KeyList.Add(key);
                comboBoxHotkey.Items.Add(Enum.GetName(typeof(Keys), key));
            }

            /*ScreenList = parent_instance.WidgetObject.WidgetManager.GetScreenList();
            foreach(Guid screen_guid in ScreenList) {
                comboBoxScreen.Items.Add(parent_instance.WidgetObject.WidgetManager.GetScreenName(screen_guid));
            }*/

            comboBoxHotkey.SelectedIndex = KeyList.FindIndex(s => s == parent_instance.HotkeyKey);
        }

        private void ComboBoxAction_SelectedIndexChanged(object sender, EventArgs e) {
            textBoxParam1.Enabled = (comboBoxAction.SelectedIndex == 0 || comboBoxAction.SelectedIndex == 6 || comboBoxAction.SelectedIndex == 7 || comboBoxAction.SelectedIndex == 8);
            textBoxParam2.Enabled = comboBoxAction.SelectedIndex == 0;
            buttonOpenFile.Enabled = (comboBoxAction.SelectedIndex == 0 || comboBoxAction.SelectedIndex == 6);
            if(comboBoxAction.SelectedIndex == 0) {
                labelCmd.Text = "Command";
            } else if(comboBoxAction.SelectedIndex == 6) {
                labelCmd.Text = "Folder";
            }
            if(comboBoxAction.SelectedIndex == 7) {
                comboBoxHotkey.Enabled = true;
                textBoxHotkeyMod.Enabled = true;
                buttonHotkeyRecord.Enabled = true;
            } else if(comboBoxAction.SelectedIndex == 8) {
                comboBoxScreen.Enabled = true;
                comboBoxHotkey.Enabled = false;
                textBoxHotkeyMod.Enabled = false;
                buttonHotkeyRecord.Enabled = false;
            } else {
                comboBoxHotkey.Enabled = false;
                textBoxHotkeyMod.Enabled = false;
                buttonHotkeyRecord.Enabled = false;
            }
        }

        private void buttonChange_Click(object sender, EventArgs e) {

            // Action
            string[] param;
            switch(comboBoxAction.SelectedIndex) {
                case 0:
                    param = new string[2];
                    param[0] = textBoxParam1.Text;
                    param[1] = textBoxParam2.Text;
                    parent_instance.SetAction(HotkeyWidgetInstance.Action.Execute, param); break;
                case 1: parent_instance.SetAction(HotkeyWidgetInstance.Action.BrightnessUp, new string[0]); break;
                case 2: parent_instance.SetAction(HotkeyWidgetInstance.Action.BrightnessDown, new string[0]); break;
                case 3: parent_instance.SetAction(HotkeyWidgetInstance.Action.MediaFunction, HotkeyWidgetInstance.MediaAction.APPCOMMAND_VOLUME_UP); break;
                case 4: parent_instance.SetAction(HotkeyWidgetInstance.Action.MediaFunction, HotkeyWidgetInstance.MediaAction.APPCOMMAND_VOLUME_DOWN); break;
                case 5: parent_instance.SetAction(HotkeyWidgetInstance.Action.MediaFunction, HotkeyWidgetInstance.MediaAction.APPCOMMAND_VOLUME_MUTE); break;
                case 6:
                    param = new string[1];
                    param[0] = textBoxParam1.Text;
                    parent_instance.SetAction(HotkeyWidgetInstance.Action.Screenshot, param); 
                    break;
                case 7:
                    param = new string[3];
                    param[0] = textBoxHotkeyMod.Text;
                    param[1] = comboBoxHotkey.Text;
                    param[2] = textBoxParam1.Text;
                    parent_instance.SetAction(HotkeyWidgetInstance.Action.Hotkey, param); 
                    break;
                case 8:
                    if(comboBoxScreen.SelectedIndex < 0) {
                        MessageBox.Show("No screen selected");
                        return;
                    } 
                    param = new string[2];
                    param[0] = ScreenList[comboBoxScreen.SelectedIndex].ToString();
                    //param[1] = parent_instance.WidgetObject.WidgetManager.GetScreenName(ScreenList[comboBoxScreen.SelectedIndex]);
                    parent_instance.SetAction(HotkeyWidgetInstance.Action.ChangeScreen, param);
                    break;

            }

            //parent_instance.SetBackground(textBoxBackground.Text);
            parent_instance.BackColor = ColorTranslator.FromHtml(textBoxBackgroundColor.Text);
            parent_instance.SetIcon(textBoxIcon.Text);

            parent_instance.SaveSettings();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e) {
            if(comboBoxAction.SelectedIndex == 0) {
                OpenFileDialog ofd = new OpenFileDialog();
                DialogResult result = ofd.ShowDialog();
                if(result == DialogResult.OK) {
                    textBoxParam1.Text = ofd.FileName;
                }
            } else if(comboBoxAction.SelectedIndex == 6) {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                if(result == DialogResult.OK) {
                    textBoxParam1.Text = fbd.SelectedPath;
                }
            }
        }

        private void buttonIcon_Click(object sender, EventArgs e) {

            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog();
            if(result == DialogResult.OK) {
                textBoxIcon.Text = ofd.FileName;
            }

        }

        private void buttonBackground_Click(object sender, EventArgs e) {

            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog();
            if(result == DialogResult.OK) {
                textBoxBackground.Text = ofd.FileName;
            }
        }

        bool recording_hotkey = false;
        //Keys recorded_modifer;
        //Keys recorded_keys;

        private void buttonHotkeyRecord_Click(object sender, EventArgs e) {
            
            if(!recording_hotkey) {
                recording_hotkey = true;
                this.KeyDown += HotkeySettingsForm_KeyDown;
                buttonHotkeyRecord.Text = "Save";
                buttonHotkeyRecord.ForeColor = Color.Red;
                buttonChange.Enabled = false;
                comboBoxAction.Enabled = false;
                labelHotkey.Select();
            } else {
                recording_hotkey = false;
                this.KeyDown -= HotkeySettingsForm_KeyDown;
                buttonHotkeyRecord.Text = "Record";
                buttonHotkeyRecord.ForeColor = Color.Black;
                buttonChange.Enabled = true;
                comboBoxAction.Enabled = true;
            }
        }

        private void HotkeySettingsForm_KeyDown(object sender, KeyEventArgs e) {
            textBoxHotkeyMod.Text = "";
            if((e.Modifiers & Keys.Control) != 0 && e.KeyCode != Keys.ControlKey) {
                textBoxHotkeyMod.Text += "CTRL ";
            }
            if((e.Modifiers & Keys.Alt) != 0 && e.KeyCode != Keys.Menu) {
                textBoxHotkeyMod.Text += "ALT ";
            }
            if((e.Modifiers & Keys.Shift) != 0 && e.KeyCode != Keys.ShiftKey) {
                textBoxHotkeyMod.Text += "SHFT ";
            }
            //comboBoxHotkey.Text = e.KeyCode.ToString();
            comboBoxHotkey.SelectedIndex = KeyList.FindIndex(s => s == e.KeyCode);
        }
    }
}
