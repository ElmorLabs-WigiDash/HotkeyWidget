using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using PictureWidget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static PictureWidget.PictureWidgetInstance;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance {
        public HotkeyWidgetInstance(HotkeyWidget parent, WidgetSize widgetSize, Guid instanceGuid) : base(parent, widgetSize, instanceGuid)
        {
            LoadSettings();
            Initialize(parent, widgetSize, instanceGuid);
        }

        public List<Guid> Actions = new List<Guid>();

        public new void ClickEvent(ClickType click_type, int x, int y) {
            if(click_type == ClickType.Single) {
                foreach (Guid guid in Actions)
                {
                    base.WidgetObject.WidgetManager.TriggerAction(guid);
                }
            }

            base.ClickEvent(click_type, x, y);
        }

        public new void SaveSettings() {
            base.WidgetObject.WidgetManager.StoreSetting(this, "HotkeyActions", string.Join(",", Actions.Select(x => x.ToString()).ToArray()));

            base.SaveSettings();
        }

        public new void LoadSettings() {
            if (base.WidgetObject.WidgetManager.LoadSetting(this, "HotkeyActions", out string actionGuidsString))
            {
                string[] guids = actionGuidsString.Split(',');
                foreach (string guidString in guids)
                {
                    Guid.TryParse(guidString, out Guid actionGuid);
                    Actions.Add(actionGuid);
                }
            }

            base.LoadSettings();
        }
    }
}

