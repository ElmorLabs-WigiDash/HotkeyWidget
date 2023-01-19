using PictureWidget;
using System;
using System.Drawing;
using System.Windows.Controls;
using WigiDashWidgetFramework;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance {
        public new UserControl GetSettingsControl() {
            return new SettingsUserControl(this);
        }
    }
}
