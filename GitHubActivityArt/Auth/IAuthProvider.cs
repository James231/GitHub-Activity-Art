// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using JsonSubTypes;
using Newtonsoft.Json;
using Octokit;

namespace GitHubActivityArt
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(BasicAuth), "basic")]
    [JsonSubtypes.KnownSubType(typeof(TokenAuth), "token")]
    public interface IAuthProvider
    {
        [JsonProperty("type")]
        public string Type { get; }

        public abstract Credentials GetCredentials();
    }
}
