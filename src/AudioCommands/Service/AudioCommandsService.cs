using System;
using System.Timers;
using AudioCommands.Helpers;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform.Events.MQTT;
using IOTLinkAPI.Platform.HomeAssistant;

namespace AudioCommands.Service
{
    public class AudioCommandsService : ServiceAddon
    {
        private Timer _monitorTimer;
        private string _audioTopic = "audio/volume";

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            GetManager().SubscribeTopic(this, "audio/volume", OnAudioVolumeSetMessage);
            GetManager().SubscribeTopic(this, "audio/mute", OnAudioMuteMessage);
            GetManager().SubscribeTopic(this, "audio/default", OnAudioSetDefaultMessage);
            GetManager().SubscribeTopic(this, "audio/default-comms", OnAudioSetDefaultCommsMessage);


            GetManager().PublishDiscoveryMessage(this, _audioTopic, "Volume", new HassDiscoveryOptions
            {
                Id = "Volume",
                Name = "Volume",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:volume-high"
            });
            _monitorTimer = new Timer();
            _monitorTimer.Interval = 10000;
            _monitorTimer.Elapsed += TimerElapsed;
            _monitorTimer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var volume = AudioPlatformHelper.GetAudioDeviceInfo(Guid.Empty).Volume;
                LoggerHelper.Info($"Current volume is {volume}");
                GetManager().PublishMessage(this, _audioTopic, volume.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send volume " + exception);
            }
        }

        private async void OnAudioVolumeSetMessage(object sender, MQTTMessageEventEventArgs e)
        {
            LoggerHelper.Verbose("OnAudioVolumeSetMessage: Message received");
            try
            {
                if (string.IsNullOrWhiteSpace(e.Message.GetPayload()))
                {
                    LoggerHelper.Warn("OnAudioVolumeSetMessage: Received an empty message payload");
                    return;
                }

                string[] args = e.Message.GetPayload().Split(',');
                double volume = Convert.ToDouble(args[args.Length == 2 ? 1 : 0]);
                Guid guid = args.Length >= 2 ? Guid.Parse(args[0]) : Guid.Empty;

                await AudioPlatformHelper.SetAudioVolume(guid, volume);
                LoggerHelper.Debug("OnAudioVolumeSetMessage: Volume set to {0}", volume);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug("OnAudioVolumeSetMessage: Wrong Payload: {0}", ex.Message);
            }
        }

        private async void OnAudioMuteMessage(object sender, MQTTMessageEventEventArgs e)
        {
            LoggerHelper.Verbose("OnAudioMuteMessage: Message received");
            try
            {
                if (string.IsNullOrWhiteSpace(e.Message.GetPayload()))
                {
                    await AudioPlatformHelper.ToggleAudioMute(Guid.Empty);
                    LoggerHelper.Debug("OnAudioMuteMessage: Toggling current audio mute flag.");
                    return;
                }

                string[] args = e.Message.GetPayload().Split(',');
                bool mute = Convert.ToBoolean(args[args.Length == 2 ? 1 : 0]);
                Guid guid = args.Length >= 2 ? Guid.Parse(args[0]) : Guid.Empty;

                await AudioPlatformHelper.SetAudioMute(guid, mute);
                LoggerHelper.Debug("OnAudioMuteMessage: Mute flag set to {0}", mute);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug("OnAudioMuteMessage: Wrong Payload: {0}", ex.Message);
            }
        }

        private void OnAudioSetDefaultMessage(object sender, MQTTMessageEventEventArgs e)
        {
            LoggerHelper.Verbose("OnAudioSetDefaultMessage: Message received");
            try
            {
                if (string.IsNullOrWhiteSpace(e.Message.GetPayload()))
                {
                    LoggerHelper.Warn("OnAudioSetDefaultMessage: Received an empty message payload");
                    return;
                }

                Guid guid = Guid.Parse(e.Message.GetPayload());

                AudioPlatformHelper.SetAudioDefault(guid);
                LoggerHelper.Debug("OnAudioVolumeSetMessage: Set Audio Device {0} to Default", guid);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug("OnAudioSetDefaultMessage: Wrong Payload: {0}", ex.Message);
            }
        }

        private void OnAudioSetDefaultCommsMessage(object sender, MQTTMessageEventEventArgs e)
        {
            LoggerHelper.Verbose("OnAudioSetDefaultCommsMessage: Message received");
            try
            {
                if (string.IsNullOrWhiteSpace(e.Message.GetPayload()))
                {
                    LoggerHelper.Warn("OnAudioSetDefaultCommsMessage: Received an empty message payload");
                    return;
                }

                Guid guid = Guid.Parse(e.Message.GetPayload());

                AudioPlatformHelper.SetAudioDefaultComms(guid);
                LoggerHelper.Debug("OnAudioSetDefaultCommsMessage: Set Audio Device {0} to Default Comms", guid);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug("OnAudioSetDefaultCommsMessage: Wrong Payload: {0}", ex.Message);
            }
        }
    }
}
