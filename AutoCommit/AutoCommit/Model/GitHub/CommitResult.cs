using Newtonsoft.Json;

namespace AutoCommit.Model.GitHub
{
    public class CommitResult : ShaInfo
    {
        [JsonProperty("tree")]
        public ShaInfo Tree
        {
            get;
            set;
        }
    }
}