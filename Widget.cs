using FrontierWidgetFramework;
using System;
using System.Drawing;
using FrontierWidgetFramework.WidgetUtility;

namespace HotkeyWidget {
    public partial class HotkeyWidget : IWidgetObject {

        // Functionality
        public string ResourcePath;
        public WidgetError Load(string resource_path) {
            
            this.ResourcePath = resource_path;

            // Load previews

            return WidgetError.NO_ERROR;
        }

        public WidgetError Unload() {
            return WidgetError.NO_ERROR;
        }

        public IWidgetInstance CreateWidgetInstance(WidgetSize widget_size, Guid instance_guid) {
            HotkeyWidgetInstance widget_instance = new HotkeyWidgetInstance(this, widget_size, instance_guid);
            return widget_instance;
        }

        public bool RemoveWidgetInstance(Guid instance_guid) {
            throw new NotImplementedException();
        }

        public Bitmap GetWidgetPreview(WidgetSize widget_size) {
            Color BackColor = Color.FromArgb(35, 35, 35);
            Size size = widget_size.ToSize();
            Bitmap BitmapPreview = new Bitmap(size.Width, size.Height);
            using(Graphics g = Graphics.FromImage(BitmapPreview)) {
                g.Clear(BackColor);
                Font FontHeader = new Font("Lucida Console", 20, FontStyle.Bold);
                SizeF str_size = g.MeasureString("Hotkey", FontHeader);
                g.DrawString("Hotkey", FontHeader, Brushes.White, (size.Width - str_size.Width) / 2, (size.Height - str_size.Height) / 2);
            }
            return BitmapPreview;
        }
    }

}
