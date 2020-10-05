using Newtonsoft.Json;

namespace AutoCommit.Model.GitHub
{
    public class UploadInfo
    {
        public const string Utf8 = "utf-8";

        [JsonProperty("content")]
        public string Content
        {
            get;
            set;
        }

        [JsonProperty("encoding")]
        public string Encoding => Utf8;
    }
}