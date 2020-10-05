using Newtonsoft.Json;

namespace AutoCommit.Model.GitHub
{
    public class UpdateReferenceInfo : ShaInfo
    {
        [JsonProperty("force")]
        public bool Force => true;

        public UpdateReferenceInfo(string sha)
        {
            Sha = sha;
        }
    }
}