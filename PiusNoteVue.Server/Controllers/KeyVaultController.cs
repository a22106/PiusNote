using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PiusNoteVue.Server.Controllers
{
    public class KeyVaultController : ControllerBase
    {
        private readonly SecretClient _secretClient;

        public KeyVaultController(IConfiguration configuration)
        {
            var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
            _secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value.Value;
        }

        public static string GetSecret(string secretName)
        {
            var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
            var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
            var secret = secretClient.GetSecret(secretName);
            return secret.Value.Value;
        }

        //[HttpGet]
        //public async Task<string> Get()
        //{
        //    var secret = await _secretClient.GetSecretAsync("PiusNoteSpeechServiceApi2");
        //    return secret.Value.Value;
        //}
    }
}
