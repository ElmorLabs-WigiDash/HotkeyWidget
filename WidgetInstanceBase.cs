using PictureWidget;
using System.Windows.Controls;

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
