using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace PiusNoteClassLibrary.Azure
{
    public class KeyVaultController
    {
        private readonly string kvUri;
        private readonly SecretClient client;

        public KeyVaultController(SecretClient secretClient, string? vaultUri = null, string? ENV = null)
        {
            kvUri = Environment.GetEnvironmentVariable("VAULT_URI");
            if (string.IsNullOrEmpty(kvUri)) { 
                // 환경 변수 없으면 예외 발생
                throw new ArgumentNullException("VAULT_URI is not configured in the environment variables.");
            }
            client = secretClient ?? new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
        }

        public void GetSecretUri() {
            Console.WriteLine(kvUri);
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await client.GetSecretAsync(secretName);
            return secret.Value.Value;
        }

        public async Task SetSecretAsync(string secretName, string secretValue) {
            await client.SetSecretAsync(secretName, secretValue);
        }

        public async Task DeleteSecretAsync(string secretName) {
            var operation = await client.StartDeleteSecretAsync(secretName);
            await operation.WaitForCompletionAsync();
        }
    }
}
