// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Newtonsoft.Json;
using Octokit;

namespace GitHubActivityArt
{
    public class BasicAuth : IAuthProvider
    {
        [JsonProperty("type")]
        public string Type { get; } = "basic";

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public Credentials GetCredentials()
        {
            return new Credentials(Username, Password);
        }
    }
}
