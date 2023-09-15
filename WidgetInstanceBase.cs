using PictureWidget;
using System;
using System.Drawing;
using System.Windows.Controls;
using WigiDashWidgetFramework;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance {
        private SettingsUserControl _userControl;

        public override UserControl GetSettingsControl()
        {
            if (_userControl == null)
            {
                _userControl = new SettingsUserControl(this);
            }
            return _userControl;
        }
    }
}
