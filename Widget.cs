using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using System;
using System.Drawing;

namespace HotkeyWidget
{
    public partial class HotkeyWidgetObject : IWidgetObject {

        public IWidgetInstance CreateWidgetInstance(WidgetSize widget_size, Guid instance_guid) {
            HotkeyWidgetInstance widget_instance = new HotkeyWidgetInstance(this, instance_guid);
            return widget_instance;
        }

        public Bitmap GetWidgetPreview(WidgetSize widget_size) {
            Color BackColor = Color.FromArgb(35, 35, 35);
            Size size = widget_size.ToSize();
            Bitmap BitmapPreview = new Bitmap(size.Width, size.Height);
            using(Graphics g = Graphics.FromImage(BitmapPreview)) {
                g.Clear(BackColor);
                Font FontHeader = new Font("Lucida Console", 20, FontStyle.Bold);
                SizeF str_size = g.MeasureString("HOTKEY", FontHeader);
                g.DrawString("HOTKEY", FontHeader, Brushes.White, (size.Width - str_size.Width) / 2, (size.Height - str_size.Height) / 2);
            }
            return BitmapPreview;
        }

        public WidgetError Load(string resource_path) {

            //WidgetManager.RegisterWidget(this);
            return WidgetError.NO_ERROR;

        }

        public bool RemoveWidgetInstance(Guid instance_guid) {
            throw new NotImplementedException();
        }

        public WidgetError Unload() {
            return WidgetError.NO_ERROR;
        }
    }

}
