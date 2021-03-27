// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Octokit;

namespace GitHubActivityArt
{
    public static class CommitMaker
    {
        public static void MakeCommit(GitHubClient client, Configuration config, int commitNumber)
        {
            string yearString = Utils.GetTodaysDate().ToString("yyyy-MM-dd");

            string headMasterRef = "refs/heads/master";
            Reference masterReference = client.Git.Reference.Get(config.RepoOwner, config.RepoName, headMasterRef).Result;
            Commit latestCommit = client.Git.Commit.Get(config.RepoOwner, config.RepoName, masterReference.Object.Sha).Result;
            var nt = new NewTree { BaseTree = latestCommit.Tree.Sha };

            var textBlob = new NewBlob { Encoding = EncodingType.Utf8, Content = $"Commit number {commitNumber} today" };
            var textBlobRef = client.Git.Blob.Create(config.RepoOwner, config.RepoName, textBlob);

            nt.Tree.Add(new NewTreeItem { Path = yearString, Mode = "100644", Type = TreeType.Blob, Sha = textBlobRef.Result.Sha });
            var newTree = client.Git.Tree.Create(config.RepoOwner, config.RepoName, nt).Result;

            var newCommit = new NewCommit($"commit {yearString} {commitNumber}", newTree.Sha, masterReference.Object.Sha);
            var commit = client.Git.Commit.Create(config.RepoOwner, config.RepoName, newCommit).Result;

            Reference reff = client.Git.Reference.Update(config.RepoOwner, config.RepoName, headMasterRef, new ReferenceUpdate(commit.Sha)).Result;
        }
    }
}
