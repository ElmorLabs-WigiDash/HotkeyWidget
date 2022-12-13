using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using System;
using System.Drawing;
using System.Windows.Controls;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : IWidgetInstance {

        private HotkeyWidget parent;
        public IWidgetObject WidgetObject { 
            get {
                return parent;
            }
        }

        public Guid Guid { get; set; }

        public WidgetSize WidgetSize { get; set; }

        public UserControl GetSettingsControl() {
            return new SettingsUserControl(this);
        }

        // Events
        public event WidgetUpdatedEventHandler WidgetUpdated;

    }
}
