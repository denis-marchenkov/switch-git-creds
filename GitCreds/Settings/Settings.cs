using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitCreds.Settings
{
    [Serializable]
    public class Settings
    {
        [JsonPropertyName("targetsToRemove")]
        public List<TargetToRemove>? TargetsToRemove { get; set; }

        [JsonPropertyName("profiles")]
        public List<Profile>? Profiles { get; set; }

        public static Settings? Read(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Settings>(jsonString);
        }

        public void Save(string filePath)
        {
            var s = JsonSerializer.Serialize(this);
            File.WriteAllText(filePath, s);
        }
    }
}
