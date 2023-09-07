﻿using WigiDashWidgetFramework;
using System;
using System.Drawing;
using WigiDashWidgetFramework.WidgetUtility;
using System.IO;
using PictureWidget;

namespace HotkeyWidget {
    public partial class HotkeyWidget : IWidgetObject {

        // Functionality
        public string ResourcePath;
        private Bitmap icon;

        public WidgetError Load(string resource_path) {
            
            this.ResourcePath = resource_path;

            // Load previews
            icon = new Bitmap(Path.Combine(ResourcePath, "icon.png"));

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
                g.DrawImageZoomedToFit(icon, size.Width, size.Height);
            }
            return BitmapPreview;
        }
    }

}
