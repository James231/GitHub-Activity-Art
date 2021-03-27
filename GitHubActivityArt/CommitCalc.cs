// -------------------------------------------------------------------------------------------------
// GitHub Activity Art - © Copyright 2021 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace GitHubActivityArt
{
    public static class CommitCalc
    {
        private static Random random;

        public static readonly GitHubColor[] GithubColors = new GitHubColor[5]
        {
            // with inclusive upper bounds
            new GitHubColor(Color.FromArgb(255, 255, 255), 0, 0), // white
            new GitHubColor(Color.FromArgb(155, 233, 168), 1, 2), // lightest
            new GitHubColor(Color.FromArgb(64, 196, 99), 4, 6),
            new GitHubColor(Color.FromArgb(48, 161, 78), 8, 9),
            new GitHubColor(Color.FromArgb(33, 110, 57), 12, 15), // darkest
        };

        public static int NumCommitsToday(Bitmap bitmap, DateTime startDate)
        {
            int numDaysInCycle = bitmap.Width * bitmap.Height;
            int daysPastStart = (int)Math.Round((Utils.GetTodaysDate() - startDate).TotalDays);
            int pixelToUse = daysPastStart % numDaysInCycle;
            int rowNum = pixelToUse % 7;
            int colNum = (int)Math.Floor(pixelToUse / 7D);
            Color color = bitmap.GetPixel(colNum, rowNum);
            int closestColIndex = ClosestColIndex(color);
            return GetRandom(GithubColors[closestColIndex].MinCommitNum, GithubColors[closestColIndex].MaxCommitNum);
        }

        private static int GetRandom(int min, int max)
        {
            if (random == null)
            {
                random = new Random();
            }

            return random.Next(min, max + 1);
        }

        private static int ClosestColIndex(Color color)
        {
            int closestCol = 0;
            double minDist = double.PositiveInfinity;
            for (int i = 0; i < GithubColors.Length; i++)
            {
                double colDist = ColorDiff(GithubColors[i].Color, color);
                if (colDist < minDist)
                {
                    minDist = colDist;
                    closestCol = i;
                }
            }

            return closestCol;
        }

        private static double ColorDiff(Color one, Color two)
        {
            return Math.Pow(one.R - two.R, 2) + Math.Pow(one.G - two.G, 2) + Math.Pow(one.B - two.B, 2);
        }

        public class GitHubColor
        {
            public GitHubColor(Color c, int min, int max)
            {
                Color = c;
                MinCommitNum = min;
                MaxCommitNum = max;
            }

            public Color Color { get; set; }

            public int MinCommitNum { get; set; }

            public int MaxCommitNum { get; set; }
        }
    }
}
