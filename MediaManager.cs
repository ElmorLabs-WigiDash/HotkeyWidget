using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace HotkeyWidget {
    public class MediaManager {

        CoreAudioController cac;

        public MediaManager() {
            cac = new CoreAudioController();
        }

        public bool GetMicrophoneMuteStatus() {
            CoreAudioDevice capture_dev = cac.GetDefaultDevice(DeviceType.Capture, Role.Multimedia);
            return capture_dev.IsMuted;
        }
    }
}
