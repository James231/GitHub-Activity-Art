// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace GitHubActivityArt
{
    public static class TestOutput
    {
        private static List<int> outputCommits = new List<int>();

        public static void AddDay(int numCommits)
        {
            outputCommits.Add(numCommits);
        }

        public static void OutputTestImage(Configuration config)
        {
            // set to monday before start date
            DateTime startDate = DateTime.ParseExact(config.StartDate, "ddMMyyyy", CultureInfo.InvariantCulture);
            int offset = (int)(DateTime.Today - GetSundayBefore(startDate.Date)).TotalDays % 7;

            int width = (int)Math.Ceiling((outputCommits.Count + offset) / 7D);
            int height = 7;

            Bitmap bitmap = new Bitmap(width, height);
            int i = -offset;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (i >= 0 && i < outputCommits.Count)
                    {
                        int numCommits = outputCommits[i];
                        bitmap.SetPixel(x, y, GetColFromNumCommits(numCommits));
                    }

                    i++;
                }
            }

            Utils.SaveTestBitmap(bitmap);
        }

        private static Color GetColFromNumCommits(int numCommits)
        {
            foreach (CommitCalc.GitHubColor gcol in CommitCalc.GithubColors)
            {
                if ((numCommits >= gcol.MinCommitNum) && (numCommits <= gcol.MaxCommitNum))
                {
                    return gcol.Color;
                }
            }

            return Color.White;
        }

        private static DateTime GetSundayBefore(DateTime date)
        {
            DateTime newDate = date;
            while (newDate.DayOfWeek != DayOfWeek.Sunday)
            {
                newDate = newDate.AddDays(-1);
            }

            return newDate;
        }
    }
}
