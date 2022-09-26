using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HotkeyWidget
{
    public partial class HotkeyWidgetObject : IWidgetObject {

        public Guid Guid {
            get {
                //return new Guid("10FD82AA-5E43-4633-9B97-A8113C82DC27");
                return new Guid(GetType().Assembly.GetName().Name);
            }
        }

        public SdkVersion TargetSdk {
            get {
                return WidgetUtility.CurrentSdkVersion;
            }
        }

        public string Name {
            get {
                return "Hotkey";
            }
        }
        public string Description {

            get {
                return "A widget displaying the current time and date";
            }

        }
        public string Author {
            get {
                return "Jon Sandström";
            }
        }
        public string Website {
            get {
                return "https://www.elmorlabs.com/";
            }
        }
        public Version Version {
            get {
                return new Version(1, 0, 0);
            }
        }

        public string VersionString => throw new NotImplementedException();

        public List<WidgetSize> SupportedSizes {
            get {
                return new List<WidgetSize>() { new WidgetSize(1, 1) };
            }
        }

        public Bitmap PreviewImage {
            get {
                //return new Bitmap(ResourcePath + "preview_2x1.png");
                return GetWidgetPreview(new WidgetSize(1, 1));
            }
        }

        // Functionality
        public IWidgetManager WidgetManager { get; set; }

        // Error handling
        public string LastErrorMessage { get; set; }
    }

}
