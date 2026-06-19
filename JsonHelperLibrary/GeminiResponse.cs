using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonHelperLibrary
{
    //Library untuk menangani response dari gemini API
    public class GeminiResponse
    {
        //Bentuk Json dari AudioRequest dari gemini API. 
        //Tipe yang dkirim adalah audio
        public static string BuildAudioRequest(string base64audio, string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new {text = prompt},
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = "audio/wav",
                                    data = base64audio
                                }
                            }
                        }
                    }
                }
            };

            return JsonSerializer.Serialize(requestBody);
        }

        //Fungsi yang menerima hasil generasi dari gemini API.
        public static string ParseTranscript(string responseJson)
        {
            try
            {
                var doc = JsonDocument.Parse(responseJson);
                var text = doc
                    .RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "Transcript tidak tersedia";
            }
            catch (Exception ex)
            {
                return $"Gagal memproses transcript : {ex.Message}";
            }
        }
    }
}
