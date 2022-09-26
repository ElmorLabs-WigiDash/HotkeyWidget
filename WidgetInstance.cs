using FrontierWidgetFramework;
using FrontierWidgetFramework.WidgetUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance {

        private Mutex drawing_mutex = new Mutex();
        private const int mutex_timeout = 1000;

        private const int WM_APPCOMMAND = 0x319;

        public enum MediaAction : int {
            APPCOMMAND_VOLUME_MUTE = 8,
            APPCOMMAND_VOLUME_DOWN = 9,
            APPCOMMAND_VOLUME_UP = 10,

            APPCOMMAND_MEDIA_NEXTTRACK = 11,
            APPCOMMAND_MEDIA_PREVIOUSTRACK = 12,
            APPCOMMAND_MEDIA_PLAY_PAUSE = 14,

            APPCOMMAND_MICROPHONE_VOLUME_MUTE = 24,
            APPCOMMAND_MICROPHONE_VOLUME_DOWN = 25,
            APPCOMMAND_MICROPHONE_VOLUME_UP = 26,
            APPCOMMAND_MIC_ON_OFF_TOGGLE = 44
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
        IntPtr wParam, IntPtr lParam);

        //Bitmap BitmapBackground;
        Bitmap BitmapCurrent;

        public Color BackColor = Color.FromArgb(35, 35, 35);
        public enum Action { Execute, Hotkey, MediaFunction, BrightnessUp, BrightnessDown, Screenshot, ChangeScreen };

        public Action ActionType;
        public string[] Param;
        //public ModifierKeys HotkeyModifers;
        public Keys HotkeyKey;
        public List<Keys> HotkeyModifiers;
        public string HotkeyTarget;
        public MediaAction MediaFunctionAction;

        public string icon_path = "";
        public string background_path = "";

        public HotkeyWidgetInstance(HotkeyWidgetObject parent, Guid instance_guid) {

            this.parent = parent;
            this.Guid = instance_guid;

            WidgetSize = new WidgetSize(1, 1);

            //BitmapBackground = (Bitmap)Bitmap.FromFile("widget_251x194_grey_gradient_dithered.png");//new Bitmap(251, 194, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            BitmapCurrent = new Bitmap(WidgetSize.ToSize().Width, WidgetSize.ToSize().Height);

            /*using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                g.DrawImage(BitmapBackground, 0, 0, BitmapCurrent.Width, BitmapCurrent.Height);
            }*/
            //SetIcon("widget_251x194_grey_gradient_dithered.png");

            LoadSettings();

        }

        private void Hook_KeyPressed(object sender, KeyboardHook.KeyPressedEventArgs e) {
            ClickEvent(ClickType.Single, 0, 0);
        }

        public void SaveSettings() {

            // Save setting
            parent.WidgetManager.StoreSetting(this, "ActionType", ((int)ActionType).ToString());
            parent.WidgetManager.StoreSetting(this, "MediaFunctionAction", ((int)MediaFunctionAction).ToString());
            for(int i = 0; i<Param.Length; i++) {
                parent.WidgetManager.StoreSetting(this, "Param"+(i+1).ToString(), Param[i]);
            }
            parent.WidgetManager.StoreSetting(this, "IconPath", icon_path);
            parent.WidgetManager.StoreSetting(this, "BackgroundColor", ColorTranslator.ToHtml(BackColor));
            parent.WidgetManager.StoreSetting(this, "BackgroundPath", background_path);

        }

        public void LoadSettings() {

            Param = new string[3];

            // Check if existing configuration exists
            string str_actiontype;
            if(parent.WidgetManager.LoadSetting(this, "ActionType", out str_actiontype)) {
                ActionType = (Action)(int.Parse(str_actiontype));
            } else {
                ActionType = Action.Execute;
                Param[0] = "cmd.exe";
                Param[1] = "";
            }
            string str_mediafunction;
            if(parent.WidgetManager.LoadSetting(this, "MediaFunctionAction", out str_mediafunction)) {
                MediaFunctionAction = (MediaAction)(int.Parse(str_mediafunction));
            }
            for(int i = 0; i < Param.Length; i++) {
                parent.WidgetManager.LoadSetting(this, "Param"+(i+1).ToString(), out Param[i]);
            }
            if(parent.WidgetManager.LoadSetting(this, "IconPath", out icon_path)) {
                icon_path = "";
            }
            string str_backcolor; 
            if(parent.WidgetManager.LoadSetting(this, "BackgroundColor", out str_backcolor)) {
                BackColor = ColorTranslator.FromHtml(str_backcolor);
            }
            if(parent.WidgetManager.LoadSetting(this, "BackgroundPath", out background_path)) {
                background_path = "widget_251x194_grey_gradient_dithered.png";
            }
            
            //SetBackground(background_path);
            ActionUpdated();
            SetIcon(icon_path);

        }


        /*public void SetBackground(string path) {
            background_path = path;

            if(drawing_mutex.WaitOne(mutex_timeout)) {
                if(path.Length == 0) {
                    BitmapBackground = new Bitmap(BitmapCurrent);
                    using(Graphics g = Graphics.FromImage(BitmapBackground)) {
                        g.Clear(BackColor);
                    }
                } else {
                    try {
                        BitmapBackground = (Bitmap)Bitmap.FromFile(path);
                    } catch(Exception ex) {

                    }
                }
                drawing_mutex.ReleaseMutex();
            }
        }*/

        Font FontHeader = new Font("Lucida Console", 40, FontStyle.Bold);
        Font FontRegular = new Font("Lucida Console", 20, FontStyle.Regular);
        Font FontBold = new Font("Lucida Console", 20, FontStyle.Bold);

        public void SetAction(Action action_type, string[] param) {
            this.ActionType = action_type;
            this.Param = param;
            ActionUpdated();
        }

        public void SetAction(Action action_type, MediaAction action) {
            this.ActionType = action_type;
            this.MediaFunctionAction = action;
            ActionUpdated();
        }

        private void ActionUpdated() {

            HotkeyModifiers = new List<Keys>();

            switch(ActionType) {
                case Action.BrightnessUp:
                    if(string.IsNullOrEmpty(icon_path)) {
                        SetIcon("icons/bright-up.png");
                    } 
                    break;
                case Action.BrightnessDown:
                    if(string.IsNullOrEmpty(icon_path)) {
                        SetIcon("icons/bright-down.png");
                    }
                    break;
                case Action.Execute:
                    if(string.IsNullOrEmpty(icon_path)) {
                        SetIcon("icons/boot.png");
                    }
                    break;
                case Action.MediaFunction:
                    if(string.IsNullOrEmpty(icon_path)) {
                        switch(MediaFunctionAction) {
                            case MediaAction.APPCOMMAND_VOLUME_UP: SetIcon("icons/vol-up.png"); break;
                            case MediaAction.APPCOMMAND_VOLUME_DOWN: SetIcon("icons/vol-down.png"); break;
                            case MediaAction.APPCOMMAND_VOLUME_MUTE: SetIcon("icons/sound-off.png"); break;
                        }
                    }
                    break;
                case Action.Hotkey:
                    try {
                        HotkeyKey = (Keys)Enum.Parse(typeof(Keys), Param[1], true); 
                        if(Param[0].Contains("CTRL")) {
                            HotkeyModifiers.Add(Keys.ControlKey);
                        }
                        if(Param[0].Contains("ALT")) {
                            HotkeyModifiers.Add(Keys.Menu);
                        }
                        if(Param[0].Contains("SHFT")) {
                            HotkeyModifiers.Add(Keys.ShiftKey);
                        }
                        if(Param.Length > 2) {
                            HotkeyTarget = Param[2];
                        } else {
                            HotkeyTarget = "";
                        }
                    } catch(Exception ex) {
                        HotkeyKey = Keys.None;
                    }

                    break;
                case Action.ChangeScreen:
                    if(Param.Length > 1) {
                        SetText(Param[1]);
                    }
                    break;
            }
            if(drawing_mutex.WaitOne(100)) {
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }

        volatile bool run_task = false;

        public void SetIcon(string path) {

            icon_path = path;

            if(task_thread != null && task_thread.IsAlive) {
                run_task = false;
                task_thread.Join(1000);
            }

            if(icon_path.EndsWith("gif", true, System.Globalization.CultureInfo.CurrentCulture)) {
                // Load gif
                LoadGif(icon_path);
            } else {
                Bitmap bmp_icon = null;

                try {
                    bmp_icon = new Bitmap(path);
                } catch(Exception) {
                    bmp_icon = null;
                }

                if(bmp_icon == null) {
                    return;
                }

                int width_out = bmp_icon.Width > BitmapCurrent.Width ? BitmapCurrent.Width : bmp_icon.Width;
                int height_out = width_out * bmp_icon.Height / bmp_icon.Width;

                if(height_out > BitmapCurrent.Height) {
                    height_out = BitmapCurrent.Height;
                    width_out = height_out * bmp_icon.Width / bmp_icon.Height;
                }
                if(drawing_mutex.WaitOne(mutex_timeout)) {
                    using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                        g.Clear(this.BackColor);
                        /*if(BitmapBackground != null) {

                            int width = BitmapBackground.Width;
                            int height = BitmapBackground.Height;
                            if(width > BitmapCurrent.Width) {
                                width = BitmapCurrent.Width;
                                height = (int)(BitmapBackground.Height * 1.0f / BitmapBackground.Width * BitmapCurrent.Width);
                            }
                            if(height > BitmapCurrent.Height) {
                                height = BitmapCurrent.Height;
                                width = (int)(BitmapBackground.Width * 1.0f / BitmapBackground.Height * BitmapCurrent.Height);
                            }
                            int x = (BitmapCurrent.Width - width) / 2;
                            int y = (BitmapCurrent.Height - height) / 2;
                            g.DrawImage(BitmapBackground, x, y, width, height);
                        }*/
                        if(bmp_icon != null) {
                            g.DrawImage(bmp_icon, (BitmapCurrent.Width - width_out) / 2, (BitmapCurrent.Height - height_out) / 2, width_out, height_out);
                        }
                    }
                    UpdateWidget();
                    drawing_mutex.ReleaseMutex();
                }
            }

        }

        // https://social.microsoft.com/Forums/en-US/fcb7d14d-d15b-4336-971c-94a80e34b85e/editing-animated-gifs-in-c?forum=netfxbcl
        public class AnimatedGif {
            private List<AnimatedGifFrame> mImages = new List<AnimatedGifFrame>();
            PropertyItem mTimes;
            public AnimatedGif(string path) {
                Image img = Image.FromFile(path);
                int frames = img.GetFrameCount(FrameDimension.Time);
                if(frames <= 1) throw new ArgumentException("Image not animated");
                byte[] times = img.GetPropertyItem(0x5100).Value;
                int frame = 0;
                for(; ; ) {
                    int dur = BitConverter.ToInt32(times, 4 * frame) * 10;
                    mImages.Add(new AnimatedGifFrame(new Bitmap(img), dur));
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

        Thread task_thread;

        public void LoadGif(string path) {

            try {
                animated_gif = new AnimatedGif(path);
                current_frame = 0;
                if(drawing_mutex.WaitOne(mutex_timeout)) {
                    using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                        g.Clear(Color.Black);
                    }
                    drawing_mutex.ReleaseMutex();
                }
                if(task_thread != null && task_thread.IsAlive) {
                    run_task = false;
                    task_thread.Join();
                }
                ThreadStart ts = new ThreadStart(UpdateTask);
                task_thread = new Thread(ts);
                task_thread.IsBackground = true;
                run_task = true;
                task_thread.Start();
            } catch(Exception ex) {
                animated_gif = null;
            }
        }
        private void UpdateTask() {

            while(run_task) {
                if(animated_gif != null && drawing_mutex.WaitOne(mutex_timeout)) {
                    using(Graphics g = Graphics.FromImage(BitmapCurrent)) {
                        int width = animated_gif.Images[current_frame].Image.Width;
                        int height = animated_gif.Images[current_frame].Image.Height;
                        if(width > BitmapCurrent.Width) {
                            width = BitmapCurrent.Width;
                            height = (int)(animated_gif.Images[current_frame].Image.Height * 1.0f / animated_gif.Images[current_frame].Image.Width * BitmapCurrent.Width);
                        }
                        if(height > BitmapCurrent.Height) {
                            height = BitmapCurrent.Height;
                            width = (int)(animated_gif.Images[current_frame].Image.Width * 1.0f / animated_gif.Images[current_frame].Image.Height * BitmapCurrent.Height);
                        }
                        int x = (BitmapCurrent.Width - width) / 2;
                        int y = (BitmapCurrent.Height - height) / 2;
                        g.DrawImage(animated_gif.Images[current_frame].Image, x, y, width, height);
                    }
                    UpdateWidget();
                    drawing_mutex.ReleaseMutex();
                    Thread.Sleep(animated_gif.Images[current_frame].Duration);
                    current_frame++;
                    if(current_frame == animated_gif.Images.Count) {
                        current_frame = 0;
                    }
                }
            }
        }

        Font TextFont = new Font("Basic Square 7", 28);

        public void SetText(string text) {

            using(Graphics g = Graphics.FromImage(BitmapCurrent)) {

                g.Clear(this.BackColor);

                /*if(BitmapBackground != null) {

                    int width = BitmapBackground.Width;
                    int height = BitmapBackground.Height;
                    if(width > BitmapCurrent.Width) {
                        width = BitmapCurrent.Width;
                        height = (int)(BitmapBackground.Height * 1.0f / BitmapBackground.Width * BitmapCurrent.Width);
                    }
                    if(height > BitmapCurrent.Height) {
                        height = BitmapCurrent.Height;
                        width = (int)(BitmapBackground.Width * 1.0f / BitmapBackground.Height * BitmapCurrent.Height);
                    }
                    int x = (BitmapCurrent.Width - width) / 2;
                    int y = (BitmapCurrent.Height - height) / 2;

                    g.DrawImage(BitmapBackground, x, y, width, height);
                }*/

                SizeF string_size = g.MeasureString(text, TextFont, BitmapCurrent.Width);
                g.DrawString(text, TextFont, Brushes.White, new RectangleF((BitmapCurrent.Width - string_size.Width) / 2, (BitmapCurrent.Height - string_size.Height) / 2, BitmapCurrent.Width, BitmapCurrent.Height));
            }
        }

        private void UpdateWidget() {
            WidgetUpdatedEventArgs e = new WidgetUpdatedEventArgs();
            e.WidgetBitmap = BitmapCurrent;
            WidgetUpdated?.Invoke(this, e);
        }

        public void RequestUpdate() {
            if(drawing_mutex.WaitOne(100)) {
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }

        public void ClickEvent(ClickType type, int x, int y) {
            switch(this.ActionType) {
                case Action.Execute:
                    ProcessStartInfo psi = new ProcessStartInfo(Param[0], Param[1]);
                    try {
                        Process.Start(psi);
                    } catch(Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case Action.Hotkey:
                    if(HotkeyTarget == "") {
                        Keyboard kb = new Keyboard();
                        foreach(Keys key in HotkeyModifiers) {
                            kb.Send((uint)key, false);
                        }
                        if(HotkeyKey != Keys.None) {
                            kb.Send((uint)(HotkeyKey), false);
                            kb.Send((uint)(HotkeyKey), true);
                        }
                        foreach(Keys key in HotkeyModifiers) {
                            kb.Send((uint)key, true);
                        }
                        /*if(HotkeyKey != Keys.None) {
                            kb.Send((uint)(HotkeyKey), true);
                        }*/
                    } else {
                        SendMessage.sendKeystroke(HotkeyTarget, HotkeyModifiers, HotkeyKey);
                        
                    }
                    break;
                case Action.MediaFunction:
                    IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
                    SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)((int)MediaFunctionAction << 16));
                    break;
                case Action.BrightnessUp:
                    int level = 0;
                    /*if(parent.WidgetManager.GetBrightness(out level)) {
                        level += 10;
                        if(level > 100) level = 100;
                        parent.WidgetManager.SetBrightness(level);
                    }*/
                    break;
                case Action.BrightnessDown:
                    int level_down = 0;
                    /*if(parent.WidgetManager.GetBrightness(out level_down)) {
                        level_down -= 10;
                        if(level_down < 10) level_down = 10;
                        parent.WidgetManager.SetBrightness(level_down);
                    }*/
                    break;
                case Action.Screenshot:
                    // https://stackoverflow.com/questions/362986/capture-the-screen-into-a-bitmap
                    var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                               Screen.PrimaryScreen.Bounds.Height,
                               PixelFormat.Format32bppArgb);

                    // Create a graphics object from the bitmap.
                    var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                    // Take the screenshot from the upper left corner to the right bottom corner.
                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                                Screen.PrimaryScreen.Bounds.Y,
                                                0,
                                                0,
                                                Screen.PrimaryScreen.Bounds.Size,
                                                CopyPixelOperation.SourceCopy);

                    // Save the screenshot to the specified path that the user has chosen.
                    bmpScreenshot.Save(Param[0] + "\\FrontierScreenshot" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".png", ImageFormat.Png);
                    break;
                case Action.ChangeScreen:
                    if(Param.Length > 0) {
                        //parent.WidgetManager.RequestScreen(new Guid(Param[0]));
                    }
                    break;

            }

        }

        public void Dispose() {
            run_task = false;
        }

        public void EnterSleep() {
            run_task = false;
        }

        public void ExitSleep() {
            if(task_thread != null && task_thread.IsAlive) {
                run_task = false;
                task_thread.Join();
            }
            ThreadStart ts = new ThreadStart(UpdateTask);
            task_thread = new Thread(ts);
            task_thread.IsBackground = true;
            run_task = true;
            task_thread.Start();
        }
    }
        
}

