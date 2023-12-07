using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PiusNoteVue.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiusNoteVue.Server.Controllers.Tests {
    [TestClass()]
    public class SpeechtoTextControllerTests {
        SpeechtoTextController speechtoTextController;

        [TestInitialize]
        public void Init() {
            speechtoTextController = new SpeechtoTextController();
        }

        [TestMethod()]
        public void SpeechtoTextControllerTest() {
            speechtoTextController = new SpeechtoTextController();
            speechtoTextController.GetSubKey();
            Assert.IsNotNull(speechtoTextController);
        }

        [TestMethod()]
        public void GetTest() {
            Assert.IsTrue(speechtoTextController.Get());
        }

        [TestMethod()]
        public async Task Post_ValidAudioFile_ReturnRecognizedText() {
            // Arrange
            var mockAudioFile = new Mock<IFormFile>();
            mockAudioFile.Setup(x => x.Length).Returns(1024);
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Form.Files).Returns(new FormFileCollection { mockAudioFile.Object });

            var controller = new SpeechtoTextController();

            // Act
            var response = await controller.Post(mockAudioFile.Object);
            Console.WriteLine(response);

            // Assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult);

            var recognizedText = okResult.Value as string;
            Assert.IsInstanceOfType(recognizedText, typeof(string));

            // Verify that RecognizedSpeechAsync() was called with the correct parameters
            //controller.Verify(controller => controller.RecognizedSpeechAsync(It.IsAny<Stream>()), Times.Once);

        }
    }
}