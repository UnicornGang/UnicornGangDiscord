using System.Text.Json.Serialization;

namespace UnicornBot.Core.Model
{
    public struct Config
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }
    }
}