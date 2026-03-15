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

                // Expecting: {"results":[ { ... } ]}
                if (!root.TryGetProperty("results", out JsonElement resultsElement) ||
                    resultsElement.ValueKind != JsonValueKind.Array ||
                    resultsElement.GetArrayLength() == 0)
                {
                    throw new KeyNotFoundException("Property 'results' missing or empty.");
                }

                JsonElement first = resultsElement[0];

                if (!first.TryGetProperty("personaId", out JsonElement personaElem))
                    throw new KeyNotFoundException("Property 'personaId' was not found in the first result object.");

                // Accept either a JSON number or a numeric string.
                if (personaElem.ValueKind == JsonValueKind.Number)
                    return personaElem.GetInt32();

                if (personaElem.ValueKind == JsonValueKind.String &&
                    int.TryParse(personaElem.GetString(), out int parsedId))
                    return parsedId;

                throw new FormatException("Property 'personaId' is not a numeric value.");
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to get personalId Reason: {ex.Message}", ex);
            }
        }

        public int RankParser(string json)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;

                // check top-level "playerProfiles" array
                if (!root.TryGetProperty("playerProfiles", out JsonElement profilesElement) ||
                    profilesElement.ValueKind != JsonValueKind.Array ||
                    profilesElement.GetArrayLength() == 0)
                {
                    throw new KeyNotFoundException("Property 'playerProfiles' missing or empty.");
                }

                JsonElement firstProfile = profilesElement[0];

                // check "playerCard" object inside the first profile
                if (!firstProfile.TryGetProperty("playerCard", out JsonElement playerCardElem) ||
                    playerCardElem.ValueKind != JsonValueKind.Object)
                {
                    throw new KeyNotFoundException("Property 'playerCard' missing or not an object.");
                }

                // get "rank" from playerCard
                if (!playerCardElem.TryGetProperty("rank", out JsonElement rankElem))
                    throw new KeyNotFoundException("Property 'rank' was not found in playerCard.");

                if (rankElem.ValueKind == JsonValueKind.Number)
                    return rankElem.GetInt32();

                if (rankElem.ValueKind == JsonValueKind.String &&
                    int.TryParse(rankElem.GetString(), out int parsedRank))
                    return parsedRank;

                throw new FormatException("Property 'rank' is not a numeric value.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get rank Reason: {ex.Message}", ex);
            }
        }
    }
}
