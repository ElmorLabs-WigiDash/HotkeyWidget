using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using System;
using System.Windows.Controls;

namespace HotkeyWidget
{
    public partial class HotkeyWidgetInstance : IWidgetInstance {

        // Identity
        private HotkeyWidgetObject parent;
        public IWidgetObject WidgetObject {
            get {
                return parent;
            }
        }
        public Guid Guid { get; set; }

        public WidgetSize WidgetSize { get; set; }

        public event WidgetUpdatedEventHandler WidgetUpdated;

        public UserControl GetSettingsControl() {
            return null;
        }
    }
}
