using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BF6PowerSaver.Services.Parsing
{
    public class GameToolsApiParser
    {
        public int EaIdParser(string json)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("response", out JsonElement responseElement))
                {
                    root = responseElement;
                }

                int presentationId = root
                    .GetProperty("personaId")
                    .GetInt32();

                return presentationId;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to get presentation ID from access code " +
                    $"Reason: {ex.Message}", ex);
            }
        }
    }
}
