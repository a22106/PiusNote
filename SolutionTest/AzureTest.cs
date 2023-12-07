using PiusNoteClassLibrary;
using PiusNoteClassLibrary.Azure;

namespace SolutionTest {
    [Parallelizable(ParallelScope.Self)] // ParallelScope.Self means that the test class is thread-safe
    [TestFixture]

    public class AzureVaultTest : PageTest {
        string TestSecretName;
        string TestSecretValue;
        string kvUri;
        private SecretClient client;


        [SetUp]
        public void SetUp() {
            // Arrange
            TestSecretName = "TestSecret";
            TestSecretValue = "sk-1234567890";

            // Act
            kvUri = Environment.GetEnvironmentVariable("VAULT_URI");
            client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Assert
            Assert.That(kvUri, Is.Not.Null);

        }

        [Test]
        public async Task SetSecret() {
            await client.SetSecretAsync(TestSecretName, TestSecretValue);
            Assert.That(client.GetSecret(TestSecretName).Value.Value, Is.EqualTo(TestSecretValue));
        }

        [Test]
        public async Task RetreiveSecret() {
            var secret = await client.GetSecretAsync(TestSecretName);
            Console.WriteLine(secret.Value.Value);
        }

        [Test]
        public async Task DeleteSecret() {
            DeleteSecretOperation operation = await client.StartDeleteSecretAsync(TestSecretName);
            await operation.WaitForCompletionAsync();
            Assert.Pass();
        }
    }

    [Parallelizable(ParallelScope.Self)] // ParallelScope.Self means that the test class is thread-safe
    [TestFixture]
    internal class SpeechServiceTest {
        private KeyVaultController KeyVaultController;
        List<string> secretNames;

        [SetUp]
        public void SetUp() {
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

        [Test]
        public void GetVaultUri() {
            // Act
            KeyVaultController.GetSecretUri();
        }

        [Test]
        public async Task GetSecret() {
            // Act
            Console.WriteLine(secretNames[0]);
            var speechKey = KeyVaultController.GetSecretAsync(secretNames[0]);
            Console.WriteLine(speechKey.Result);
            Assert.That(speechKey.Result, Is.Not.Null);
        }

    }
}
