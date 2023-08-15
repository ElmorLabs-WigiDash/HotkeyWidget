using WigiDashWidgetFramework;
using WigiDashWidgetFramework.WidgetUtility;
using PictureWidget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance, IWidgetInstanceWithRemoval
    {
        public HotkeyWidgetInstance(HotkeyWidget parent, WidgetSize widgetSize, Guid instanceGuid) : base(parent, widgetSize, instanceGuid)
        {
        }

        public ObservableCollection<Guid> Actions = new ObservableCollection<Guid>();

        public override void ClickEvent(ClickType click_type, int x, int y) {
            if(click_type == ClickType.Single) {
                foreach (Guid guid in Actions)
                {
                    WidgetObject.WidgetManager.TriggerAction(guid);
                }
            }

            base.ClickEvent(click_type, x, y);
        }

        public override void SaveSettings() {
            WidgetObject.WidgetManager.StoreSetting(this, "HotkeyActions", string.Join(",", Actions.Select(x => x.ToString()).ToArray()), false);
            base.SaveSettings();
        }

        public override void LoadSettings()
        {
            if (WidgetObject.WidgetManager.LoadSetting(this, "HotkeyActions", out string actionGuidsString))
            {
                if (!string.IsNullOrEmpty(actionGuidsString))
                {
                    string[] guids = actionGuidsString.Split(',');
                    foreach (string guidString in guids)
                    {
                        bool guidParseResult = Guid.TryParse(guidString, out Guid actionGuid);

                        if (!guidParseResult) continue;

                        string actionString = WidgetObject.WidgetManager.GetActionString(WidgetObject.WidgetManager.GetParentDevice(this) ?? Guid.Empty, actionGuid);
                        if (string.IsNullOrEmpty(actionString)) continue;

                        Actions.Add(actionGuid);
                    }
                }
            }
            base.LoadSettings();
        }

        public void OnRemove()
        {
            foreach (Guid actionGuid in Actions)
            {
                WidgetObject.WidgetManager.RemoveAction(WidgetObject.WidgetManager.GetParentDevice(this) ?? Guid.Empty, actionGuid);
            }
        }
    }
}

