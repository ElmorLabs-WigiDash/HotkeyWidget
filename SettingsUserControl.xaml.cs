using HandyControl.Data;
using Microsoft.Win32;
using PictureWidget;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
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

            textBoxFile.Text = Path.GetFileName(parent.ImagePath);

            try {
                bgColorSelect.Content = ColorTranslator.ToHtml(parent.BackColor);
            } catch { }

            textOverlay.Text = parent.OverlayText;

            try 
            {
                overlayColorSelect.Content = ColorTranslator.ToHtml(parent.OverlayColor);
            } catch { }

            try
            {
                vectorColorSelect.Content = ColorTranslator.ToHtml(parent.VectorColor);
            }
            catch { }

            vectorScaleSelect.Value = parent.VectorScale * 100;

            overlayFontSelect.Content = new FontConverter().ConvertToInvariantString(parent.OverlayFont);
            overlayFontSelect.Tag = parent.OverlayFont;

            OverlayXPos.SelectedIndex = parent.OverlayXPos;
            OverlayYPos.SelectedIndex = parent.OverlayYPos;

            OverlayXOffset.Value = parent.OverlayXOffset;
            OverlayYOffset.Value = parent.OverlayYOffset;

            globalThemeCheck.IsChecked = parent.UseGlobal;
            wordWrapChk.IsChecked = parent.OverlayWrap;
            autoScaleChk.IsChecked = GraphicsExtension.AutoScale;

            overlayColorSelect.IsEnabled = !parent.UseGlobal;
            overlayFontSelect.IsEnabled = !parent.UseGlobal;
            bgColorSelect.IsEnabled = !parent.UseGlobal;

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

            try
            {
                parent.BackColor = ColorTranslator.FromHtml(bgColorSelect.Content.ToString());
                parent.OverlayColor = ColorTranslator.FromHtml(overlayColorSelect.Content.ToString());
                parent.VectorColor = ColorTranslator.FromHtml(vectorColorSelect.Content.ToString());
            }
            catch { }

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void buttonFile_Click(object sender, RoutedEventArgs e)
        {
            string result = parent.WidgetObject.WidgetManager.RequestImageSelection(string.Empty);
            if (result != null && result != string.Empty)
            {
                textBoxFile.Text = Path.GetFileName(result);
                parent.ImportImage(result);
            }
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e)
        {
            Guid actionGuid = Guid.NewGuid();

            bool addSuccess = parent.WidgetObject.WidgetManager.EditAction(_parentDevice, actionGuid, parent.Guid.ToString());

            if (!addSuccess) return;

            parent.WidgetObject.WidgetManager.BindAction(parent, actionGuid);
            parent.Actions.Add(actionGuid);

            UpdateActionList();

            parent.SaveSettings();
            parent.UpdateSettings();
        }
        
        private void UpdateActionList()
        {
            actionList.Children.Clear();

            if (parent.Actions.Count <= 0)
            {
                actionList.Visibility = Visibility.Collapsed;
                NoActionLabel.Visibility = Visibility.Visible;
                return;
            } else
            {
                actionList.Visibility = Visibility.Visible;
                NoActionLabel.Visibility = Visibility.Collapsed;
            }

            foreach (Guid actionGuid in parent.Actions)
            {
                string actionString = parent.WidgetObject.WidgetManager.GetActionString(_parentDevice, actionGuid);

                Action deleteAction = new Action(() =>
                {
                    parent.WidgetObject.WidgetManager.RemoveAction(_parentDevice, actionGuid);
                    parent.WidgetObject.WidgetManager.UnbindAction(parent, actionGuid);
                    parent.Actions.Remove(actionGuid);

                    UpdateActionList();

                    parent.SaveSettings();
                    parent.UpdateSettings();
                });

                Action editAction = new Action(() =>
                {
                    parent.WidgetObject.WidgetManager.EditAction(_parentDevice, actionGuid);
                    UpdateActionList();

                    parent.SaveSettings();
                    parent.UpdateSettings();
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

                    parent.SaveSettings();
                    parent.UpdateSettings();
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

            parent.OverlayFont = overlayFontSelect.Tag as Font;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void globalThemeCheck_Click(object sender, RoutedEventArgs e)
        {
            parent.UseGlobal = globalThemeCheck.IsChecked ?? false;
            overlayColorSelect.IsEnabled = !parent.UseGlobal;
            overlayFontSelect.IsEnabled = !parent.UseGlobal;
            bgColorSelect.IsEnabled = !parent.UseGlobal;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void OverlayOffset_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            parent.OverlayXOffset = (int)OverlayXOffset.Value;
            parent.OverlayYOffset = (int)OverlayYOffset.Value;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void OverlayPos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OverlayXPos.SelectedIndex == -1 || OverlayYPos.SelectedIndex == -1)
                return;

            parent.OverlayXPos = OverlayXPos.SelectedIndex;
            parent.OverlayYPos = OverlayYPos.SelectedIndex;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void textOverlay_TextChanged(object sender, TextChangedEventArgs e)
        {
            parent.OverlayText = textOverlay.Text;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void clearFile_Click(object sender, RoutedEventArgs e)
        {
            parent.WidgetObject.WidgetManager.RemoveFile(parent, "Image");
            parent.ImagePath = string.Empty;
            textBoxFile.Text = string.Empty;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void autoScaleChk_Click(object sender, RoutedEventArgs e)
        {
            GraphicsExtension.AutoScale = autoScaleChk.IsChecked == true;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void wordWrapChk_Click(object sender, RoutedEventArgs e)
        {
            parent.OverlayWrap = wordWrapChk.IsChecked == true;

            parent.SaveSettings();
            parent.UpdateSettings();
        }

        private void VectorScaleSelect_OnValueChanged(object sender, FunctionEventArgs<double> e)
        {
            if (parent == null) return;
            parent.VectorScale = vectorScaleSelect.Value / 100;

            parent.SaveSettings();
            parent.UpdateSettings();
        }
    }
}
