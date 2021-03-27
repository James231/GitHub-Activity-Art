// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Newtonsoft.Json;
using Octokit;

namespace GitHubActivityArt
{
    public class TokenAuth : IAuthProvider
    {
        [JsonProperty("type")]
        public string Type { get; } = "token";

        [JsonProperty("token")]
        public string Token { get; set; }

        public Credentials GetCredentials()
        {
            return new Credentials(Token);
        }
    }
}
