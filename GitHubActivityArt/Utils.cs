// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Octokit;

namespace GitHubActivityArt
{
    public static class Utils
    {
        public static bool DisableLog { get; set; } = false;

        public static DateTime TodaysDate { get; set; }

        public static int GetNumberOfCommitsToday(GitHubClient client, string username)
        {
            // only checks the last 30 events
            int count = 0;
            var options = new ApiOptions();
            options.StartPage = 1;
            options.PageSize = 30;
            options.PageCount = 1;

            IReadOnlyList<Activity> activities = client.Activity.Events.GetAllUserPerformed(username, options).Result;
            foreach (Activity a in activities)
            {
                if (a.CreatedAt.Date == GetTodaysDate())
                {
                    if (a.Type == "PushEvent")
                    {
                        PushEventPayload payload = (PushEventPayload)a.Payload;
                        count += payload.Commits.Count;
                    }

                    if (a.Type == "CreateEvent")
                    {
                        CreateEventPayload payload = (CreateEventPayload)a.Payload;
                        if (payload.RefType.StringValue == "repository")
                        {
                            count += 1;
                        }

                        if (payload.RefType.StringValue == "branch")
                        {
                            count += 2;
                        }
                    }
                }
            }

            return count;
        }

        public static Configuration GetConfiguration()
        {
            string filePath = GetLocalFilePath("config.json");

            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                StreamReader reader = new StreamReader(filePath);
                string contents = reader.ReadToEnd();
                reader.Close();

                Configuration config = JsonConvert.DeserializeObject<Configuration>(contents);
                return config;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void WriteToLog(string text)
        {
            if (DisableLog)
            {
                return;
            }

            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string outputLine = $"{timestamp} {text}";
            Console.WriteLine(outputLine);

            string logFilePath = GetLocalFilePath("output.log");
            try
            {
                StreamWriter writer = new StreamWriter(logFilePath, true);
                writer.WriteLine(outputLine);
                writer.Close();
            }
            catch
            {
            }
        }

        public static Bitmap GetBitmap(string path)
        {
            string localFilePath = GetLocalFilePath(path);
            if (!File.Exists(localFilePath))
            {
                return null;
            }

            Bitmap img;
            try
            {
                img = new Bitmap(localFilePath);
            }
            catch (Exception)
            {
                return null;
            }

            if (img == null)
            {
                return null;
            }

            if (img.Height != 7)
            {
                return null;
            }

            return img;
        }

        public static void SaveTestBitmap(Bitmap bitmap)
        {
            string localFilePath = GetLocalFilePath("test_result.bmp");
            if (File.Exists(localFilePath))
            {
                int startIdx = 1;
                while (File.Exists(GetLocalFilePath($"test_result_{startIdx}.bmp")))
                {
                    startIdx++;
                }

                localFilePath = GetLocalFilePath($"test_result_{startIdx}.bmp");
            }

            bitmap.Save(localFilePath);
        }

        public static DateTime GetTodaysDate()
        {
            return TodaysDate;
        }

        private static string GetLocalFilePath(string fileName)
        {
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string exeDir = Path.GetDirectoryName(exePath);
            return Path.Combine(exeDir, fileName);
        }
    }
}
