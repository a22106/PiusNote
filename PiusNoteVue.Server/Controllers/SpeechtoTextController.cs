﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
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
        private readonly string subscriptionKey = KeyVaultController.GetSecret("PiusNoteSpeechServiceApi1");
        private readonly string region = "koreacentral";

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        // It gets the audio file from the client and sends it to the Speech to Text API.
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("No audio file found.");
            }

            try
            {
                var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
                string speechRecognitionResult = await RecognizeSpeechAsync(audioFile, speechConfig);
                //Log.Information($"Text recognized: {speechRecognitionResult}");
                Console.WriteLine($"Text recognized: {speechRecognitionResult}");
                return Ok(speechRecognitionResult);
            }
            catch (Exception ex)
            {
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
