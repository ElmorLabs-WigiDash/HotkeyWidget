using WigiDashWidgetFramework.WidgetUtility;
using PictureWidget;
using System;
using System.Drawing;
using System.IO;
using System.Collections.ObjectModel;
using HandyControl.Tools.Extension;

namespace HotkeyWidget {
    public partial class HotkeyWidgetInstance : PictureWidgetInstance
    {
        public HotkeyWidgetInstance(HotkeyWidgetServer parent, WidgetSize widgetSize, Guid instanceGuid, string resourcePath) : base(parent, widgetSize, instanceGuid, resourcePath)
        {
        }

        public ObservableCollection<Guid> Actions = new ObservableCollection<Guid>();
        public ObservableCollection<Guid> ActionsToggled = new ObservableCollection<Guid>();

        private bool _isToggled = false;

        public Image HotkeyImage = null;
        public Image HotkeyImageToggled = null;

        public string HotkeyImagePath = "";
        public string HotkeyImageToggledPath = "";

        public override void ClickEvent(ClickType click_type, int x, int y) {
            if(click_type == ClickType.Single) {

                if(_isToggled && ActionsToggled.Count != 0)
                {
                    foreach (Guid guid in ActionsToggled)
                    {
                        WidgetObject.WidgetManager.TriggerAction(guid);
                    }
                } else
                {
                    foreach (Guid guid in Actions)
                    {
                        WidgetObject.WidgetManager.TriggerAction(guid);
                    }
                }

                if(_isToggled && HotkeyImage != null)
                {

                    ImagePath = HotkeyImagePath;
                    CachedImagePath = HotkeyImagePath;
                    CachedImage = HotkeyImage;

                    _isToggled = false;

                } 
                else
                {
                    if (HotkeyImageToggled != null)
                    {
                        ImagePath = HotkeyImageToggledPath;
                        CachedImagePath = HotkeyImageToggledPath;
                        CachedImage = HotkeyImageToggled;

                        _isToggled = true;
                    }
                }

                DrawFrame();
            }

        }

        public void RemoveImage(bool isToggled)
        {
            if (isToggled)
            {
                HotkeyImageToggled = null;
                HotkeyImageToggledPath = null;

                UpdateImageCache();

                ImagePath = HotkeyImagePath;
                CachedImagePath = HotkeyImagePath;
                CachedImage = HotkeyImage;

                _isToggled = false;
            }
            else
            {
                HotkeyImage = null;
                HotkeyImagePath = null;
                
                UpdateImageCache();

                ImagePath = HotkeyImageToggledPath;
                CachedImagePath = HotkeyImageToggledPath;
                CachedImage = HotkeyImage;

                _isToggled = true;
            }

            DrawFrame();
        }

        public override void UpdateSettings()
        {
            UpdateImageCache();

            base.UpdateSettings();
        }

        public void UpdateImageCache()
        {
            if (!string.IsNullOrEmpty(HotkeyImagePath))
            {
                if (Path.GetExtension(HotkeyImagePath) == ".svg")
                {
                    CachedVectorArgs = (VectorColor, VectorScale);
                    HotkeyImage = GetBitmapFromSvg(HotkeyImagePath);
                }
                else
                {
                    try
                    {
                        byte[] imageBytes = File.ReadAllBytes(HotkeyImagePath);
                        HotkeyImage = Image.FromStream(new MemoryStream(imageBytes));
                    }
                    catch (Exception e)
                    {
                        //Logger.Error(e, $"Failed to load image from {HotkeyImagePath}");
                    }
                }
            }
            else
            {
                HotkeyImage = null;
            }

            if (!string.IsNullOrEmpty(HotkeyImageToggledPath))
            {
                if (Path.GetExtension(HotkeyImageToggledPath) == ".svg")
                {
                    CachedVectorArgs = (VectorColor, VectorScale);
                    HotkeyImageToggled = GetBitmapFromSvg(HotkeyImageToggledPath);
                }
                else
                {
                    try
                    {
                        byte[] imageBytes = File.ReadAllBytes(HotkeyImageToggledPath);
                        HotkeyImageToggled = Image.FromStream(new MemoryStream(imageBytes));
                    }
                    catch (Exception e)
                    {
                        //Logger.Error(e, $"Failed to load image from {HotkeyImageToggledPath}");
                    }
                }
            }
            else
            {
                HotkeyImageToggled = null;
            }

            CachedImage = null;
        }

        public override void LoadSettings()
        {
            Actions.AddRange(WidgetObject.WidgetManager.GetBoundActions(this));
            ActionsToggled.AddRange(WidgetObject.WidgetManager.GetBoundActions(this, 1));

            if (WidgetObject.WidgetManager.LoadFile(this, "Image", out string imagePath))
            {
                HotkeyImagePath = imagePath;
            }

            if (WidgetObject.WidgetManager.LoadFile(this, "ImageToggle", out string imageTogglePath))
            {
                HotkeyImageToggledPath = imageTogglePath;
            }

            UpdateImageCache();

            base.LoadSettings();
        }

    }
}

