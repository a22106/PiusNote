using Microsoft.CognitiveServices.Speech.Audio;

namespace PiusNoteVue.Server.Controllers
{
    public class BinaryAudioStreamReader : PullAudioInputStreamCallback
    {
        private readonly BinaryReader _audioStream;

        public BinaryAudioStreamReader(Stream audioStream)
        {
            _audioStream = new BinaryReader(audioStream);
        }

        public override int Read(byte[] dataBuffer, uint size)
        {
            return _audioStream.Read(dataBuffer, 0, (int)size);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _audioStream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}