using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioCommands.Helpers
{
    public static class WindowsAudioApi
    {
        public static List<AudioDeviceInfo> GetAudioDevices()
        {
            return AudioController.GetInstance().GetAudioDevices();
        }

        public static AudioDeviceInfo GetAudioDeviceInfo(Guid guid)
        {
            return AudioController.GetInstance().GetAudioDeviceInfo(guid);
        }

        public static bool SetAudioDefault(Guid guid)
        {
            return AudioController.GetInstance().SetAudioDefault(guid);
        }

        public static bool SetAudioDefaultComms(Guid guid)
        {
            return AudioController.GetInstance().SetAudioDefaultComms(guid);
        }

        public static Task<bool> SetAudioMute(Guid guid, bool mute)
        {
            return AudioController.GetInstance().SetAudioMute(guid, mute);
        }

        public static Task<bool> ToggleAudioMute(Guid guid)
        {
            return AudioController.GetInstance().ToggleAudioMute(guid);
        }

        public static async Task SetAudioVolume(Guid guid, double volume)
        {
            await AudioController.GetInstance().SetAudioVolume(guid, volume);
        }
    }
}
