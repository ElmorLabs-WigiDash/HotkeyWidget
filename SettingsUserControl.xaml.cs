using Microsoft.Win32;
using PictureWidget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Utility;
using Color = System.Drawing.Color;

namespace HotkeyWidget {
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl {


        HotkeyWidgetInstance parent;
        private Guid _parentDevice;

        public SettingsUserControl(HotkeyWidgetInstance widget_instance) {

            //_contentLoaded = true;
            //Utility.Utility.LoadXaml(this);

            InitializeComponent();

            parent = widget_instance;
            _parentDevice = parent.WidgetObject.WidgetManager.GetParentDevice(parent) ?? Guid.Empty;

            textBoxFile.Text = parent.ImagePath;
            try {
                bgColorSelect.Content = ColorTranslator.ToHtml(parent.BackColor);
            } catch { }

            textOverlay.Text = parent.OverlayText;

            try 
            {
                overlayColorSelect.Content = ColorTranslator.ToHtml(parent.OverlayColor);
            } catch { }

            overlayFontSelect.Content = new FontConverter().ConvertToInvariantString(parent.OverlayFont);
            overlayFontSelect.Tag = parent.OverlayFont;

            OverlayXOffset.Value = parent.OverlayXOffset;
            OverlayYOffset.Value = parent.OverlayYOffset;

            UpdateActionList();

            //actionType.Content = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, _actionGuid);
        }

        private void colorSelect_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button caller)
            {
                Color defaultColor = ColorTranslator.FromHtml(caller.Content.ToString());
                Color selectedColor = parent.WidgetObject.WidgetManager.RequestColorSelection(defaultColor);
                caller.Content = ColorTranslator.ToHtml(selectedColor);
            }
        }

        private void buttonFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.bmp;*.ico";
            bool? result = ofd.ShowDialog();
            if (result != null && result != false)
            {
                textBoxFile.Text = ofd.FileName;
            }
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e) {
            try
            {
                parent.BackColor = ColorTranslator.FromHtml(bgColorSelect.Content.ToString());
                parent.OverlayColor = ColorTranslator.FromHtml(overlayColorSelect.Content.ToString());
            }
            catch { }

            parent.LoadImage(textBoxFile.Text);

            parent.OverlayText = textOverlay.Text;
            parent.OverlayFont = overlayFontSelect.Tag as Font;
            parent.UseGlobal = globalThemeCheck.IsChecked ?? false;

            parent.OverlayXOffset = (int)OverlayXOffset.Value;
            parent.OverlayYOffset = (int)OverlayYOffset.Value;

            //parent.RequestUpdate();
            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e)
        {
            //parent.WidgetObject.WidgetManager.RemoveAction(_parentDevice, parent.ActionGuid);
            //bool addSuccess = parent.WidgetObject.WidgetManager.CreateAction(_parentDevice, parent.ActionGuid, parent.Guid.ToString(), out Guid actionGuid);
            Guid actionGuid = Guid.NewGuid();

            bool addSuccess = parent.WidgetObject.WidgetManager.EditAction(_parentDevice, actionGuid, parent.Guid.ToString());

            //if (!addSuccess || actionGuid == Guid.Empty) return;
            if (!addSuccess) return;
            parent.Actions.Add(actionGuid);

            UpdateActionList();

            //actionType.Content = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, actionGuid);
            //actionType.Content = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, actionGuid);
            //_actionGuid = actionGuid;
        }
        
        private void UpdateActionList()
        {
            actionList.Children.Clear();

            foreach (Guid actionGuid in parent.Actions)
            {
                string actionString = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, actionGuid);

                Action deleteAction = new Action(() =>
                {
                    parent.WidgetObject.WidgetManager.RemoveAction(_parentDevice, actionGuid);
                    parent.Actions.Remove(actionGuid);
                    UpdateActionList();
                });

                Action editAction = new Action(() =>
                {
                    parent.WidgetObject.WidgetManager.EditAction(_parentDevice, actionGuid);
                    UpdateActionList();
                });

                Action<bool> moveAction = new Action<bool>((bool moveUp) => {
                    int index = parent.Actions.IndexOf(actionGuid);
                    parent.Actions.Remove(actionGuid);

                    if (moveUp)
                    {
                        int newIndex = (index - 1).Clamp(0, parent.Actions.Count - 1);
                        parent.Actions.Insert(newIndex, actionGuid);
                    } else
                    {
                        if (index + 1 >= parent.Actions.Count)
                        {
                            parent.Actions.Add(actionGuid);
                        }
                        else
                        {
                            int newIndex = (index + 1).Clamp(0, parent.Actions.Count - 1);
                            parent.Actions.Insert(newIndex, actionGuid);
                        }
                    }

                    UpdateActionList();
                });

                ActionRow actionRow = new ActionRow(actionString, deleteAction, editAction, moveAction);

                actionList.Children.Add(actionRow);
            }
        }

        private void overlayFontSelect_Click(object sender, RoutedEventArgs e)
        {
            Font defaultFont = parent.OverlayFont;
            Font selectedFont = parent.WidgetObject.WidgetManager.RequestFontSelection(defaultFont);

            if (sender is Button caller)
            {
                caller.Content = new FontConverter().ConvertToInvariantString(selectedFont);
                caller.Tag = selectedFont;
            }
        }
    }
}
