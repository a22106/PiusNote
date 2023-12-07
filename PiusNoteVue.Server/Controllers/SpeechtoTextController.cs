using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using PiusNoteClassLibrary.Azure;
using PiusNoteClassLibrary;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PiusNoteVue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechtoTextController : ControllerBase
    {
        private readonly string subscriptionKey;
        private readonly string region = "koreacentral";
        private readonly KeyVaultController? KeyVaultController;

        public SpeechtoTextController() {
            KeyVaultController = new KeyVaultController(null);
            subscriptionKey = KeyVaultController.GetSecretAsync("PiusNoteSpeechServiceApi1").Result;
            if (string.IsNullOrEmpty(subscriptionKey)) {
                // 환경 변수 없으면 예외 발생
                throw new ArgumentNullException("SpeechtoTextSubscriptionKey is not configured in the environment variables.");
            }
        }

        public void GetSubKey() {
            Console.WriteLine(subscriptionKey);
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public bool Get()
        {
            // if the subscription key is not set, return an error message to the client.
            // otherwise, return true.

            if (string.IsNullOrEmpty(subscriptionKey)) {
                return false;
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// It gets the audio file from the client and sends it to the Speech to Text API.
        /// 오디오 파일을 클라이언트로부터 받아서 텍스트 API로 보냅니다.
        /// </summary>
        /// <param name="audioFile"></param>
        /// <returns>
        /// The text recognized by the Speech to Text API.
        /// 텍스트 API로 인식한 텍스트.
        /// </returns>
        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0) {
                return BadRequest("No audio file found.");
            }

            try {
                var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
                string speechRecognitionResult = await RecognizeSpeechAsync(audioFile, speechConfig);
                //Log.Information($"Text recognized: {speechRecognitionResult}");
                Console.WriteLine($"Text recognized: {speechRecognitionResult}");
                return Ok(speechRecognitionResult);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private async Task<string> RecognizeSpeechAsync(IFormFile audioFile, SpeechConfig speechConfig)
        {
            // Convert the IFormFile to a PullAudioInputStream so that it can be used with the Speech SDK.
            using var audioStream = audioFile.OpenReadStream();
            using var pullStream = AudioInputStream.CreatePullStream(new BinaryAudioStreamReader(audioStream));
            using var audioInput = AudioConfig.FromStreamInput(pullStream);
            using var recognizer = new SpeechRecognizer(speechConfig, audioInput);

            var result = await recognizer.RecognizeOnceAsync();
            return result.Text;
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
