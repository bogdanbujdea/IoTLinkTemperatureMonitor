using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AudioCommands.Helpers
{
    public class AudioPlatformHelper
    {
        /// <summary>
        /// Return a <see cref="AudioDeviceInfo"/> object with all current device information.
        /// </summary>
        /// <returns><see cref="AudioDeviceInfo"/> object</returns>
        public static List<AudioDeviceInfo> GetAudioDevices()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            return WindowsAudioApi.GetAudioDevices();
        }

        /// <summary>
        /// Return an <see cref="AudioDeviceInfo"/> object with the device information.
        /// </summary>
        /// <param name="guid">Device Unique Identifider</param>
        /// <returns></returns>
        public static AudioDeviceInfo GetAudioDeviceInfo(Guid guid)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            return WindowsAudioApi.GetAudioDeviceInfo(guid);
        }

        /// <summary>
        /// Set device as the primary playback audio device
        /// </summary>
        /// <param name="guid">Device Unique Identifider</param>
        /// <returns>Boolean</returns>
        public static bool SetAudioDefault(Guid guid)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            return WindowsAudioApi.SetAudioDefault(guid);
        }

        /// <summary>
        /// Set device as the primary communication playback audio device
        /// </summary>
        /// <param name="guid">Device Unique Identifider</param>
        /// <returns>Boolean</returns>
        public static bool SetAudioDefaultComms(Guid guid)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            return WindowsAudioApi.SetAudioDefaultComms(guid);
        }

        /// <summary>
        /// Set primary audio device volume mute flag
        /// </summary>
        /// <param name="guid">Device Unique Identifider</param>
        /// <param name="mute">Boolean indicating the desired mute flag</param>
        /// <returns>Boolean</returns>
        public static Task<bool> SetAudioMute(Guid guid, bool mute)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            return WindowsAudioApi.SetAudioMute(guid, mute);
        }

        /// <summary>
        /// Toggle primary audio device volume mute flag
        /// </summary>
        /// <param name="guid">Device Unique Identifider</param>
        /// <returns>Boolean</returns>
        public static Task<bool> ToggleAudioMute(Guid guid)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            return WindowsAudioApi.ToggleAudioMute(guid);
        }

        /// <summary>
        /// Set the primary audio device volume level
        /// </summary>
        /// <param name="guid">Device Unique Identifider</param>
        /// <param name="volume">Desired volume level (0-100)</param>
        public static async Task SetAudioVolume(Guid guid, double volume)
        {
            if (volume < 0 || volume > 100)
                throw new Exception("Volume level needs to be between 0 and 100");

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            await WindowsAudioApi.SetAudioVolume(guid, volume);
        }
    }
}
