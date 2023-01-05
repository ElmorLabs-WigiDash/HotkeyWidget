using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using PictureWidget;
using System;
using System.Drawing;
using System.Windows.Controls;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance, IWidgetInstance {
        public new UserControl GetSettingsControl() {
            return new SettingsUserControl(this);
        }
    }
}
