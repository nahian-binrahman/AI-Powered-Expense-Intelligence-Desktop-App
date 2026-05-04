using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Polly;
using System.Windows;
using PersonalFinanceCategorizer.Models;

namespace PersonalFinanceCategorizer.Services
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey;

        public AiService()
        {
            LoadEnvFile();
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
            Console.WriteLine("API KEY STATUS: " + (_apiKey.Length > 0 ? "Loaded" : "Missing"));
        }

        private void LoadEnvFile()
        {
            try
            {
                string envPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".env");
                if (System.IO.File.Exists(envPath))
                {
                    foreach (var line in System.IO.File.ReadAllLines(envPath))
                    {
                        var parts = line.Split('=', 2);
                        if (parts.Length == 2)
                        {
                            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
                        }
                    }
                }
            }
            catch { /* Ignore errors */ }
        }

        public async Task<(string Category, double Confidence)> CategorizeTransactionAsync(string description)
        {
            Console.WriteLine("Processing: " + description);

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                Console.WriteLine("ERROR: API key not found.");
                MessageBox.Show("OpenAI API Key is missing! Please set the 'OPENAI_API_KEY' environment variable or hardcode it in AiService.cs.", "API Key Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return ("Unclear", 0.0);
            }

            var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 2,
                    Delay = TimeSpan.FromSeconds(1)
                })
                .Build();

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
                {
                    Content = JsonContent.Create(new
                    {
                        model = "gpt-3.5-turbo-0125",
                        messages = new[]
                        {
                            new
                            {
                                role = "system",
                                content = "Classify the transaction into categories: Food, Transport, Bills, Shopping, Health, Entertainment, Income, Unclear. If unclear, make best guess based on common spending behavior. Return JSON with category and confidence."
                            },
                            new { role = "user", content = description }
                        },
                        response_format = new { type = "json_object" }
                    })
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                Console.WriteLine("Sending request to AI...");
                var response = await pipeline.ExecuteAsync(async token => await _httpClient.SendAsync(request, token));

                Console.WriteLine("API RESPONSE STATUS: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("ERROR BODY: " + errorBody);
                    return ("Unclear", 0.0);
                }

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);
                var json = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                
                var result = JsonSerializer.Deserialize<AiResult>(json!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (result?.Category ?? "Unclear", result?.Confidence ?? 0.0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
                return ("Unclear", 0.0);
            }
        }

        private class AiResult
        {
            public string Category { get; set; } = "Unclear";
            public double Confidence { get; set; }
        }
    }
}