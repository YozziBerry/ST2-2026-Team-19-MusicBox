using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MusicBox.Services
{
    public class GeminiAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiAIService> _logger;

        public GeminiAIService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiAIService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetAnswerAsync(
            string userQuestion,
            string musicCatalogue,
            CancellationToken cancellationToken = default)
        {
            // Get the API key from User Secrets
            string? apiKey =
                _configuration["Gemini:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Gemini API key is missing."
                );
            }

            // Get the selected Gemini model
            string model =
                _configuration["Gemini:Model"]
                ?? "gemini-3.8-flash";

            // Prepare the request body
            var requestBody = new
            {
                systemInstruction = new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = @"
                                You are MusicBox AI,
                                a friendly music assistant.

                                Respond in Bulgarian unless
                                the user writes in another language.

                                Help users with music,
                                artists, genres and recommendations.

                                You have access to the MusicBox
                                music catalogue.

                                Only recommend songs that
                                actually exist in the supplied catalogue
                                when claiming that they are in MusicBox.

                                Never invent songs and claim
                                that they are stored in the database.

                                If the catalogue is empty,
                                explain that clearly.

                                Give helpful, clear and friendly answers.
                            "
                        }
                    }
                },

                contents = new[]
                {
                    new
                    {
                        role = "user",

                        parts = new[]
                        {
                            new
                            {
                                text = $@"
                                    User question:
                                    {userQuestion}

                                    MusicBox music catalogue:
                                    {musicCatalogue}
                                "
                            }
                        }
                    }
                },

                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 800
                }
            };

            // Build the Gemini API request
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/{Uri.EscapeDataString(model)}:generateContent"
            );

            // Authenticate the request
            request.Headers.Add(
                "x-goog-api-key",
                apiKey
            );

            request.Content = JsonContent.Create(requestBody);

            // Send the request to Google Gemini
            using HttpResponseMessage response =
                await _httpClient.SendAsync(
                    request,
                    cancellationToken
                );

            string responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken
                );

            // Check for API errors
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase
                    ?? "Unknown API error.";

                try
                {
                    using JsonDocument errorJson =
                        JsonDocument.Parse(responseBody);

                    if (errorJson.RootElement.TryGetProperty(
                        "error", out var error) &&
                        error.TryGetProperty(
                            "message", out var message))
                    {
                        errorMessage =
                            message.GetString() ?? errorMessage;
                    }
                }
                catch (JsonException)
                {
                    // Keep the default error message
                }

                _logger.LogWarning(
                    "Gemini API returned HTTP {StatusCode}.",
                    (int)response.StatusCode
                );

                throw new HttpRequestException(
                    $"Gemini API error: {errorMessage}",
                    null,
                    response.StatusCode
                );
            }

            // Read the JSON response
            using JsonDocument json =
                JsonDocument.Parse(responseBody);

            if (!json.RootElement.TryGetProperty(
                "candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
            {
                throw new InvalidOperationException(
                    "Gemini did not return a response."
                );
            }

            // Extract the generated text
            StringBuilder answer = new StringBuilder();

            var content = candidates[0].GetProperty("content");

            var parts = content.GetProperty("parts");

            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out var text))
                {
                    string? textValue = text.GetString();

                    if (!string.IsNullOrWhiteSpace(textValue))
                    {
                        answer.AppendLine(textValue);
                    }
                }
            }

            if (answer.Length == 0)
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty response."
                );
            }

            return answer.ToString().Trim();
        }
    }
}
