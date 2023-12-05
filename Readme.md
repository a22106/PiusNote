# PiusNoteVue

## 환경변수 설정

### Azure config

- AZURE_CLIENT_ID: The client ID of your registered Azure application.
- AZURE_TENANT_ID: The tenant ID of the Azure Active Directory being used.
- AZURE_CLIENT_SECRET: The client secret that was generated for your registered 

##### Windows
```bash
setx SPEECH_KEY "YOUR_SPEECH_KEY"
setx SPEECH_REGION "YOUR_SPEECH_REGION"
setx AZURE_OPENAI_KEY "REPLACE_WITH_YOUR_KEY_VALUE_HERE"
setx AZURE_OPENAI_ENDPOINT "REPLACE_WITH_YOUR_ENDPOINT_HERE"
```