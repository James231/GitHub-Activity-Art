// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace GitHubActivityArt
{
    public class Configuration
    {
        [JsonProperty("auth")]
        public IAuthProvider Auth { get; set; }

        [JsonProperty("repo_name")]
        public string RepoName { get; set; }

        [JsonProperty("repo_owner")]
        public string RepoOwner { get; set; }

        [JsonProperty("image_file_name")]
        public string ImageFileName { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("test_mode")]
        public bool TestMode { get; set; }
    }
}
