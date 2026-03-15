using BF6PowerSaver.Services.Parsing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

namespace BF6PowerSaver.Services.Network
{
    public class HttpRequestService
    {
        private readonly CookieContainer _cookies;
        private readonly HttpClientHandler _handler;
        private readonly HttpClient _client;

        public HttpRequestService()
        {
            _cookies = new CookieContainer();
            _handler = new HttpClientHandler
            {
                CookieContainer = _cookies,
                UseCookies = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = true
            };
            _client = new HttpClient(_handler);
        }

        public async Task<int> GetEaIdFromUsername(string eaUsername)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(eaUsername))
                    throw new ArgumentException("Username cannot be null or empty.", nameof(eaUsername));

                string url = $"https://api.gametools.network/bf6/player/?name={Uri.EscapeDataString(eaUsername)}";

                using HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                GameToolsApiParser parser = new GameToolsApiParser();

                return parser.EaIdParser(json);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to get personalId from '{eaUsername}'. " +
                    $"Reason: {ex.Message}", ex);
            }
        }

    }
}
