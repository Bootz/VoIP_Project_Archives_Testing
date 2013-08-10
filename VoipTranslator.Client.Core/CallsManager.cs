using System;
using System.Threading.Tasks;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;

namespace VoipTranslator.Client.Core
{
    public class CallsManager
    {
        private readonly CommandBuilder _commandBuilder;
        private readonly IAudioDeviceResource _audioDevice;
        private readonly TransportManager _transportManager;
        private Command _incomingCallCommand = null;

        public CallsManager(CommandBuilder commandBuilder,
            IAudioDeviceResource audioDevice,
            TransportManager transportManager)
        {
            _commandBuilder = commandBuilder;
            _audioDevice = audioDevice;
            _transportManager = transportManager;
            _transportManager.CommandRecieved += _transportManager_OnCommandRecieved;
            _audioDevice.DataPacketCaptured += _audioDevice_OnDataPacketCaptured;
        }

        private void _audioDevice_OnDataPacketCaptured(object sender, BinaryDataEventsArgs e)
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
                    if (IsInCall)
                    {
                        var data = _commandBuilder.GetUnderlyingObject<byte[]>(e.Command);
                        _audioDevice.Play(data);
                    }
                    break;
                case CommandName.EndCall:
                    IsInCall = false;
                    _audioDevice.StopCapture();
                    CallEnded(this, EventArgs.Empty);
                    break;
                case CommandName.IncomingCall:
                    _incomingCallCommand = e.Command;
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

        public void Answer()
        {
            IsInCall = true;
            _audioDevice.StartCapture();
            _commandBuilder.ChangeUnderlyingObject(_incomingCallCommand, AnswerResultType.Answered);
            _transportManager.SendCommand(_incomingCallCommand);
        }

        public void Decline()
        {
            IsInCall = false;
            _commandBuilder.ChangeUnderlyingObject(_incomingCallCommand, AnswerResultType.Declined);
            _transportManager.SendCommand(_incomingCallCommand);
        }

        public void End()
        {
            IsInCall = false;
            _audioDevice.StopCapture();
            var command = _commandBuilder.Create(CommandName.EndCall, string.Empty);
            _transportManager.SendCommand(command);
        }
    }
}
