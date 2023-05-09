using System.Text.Json.Serialization;

namespace GitCreds.Settings
{
    [Serializable]
    public class Profile
    {
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
