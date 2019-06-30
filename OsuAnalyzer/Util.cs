using NAudio.Wave;
using OsuParsers;
using OsuParsers.Beatmaps;
using OsuParsers.Database.Objects;
using OsuParsers.Replays;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuAnalyzer
{

    

    public class MapWrap
    {
        public float radius;
        public double spins_per_second;
        public double hit50, hit100, hit300;
        public double preempt, fade_in;
        public Beatmap map;
        public Replay rp;
        public string osuRoot;
        public Score sc;
        public DbBeatmap dmp;

        public MapWrap(DbBeatmap dmp, Score sc, string osuRoot)
        {
            this.sc = sc;
            this.dmp = dmp;
            this.osuRoot = osuRoot;

            var bmPath = Path.Combine(osuRoot, "Songs", dmp.FolderName, dmp.FileName);
            var rpPath = Path.Combine(osuRoot, @"Data\r", $"{dmp.MD5Hash}-{sc.ScoreTimestamp.ToFileTimeUtc()}.osr");
            map = Parser.ParseBeatmap(bmPath);
            rp= Parser.ParseReplay(rpPath);

            radius = 54.4f - 4.48f * map.DifficultySection.CircleSize;

            double OD = map.DifficultySection.OverallDifficulty;
            if (OD < 5) spins_per_second = 5 - 2 * (5 - OD) / 5;
            else if (OD == 5) spins_per_second = 5;
            else spins_per_second = 5 + 2.5 * (OD - 5) / 5;

            hit50 = 150 + 50 * (5 - OD) / 5;
            hit100 = 100 + 40 * (5 - OD) / 5;
            hit300 = 50 + 30 * (5 - OD) / 5;

            double AR = map.DifficultySection.ApproachRate;
            if (AR < 5) preempt = 1200 + 600 * (5 - AR) / 5;
            else if (AR == 5) preempt = 1200;
            else fade_in = preempt = 1200 - 750 * (AR - 5) / 5;

            if (AR < 5) fade_in = 800 + 400 * (5 - AR) / 5;
            else if (AR == 5) fade_in = 800;
            else fade_in = 800 - 500 * (AR - 5) / 5;
        }
    }

    public delegate bool Pred<T>(T x);

    public static class Util
    {
        //expects predicate which is true for upper x
        //finds the first index for which predicate is true
        public static int LowerBound<T>(List<T> list, Pred<T> pred)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            int lo = 0, hi = list.Count ;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;  
                if (!pred(list[m])) lo = m + 1;
                else hi = m;
            }
            return lo;
        }
    }
}
