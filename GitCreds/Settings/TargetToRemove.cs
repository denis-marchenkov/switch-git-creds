using System.Text.Json.Serialization;

namespace GitCreds.Settings
{
    [Serializable]
    public class TargetToRemove
    {
        [JsonPropertyName("target")]
        public string Target { get; set; }
    }
}
