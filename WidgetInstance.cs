using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance {

        // Functionality
        public void RequestUpdate() {

            if(drawing_mutex.WaitOne(mutex_timeout)) {
            //lock(BitmapCurrent) { 
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }

        public void ClickEvent(ClickType click_type, int x, int y) {
            if(click_type == ClickType.Single) {
                parent.WidgetManager.TriggerAction((Guid)ActionGuid);
            }
        }

        public void Dispose() {
            pause_task = false;
            run_task = false;
        }

        public void EnterSleep() {
            pause_task = true;
        }

        public void ExitSleep() {
            //lock(BitmapCurrent) {
                //UpdateWidget();
            //}
            if(drawing_mutex.WaitOne(mutex_timeout)) {
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
            pause_task = false;
        }

        // Class specific
        private Mutex drawing_mutex = new Mutex();
        private const int mutex_timeout = 100;

        private Thread task_thread;
        private volatile bool run_task = false;
        private volatile bool pause_task = false;
        
        private Bitmap BitmapCurrent;

        public Color BackColor = Color.FromArgb(35, 35, 35);

        private Point Offset = Point.Empty;

        public Guid ActionGuid = Guid.NewGuid();

        // https://social.microsoft.com/Forums/en-US/fcb7d14d-d15b-4336-971c-94a80e34b85e/editing-animated-gifs-in-c?forum=netfxbcl
        public class AnimatedGif {

            private List<AnimatedGifFrame> mImages = new List<AnimatedGifFrame>();
            PropertyItem mTimes;
            public AnimatedGif(Image img, int frames, int width, int height) {
                //Image img = Image.FromFile(path);
               // int frames = img.GetFrameCount(FrameDimension.Time);
                if(frames <= 1) throw new ArgumentException("Image not animated");
                byte[] times = img.GetPropertyItem(0x5100).Value;
                int frame = 0;
                for(; ; ) {
                    int dur = BitConverter.ToInt32(times, 4 * frame) * 10;
                    Bitmap new_bmp = new Bitmap(width, height);
                    using(Graphics g = Graphics.FromImage(new_bmp)) {
                        g.DrawImage(img, 0, 0, width, height);
                    }
                    mImages.Add(new AnimatedGifFrame(new_bmp, dur));
                    if(++frame >= frames) break;
                    img.SelectActiveFrame(FrameDimension.Time, frame);
                }
                img.Dispose();
            }
            public List<AnimatedGifFrame> Images { get { return mImages; } }
        }

        public class AnimatedGifFrame {
            private int mDuration;
            private Image mImage;
            internal AnimatedGifFrame(Image img, int duration) {
                mImage = img; mDuration = duration;
            }
            public Image Image { get { return mImage; } }
            public int Duration { get { return mDuration; } }
        }

        AnimatedGif animated_gif;

        int current_frame;

        public string image_path = "";

        public enum PictureWidgetType : int { Single, Folder };

        public PictureWidgetType WidgetType;

        private List<string> FolderImages;

        public HotkeyWidgetInstance(HotkeyWidget parent, WidgetSize widget_size, Guid instance_guid) {

            this.parent = parent;
            this.Guid = instance_guid;
            this.WidgetSize = widget_size;

            BlankWidget();

            LoadSettings();
        }

        private void UpdateWidget() {
            WidgetUpdatedEventArgs e = new WidgetUpdatedEventArgs();
            e.Offset = Point.Empty;
            if(animated_gif != null) {
                e.WaitMax = animated_gif.Images[current_frame].Duration;
            } else {
                e.WaitMax = mutex_timeout;
            }
            e.WidgetBitmap = BitmapCurrent;

            WidgetUpdated?.Invoke(this, e);
        }

        private void BlankWidget()
        {
            BitmapCurrent = new Bitmap(WidgetSize.ToSize().Width, WidgetSize.ToSize().Height);
            if (drawing_mutex.WaitOne(mutex_timeout))
            {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.Clear(Color.Gray);
                }
                drawing_mutex.ReleaseMutex();
            }
        }

        private void UpdateTask() {

            while(run_task) {

                while(pause_task) {
                    Thread.Sleep(100);
                }

                if (WidgetType == PictureWidgetType.Single) {
                    try {
                        // Show next frame
                        if(animated_gif != null && drawing_mutex.WaitOne(mutex_timeout)) {
                            //lock(BitmapCurrent) {
                                using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                                    g.DrawImage(animated_gif.Images[current_frame].Image, Offset);
                                }
                            //}
                            drawing_mutex.ReleaseMutex();
                            Thread.Sleep(animated_gif.Images[current_frame].Duration);
                            current_frame++;
                            if(current_frame == animated_gif.Images.Count) {
                                current_frame = 0;
                            }
                        }
                    } catch(Exception ex) { }

                } else if(WidgetType == PictureWidgetType.Folder) {

                    // Show next picture

                    // Load image
                    Bitmap img = null;

                    try {
                        img = new Bitmap(FolderImages[current_frame]);
                    } catch {

                    }

                    if(img != null) {

                        if(drawing_mutex.WaitOne(mutex_timeout)) {
                        //lock(BitmapCurrent) { 
                            try {
                                // Calculate width
                                int width = WidgetSize.ToSize().Width;
                                int height = WidgetSize.ToSize().Height;

                                lock(BitmapCurrent) {
                                    if(width > BitmapCurrent.Width) {
                                        width = BitmapCurrent.Width;
                                        height = (int)(width * (BitmapCurrent.Height * 1.0f / BitmapCurrent.Width));
                                    }
                                    if(height > BitmapCurrent.Height) {
                                        height = BitmapCurrent.Height;
                                        width = (int)(height * (BitmapCurrent.Width * 1.0f / BitmapCurrent.Height));
                                    }

                                    if(width > 0 && height > 0) {

                                        // Caluculate offset
                                        Offset.X = (BitmapCurrent.Width - width) / 2;
                                        Offset.Y = (BitmapCurrent.Height - height) / 2;

                                        // Draw image
                                        using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                                            g.Clear(Color.Black);
                                            g.DrawImage(img, Offset.X, Offset.Y, width, height);
                                        }
                                    }
                                }
                            } catch {
                            }
                            drawing_mutex.ReleaseMutex();
                        }
                    }

                    current_frame++;
                    if(current_frame == FolderImages.Count) {
                        current_frame = 0;
                    }

                    UpdateWidget();
                    Thread.Sleep(100);
                }

            }

        }

        public void LoadFolder(string path) {

            run_task = false;

            if(task_thread != null) {
                task_thread.Join(100);
            }

            current_frame = 0;
            image_path = path;
            WidgetType = PictureWidgetType.Folder;

            // Find files in folder
            FolderImages = new List<string>();
            string[] files = Directory.GetFiles(image_path);
            foreach(string file in files) {
                if(file.Length > 3) {
                    string file_end = file.Substring(file.Length - 4);
                    switch(file_end) {
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                        case ".gif":
                        case ".tif":
                        case ".bmp":
                        case ".ico":
                            FolderImages.Add(file); break;
                    }
                }
            }

            if(FolderImages.Count > 0) {
                pause_task = false;
                run_task = true;

                ThreadStart thread_start = UpdateTask;
                task_thread = new Thread(thread_start)
                {
                    IsBackground = true
                };
                task_thread.Start();
            }
        }

        public void LoadImage(string path) {

            run_task = false;

            if(task_thread != null && task_thread.IsAlive) {
                pause_task = false;
                task_thread.Join(100);
            }

            Image img = null;

            try {
                // Load image
                img = Image.FromFile(path);
            } catch(Exception ex) {
                //MessageBox.Show(ex.Message);
            }

            if(img != null) {

                // Calculate width
                int width = img.Width;
                int height = img.Height;

                //lock(BitmapCurrent) {
                if(drawing_mutex.WaitOne(mutex_timeout)) {
                    if(width > BitmapCurrent.Width) {
                        width = BitmapCurrent.Width;
                        height = (int)(width * (BitmapCurrent.Height * 1.0f / BitmapCurrent.Width));
                    }
                    if(height > BitmapCurrent.Height) {
                        height = BitmapCurrent.Height;
                        width = (int)(height * (BitmapCurrent.Width * 1.0f / BitmapCurrent.Height));
                    }

                    // Caluculate offset
                    Offset.X = (BitmapCurrent.Width - width) / 2;
                    Offset.Y = (BitmapCurrent.Height - height) / 2;

                    // Draw image
                    //lock(BitmapCurrent) {
                    using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                        g.Clear(Color.Black);
                        if(animated_gif == null) {
                            g.DrawImage(img, Offset.X, Offset.Y, width, height);
                        }
                    }
                    UpdateWidget();
                    drawing_mutex.ReleaseMutex();
                }

                // Handle gif
                animated_gif = null;
                try {
                    int frames = img.GetFrameCount(FrameDimension.Time);
                    if(frames > 1) {
                        animated_gif = new AnimatedGif(img, frames, width, height);
                        current_frame = 0;
                    }
                } catch(Exception ex) { }

                img.Dispose();

                image_path = path;
                WidgetType = PictureWidgetType.Single;

                if(animated_gif != null) {
                    pause_task = false;
                    run_task = true;

                    ThreadStart thread_start = UpdateTask;
                    task_thread = new Thread(thread_start);
                    task_thread.IsBackground = true;
                    task_thread.Start();
                }
            }
        }

        public void SaveSettings() {
            parent.WidgetManager.StoreSetting(this, "ImagePath", image_path);
            parent.WidgetManager.StoreSetting(this, "WidgetType", ((int)WidgetType).ToString());
            parent.WidgetManager.StoreSetting(this, "BackColor", ColorTranslator.ToHtml(BackColor));
            parent.WidgetManager.StoreSetting(this, "HotkeyAction", ActionGuid.ToString());
        }

        public void LoadSettings() {
            string path, type;
            if(parent.WidgetManager.LoadSetting(this, "ImagePath", out path)) {
                if(parent.WidgetManager.LoadSetting(this, "WidgetType", out type)) {
                    int widget_type;
                    if(int.TryParse(type, out widget_type)) {
                        switch(widget_type) {
                            case (int)PictureWidgetType.Single:
                                LoadImage(path); break;
                            case (int)PictureWidgetType.Folder:
                                LoadFolder(path); break;
                        }
                    }
                }
            }

            string color;
            if(parent.WidgetManager.LoadSetting(this, "BackColor", out color)) {
                try {
                    BackColor = ColorTranslator.FromHtml(color);
                } catch { }
            }

            if (parent.WidgetManager.LoadSetting(this, "HotkeyAction", out string actionGuidString))
            {
                Guid.TryParse(actionGuidString, out ActionGuid);
            }
        }
    }
}

