using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotkeyWidget {
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl {


        HotkeyWidgetInstance parent;
        private Guid _actionGuid;
        private Guid _parentDevice;

        public SettingsUserControl(HotkeyWidgetInstance widget_instance) {

            //_contentLoaded = true;
            //Utility.Utility.LoadXaml(this);

            InitializeComponent();

            parent = widget_instance;
            _parentDevice = parent.WidgetObject.WidgetManager.GetParentDevice(parent) ?? Guid.Empty;

            comboBoxType.Items.Add("Single");
            comboBoxType.Items.Add("Folder");

            comboBoxType.SelectedIndex = (int)parent.WidgetType;
            textBoxFile.Text = parent.image_path;
            try {
                textBoxColor.Text = ColorTranslator.ToHtml(parent.BackColor);
            } catch { }

            actionType.Content = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, _actionGuid);
        }

        private void buttonFile_Click(object sender, RoutedEventArgs e) {
            switch(comboBoxType.SelectedIndex) {
                case (int)HotkeyWidgetInstance.PictureWidgetType.Single:
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.bmp;*.ico";
                    bool? result = ofd.ShowDialog();
                    if(result != null && result != false) {
                        textBoxFile.Text = ofd.FileName;
                    }
                    break;
                case (int)HotkeyWidgetInstance.PictureWidgetType.Folder:
                    System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                    if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        textBoxFile.Text = fbd.SelectedPath;
                    }
                    break;
            }
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e) {

            try {
                parent.BackColor = ColorTranslator.FromHtml(textBoxColor.Text);
            } catch { }

            switch(comboBoxType.SelectedIndex) {
                case (int)HotkeyWidgetInstance.PictureWidgetType.Single:
                    if(File.Exists(textBoxFile.Text)) {
                        parent.LoadImage(textBoxFile.Text);
                    }
                    break;
                case (int)HotkeyWidgetInstance.PictureWidgetType.Folder:
                    if(Directory.Exists(textBoxFile.Text)) {
                        parent.LoadFolder(textBoxFile.Text);
                    }
                    break;
            }

            parent.ActionGuid = _actionGuid;

            parent.SaveSettings();
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e)
        {
            parent.WidgetObject.WidgetManager.RemoveAction(_parentDevice, parent.ActionGuid);
            bool addSuccess = parent.WidgetObject.WidgetManager.CreateAction(_parentDevice, parent.ActionGuid, parent.Guid.ToString(), out Guid actionGuid);

            if (!addSuccess || actionGuid == Guid.Empty) return;

            actionType.Content = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, actionGuid);
            _actionGuid = actionGuid;
        }
    }
}
