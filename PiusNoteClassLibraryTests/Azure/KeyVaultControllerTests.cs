using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiusNoteClassLibrary.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiusNoteClassLibrary.Azure.Tests {
    [TestClass()]
    public class KeyVaultControllerTests {
        private KeyVaultController KeyVaultController;
        List<string> secretNames;

        [TestInitialize]
        public void Init() {
            // Arrange
            KeyVaultController = new KeyVaultController(null);
            secretNames = new List<string> {
                "OpenAIApiKey1",
                "OpenAIApiKey2",
                "OpenAIEndPoint",
                "PiusNoteSpeechServiceApi1",
                "PiusNoteSpeechServiceApi2",
                "PiusNoteSpeechServiceEndPoint",
            };
        }

        [TestMethod()]
        public void GetSecretAsyncTest() {
            // Act
            Console.WriteLine(secretNames[0]);
            var speechKey = KeyVaultController.GetSecretAsync(secretNames[0]);
            Console.WriteLine(speechKey.Result);
            Assert.IsNotNull(speechKey.Result);
        }

    }
}