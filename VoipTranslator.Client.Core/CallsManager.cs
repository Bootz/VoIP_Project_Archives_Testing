using System;
using System.Threading.Tasks;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Client.Core
{
    public class CallsManager
    {
        private readonly AccountManager _accountManager;
        private readonly CommandBuilder _commandBuilder;
        private readonly IAudioDeviceResource _audioDevice;
        private readonly TransportManager _transportManager;

        public CallsManager(AccountManager accountManager,
            CommandBuilder commandBuilder,
            IAudioDeviceResource audioDevice,
            TransportManager transportManager)
        {
            _accountManager = accountManager;
            _commandBuilder = commandBuilder;
            _audioDevice = audioDevice;
            _transportManager = transportManager;
            _transportManager.CommandRecieved += _transportManager_OnCommandRecieved;
            _audioDevice.DataPacketCaptured += _audioDevice_DataPacketCaptured;
        }

        private void _audioDevice_DataPacketCaptured(object sender, BinaryDataEventsArgs e)
        {
            if (!IsInCall)
                return;

            var cmd = _commandBuilder.Create(CommandName.VoicePacket, e.Data);
            _transportManager.SendCommand(cmd);
        }

        private void _transportManager_OnCommandRecieved(object sender, CommandEventArgs e)
        {
            switch (e.Command.Name)
            {
                case CommandName.VoicePacket:
                    var data = _commandBuilder.GetUnderlyingObject<byte[]>(e.Command);
                    _audioDevice.Play(data);
                    break;
                case CommandName.EndCall:
                    _audioDevice.StopCapture();
                    IsInCall = false;
                    CallEnded(this, EventArgs.Empty);
                    break;
                case CommandName.IncomingCall:
                    IncomingCall(this, new CallEventsArgs { Number = _commandBuilder.GetUnderlyingObject<string>(e.Command)});
                    break;
            }
        }

        public event EventHandler IncomingCall = delegate { }; 

        public event EventHandler CallEnded = delegate { };  

        public bool IsInCall { get; private set; }

        public async Task<bool> Dial(string number)
        {
            IsInCall = true;
            Command cmd = _commandBuilder.Create(CommandName.Dial, number);
            Command response = await _transportManager.SendCommandAndGetAnswerAsync(cmd);
            var result = _commandBuilder.GetUnderlyingObject<DialResult>(response);
            if (result.Result != DialResultType.Answered)
            {
                IsInCall = false;
            }
            else
            {
                _audioDevice.StartCapture();
            }
            return IsInCall;
        }

        public Task Answer()
        {
            return null;
        }

        public Task Decline()
        {
            return null;
        }

        public Task End()
        {
            return null;
        }
    }
}
