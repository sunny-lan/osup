using OsuAnalyzer.Drawables;
using OsuParsers;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Database.Objects;
using OsuParsers.Replays;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace OsuAnalyzer
{
    public class Theme
    {
        public Color miss = Color.Red;
        public Color hit50 = Color.Yellow;
        public Color hit100 = Color.Green;
        public Color hit300 = Color.Blue;

        public Color circleOutline = Color.White;
        public Color circleBody = Color.Gray;
        public float circleBodyWidth = 10, circleOutlineWidth=3;
        public float approachCircleWidth = 2;
        
        //TODO move everything into theme
    }

    public class MapAnalysisContext
    {
        public Theme th=new Theme();
        public float radius;
        public double spins_per_second;
        public double hit50, hit100, hit300;
        public double preempt, fade_in;
        public Beatmap mp;
        public Replay rp;
        public string osuRoot;
        public Score sc;
        public DbBeatmap dmp;
        public List<Drawable> objs;

        /// <summary>
        /// Adjusts window of time shown of past
        /// </summary>
        public double preTime=1000;

        private long _ltime=-1;
        private int _lres;
        public int getRpIdx(long time)
        {
            if (time != _ltime)
            {
                _lres = Util.LowerBound(rp.ReplayFrames, x =>
                  x.Time >= time - preTime
                );
                _ltime = time;
            }
            return _lres;
        }

        private void loadFiles()
        {
            var bmPath = Path.Combine(osuRoot, "Songs", dmp.FolderName, dmp.FileName);
            var rpPath = Path.Combine(osuRoot, @"Data\r", $"{dmp.MD5Hash}-{sc.ScoreTimestamp.ToFileTimeUtc()}.osr");
            mp = Parser.ParseBeatmap(bmPath);
            rp = Parser.ParseReplay(rpPath);
        }

        private void calcDiff()
        {
            radius = 54.4f - 4.48f * mp.DifficultySection.CircleSize;
            radius *= 512 / 640f;//WTF

            double OD = mp.DifficultySection.OverallDifficulty;
            if (OD < 5) spins_per_second = 5 - 2 * (5 - OD) / 5;
            else if (OD == 5) spins_per_second = 5;
            else spins_per_second = 5 + 2.5 * (OD - 5) / 5;

            hit50 = 150 + 50 * (5 - OD) / 5;
            hit100 = 100 + 40 * (5 - OD) / 5;
            hit300 = 50 + 30 * (5 - OD) / 5;

            double AR = mp.DifficultySection.ApproachRate;
            if (AR < 5) preempt = 1200 + 600 * (5 - AR) / 5;
            else if (AR == 5) preempt = 1200;
            else fade_in = preempt = 1200 - 750 * (AR - 5) / 5;

            if (AR < 5) fade_in = 800 + 400 * (5 - AR) / 5;
            else if (AR == 5) fade_in = 800;
            else fade_in = 800 - 500 * (AR - 5) / 5;
        }

        private void convObjs()
        {
            var objsL = mp.HitObjects;
            objs = objsL.Select<HitObject, Drawable>(obj =>
            {
                if (obj is Circle)
                    return new CircleView(obj as Circle, this);

                if (obj is Spinner)
                    return new SpinnerView(obj as Spinner, this);

                if (obj is Slider)
                    return new SliderView(obj as Slider, this);

                throw new ArgumentException("obj is not recognized type");
            }).ToList();
        }

        private void procReplay()
        {

        }

        public MapAnalysisContext(DbBeatmap dmp, Score sc, string osuRoot)
        {
            this.sc = sc;
            this.dmp = dmp;
            this.osuRoot = osuRoot;

            loadFiles();

            calcDiff();

            convObjs();

            procReplay();
        }
    }

}
