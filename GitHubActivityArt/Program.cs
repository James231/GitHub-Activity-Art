// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Globalization;
using Octokit;

namespace GitHubActivityArt
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Configuration config = Utils.GetConfiguration();

            if (config.TestMode)
            {
                Utils.DisableLog = true;
                for (int i = 0; i < 500; i++)
                {
                    DateTime fakeDateToday = DateTime.Today.AddDays(i);
                    Utils.TodaysDate = fakeDateToday;
                    Start(config);
                }

                TestOutput.OutputTestImage(config);
                Console.WriteLine("Finished Test. Output 'test_result.bmp'.");
                return;
            }

            Utils.TodaysDate = DateTime.Today;
            Start(config);
        }

        private static void Start(Configuration config)
        {
            if (config == null)
            {
                Utils.WriteToLog("Failed to load configuration");
                return;
            }

            if (config.Auth == null)
            {
                Utils.WriteToLog("Configuration Error: 'auth' credentials are missing or in an invalid format");
                return;
            }

            if (string.IsNullOrEmpty(config.RepoName))
            {
                Utils.WriteToLog("Configuration Error: 'repo_name' is missing");
                return;
            }

            if (string.IsNullOrEmpty(config.StartDate))
            {
                Utils.WriteToLog("Configuration Error: 'start_date' is missing");
                return;
            }

            Bitmap bitmap = Utils.GetBitmap(config.ImageFileName);
            if (bitmap == null)
            {
                Utils.WriteToLog("Configuration Error: 'image_file_name' does not point to valid image file");
                return;
            }

            DateTime startDate;
            if (!DateTime.TryParseExact(config.StartDate, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                Utils.WriteToLog("Configuration Error: 'start_date' should be in format 'ddMMyyyy'");
                return;
            }

            // Get todays required number of commits
            int numbCommits = CommitCalc.NumCommitsToday(bitmap, startDate);
            if (config.TestMode)
            {
                TestOutput.AddDay(numbCommits);
                return;
            }

            var client = new GitHubClient(new ProductHeaderValue("James231"));
            client.Credentials = config.Auth.GetCredentials();

            // Get number of commits already made today
            int commitsToday = Utils.GetNumberOfCommitsToday(client, "James231");
            int commitsToMake = Math.Max(0, numbCommits - commitsToday);
            if (commitsToMake == 0)
            {
                Utils.WriteToLog("Finished: No commits made today.");
                return;
            }

            for (int i = 0; i < commitsToMake; i++)
            {
                // Make a commit
                CommitMaker.MakeCommit(client, config, i + 1);
            }

            Utils.WriteToLog($"Finished: {commitsToMake} commits made today.");
        }
    }
}
