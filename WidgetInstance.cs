using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using PictureWidget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static PictureWidget.PictureWidgetInstance;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance {
        public HotkeyWidgetInstance(HotkeyWidget parent, WidgetSize widgetSize, Guid instanceGuid) : base(parent, widgetSize, instanceGuid)
        {
            Initialize(parent, widgetSize, instanceGuid);
        }

        public Guid ActionGuid = Guid.NewGuid();
        public new void ClickEvent(ClickType click_type, int x, int y) {
            if(click_type == ClickType.Single) {
                base.WidgetObject.WidgetManager.TriggerAction((Guid)ActionGuid);
            }

            base.ClickEvent(click_type, x, y);
        }

        public new void SaveSettings() {
            base.WidgetObject.WidgetManager.StoreSetting(this, "HotkeyAction", ActionGuid.ToString());

            base.SaveSettings();
        }

        public new void LoadSettings() {
            if (base.WidgetObject.WidgetManager.LoadSetting(this, "HotkeyAction", out string actionGuidString))
            {
                Guid.TryParse(actionGuidString, out ActionGuid);
            }

            base.LoadSettings();
        }
    }
}

