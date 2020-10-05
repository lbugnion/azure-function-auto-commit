using Newtonsoft.Json;

namespace AutoCommit.Model.GitHub
{
    public class ShaInfo
    {
        [JsonProperty("sha")]
        public string Sha
        {
            get;
            set;
        }
    }
}