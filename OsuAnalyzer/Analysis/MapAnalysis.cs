using OsuAnalyzer.HitObjects;
using OsuAnalyzer.Drawing;
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
using System.Numerics;

namespace OsuAnalyzer
{

    public class MapAnalysisContext
    {
        public Theme th = new Theme();

        public string osuRoot;

        public Score sc;
        public DbBeatmap dmp;

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


        public Beatmap bm;
        public Replay rp;

        private void loadFiles()
        {
            var bmPath = Path.Combine(osuRoot, "Songs", dmp.FolderName, dmp.FileName);
            var rpPath = Path.Combine(osuRoot, @"Data\r", $"{dmp.MD5Hash}-{sc.ScoreTimestamp.ToFileTimeUtc()}.osr");
            bm = Parser.ParseBeatmap(bmPath);
            rp = Parser.ParseReplay(rpPath);
        }

        public double radius;
        public double spins_per_second;
        public double hit50, hit100, hit300, hit0;
        public double preempt, fade_in;

        private void calcDiff()
        {
            radius = 54.4 - 4.48 * bm.DifficultySection.CircleSize;

            double OD = bm.DifficultySection.OverallDifficulty;
            if (OD < 5) spins_per_second = 5 - 2 * (5 - OD) / 5;
            else if (OD == 5) spins_per_second = 5;
            else spins_per_second = 5 + 2.5 * (OD - 5) / 5;

            hit50 = 150 + 50 * (5 - OD) / 5;
            hit100 = 100 + 40 * (5 - OD) / 5;
            hit300 = 50 + 30 * (5 - OD) / 5;
            hit0 = hit50+1;//TODO actually implement

            double AR = bm.DifficultySection.ApproachRate;
            if (AR < 5) preempt = 1200 + 600 * (5 - AR) / 5;
            else if (AR == 5) preempt = 1200;
            else fade_in = preempt = 1200 - 750 * (AR - 5) / 5;

            if (AR < 5) fade_in = 800 + 400 * (5 - AR) / 5;
            else if (AR == 5) fade_in = 800;
            else fade_in = 800 - 500 * (AR - 5) / 5;
        }

        public List<HitObjectP> objs;

        private void convObjs()
        {
            //TODO proper stacking algorithm
            var objsL = bm.HitObjects;
            var dst = bm.GeneralSection.StackLeniency;
            for (int i = objsL.Count - 1; i >= 0;)
            {
                Point v = objsL[i].Position;
                int cnt = 0;
                while (i >= 0 && objsL[i].Position == v)
                {
                    objsL[i].Position = new Point((int)(v.X-cnt*dst), (int)(v.Y-cnt*dst));
                    cnt++;
                    i--;
                }
            }

            objs = objsL.Select<HitObject, HitObjectP>(obj =>
            {
                if (obj is Circle)
                    return new CircleP(obj as Circle, this);

                if (obj is Spinner)
                    return new SpinnerP(obj as Spinner, this);

                if (obj is Slider)
                    return new SliderP(obj as Slider, this);

                throw new ArgumentException("obj is not recognized type");
            }).ToList();
        }

        public List<int> nxtObjIdx, combo;
        public List<Judgement> judgements;
        public List<Judgement.Bad> badJudgements;
        public List<Judgement.Ok> okJudgements;
        public List<Judgement.Good> goodJudgements;
        private int curCombo = 0;

        private void addJudgement(Judgement j, int bi, int ri, long time = -1)
        {
            if (time == -1) time = rp.ReplayFrames[ri].Time;
            j.time = time;
            j.bmIdx = bi;
            objs[bi].judgement = j;
            if (j is Judgement.Bad)
            {
                curCombo = 0;
                badJudgements.Add(j as Judgement.Bad);
            }
            else
            {
                curCombo+=objs[bi].comboBoost();
            }
            combo.Add(curCombo);
            if (j is Judgement.Ok) okJudgements.Add(j as Judgement.Ok);
            if (j is Judgement.Good) goodJudgements.Add(j as Judgement.Good);
            judgements.Add(j);
        }

        private void procReplay()
        {

            var r = rp.ReplayFrames;
            var t = bm.TimingPoints;
            var b = bm.HitObjects;

            judgements = new List<Judgement>(b.Count);
            combo = new List<int>(b.Count);
            nxtObjIdx = new List<int>(r.Count);

            goodJudgements = new List<Judgement.Good>(rp.Count300);
            okJudgements = new List<Judgement.Ok>(rp.Count50 + rp.Count100);
            badJudgements = new List<Judgement.Bad>(rp.CountMiss);

            int bi = 0, ti = -1;
            bool pl = false, pr = false;

            for (int ri = 0; ri < r.Count; ri++)
            {
                var cf = r[ri];
                bool ld = (cf.StandardKeys & OsuParsers.Enums.StandardKeys.K1) > 0;
                bool rd = (cf.StandardKeys & OsuParsers.Enums.StandardKeys.K2) > 0;
                bool down = ld || rd;
                bool lhit = ld && !pl, rhit = rd && !pr;
                bool hit = lhit || rhit;

                //find last timing point before current time
                while (ti + 1 < t.Count && t[ti + 1].Offset <= cf.Time)
                    ti++;

                //find first not missed note
                while (bi < objs.Count && b[bi].EndTime+hit0 < cf.Time)
                {
                    addJudgement(new Judgement.Bad.NoClick(), bi, ri);
                    bi++;
                }
                
                //store the next object for this replay frame
                nxtObjIdx.Add(bi);

                //judge note if there is any left
                //also make sure it is not too early to judge it
                if (bi < objs.Count && b[bi].StartTime - hit0 <= cf.Time)
                {
                    TimingPoint tp = null;
                    if (ti >= 0) tp = t[ti];
                    var res = objs[bi].judge(cf,down, hit, tp);
                    //judgement was passed for this note, goto next one
                    if (res != null)
                    {
                        addJudgement(res, bi, ri);
                        bi++;
                    }
                }

                pr = rd;
                pl = ld;
            }
        }


        /// <summary>
        /// Adjusts window of time shown of past
        /// </summary>
        public double preTime = 1000;

        private long _lrptime = -1;
        private int _lrpres;
        /// <summary>
        /// gets first drawn replay frame at given time, including preTime
        /// </summary>
        /// <param name="time">current time</param>
        /// <returns>first drawn replay frame visible since time</returns>
        public int rpIdx(long time)
        {
            if (time != _lrptime)
            {
                _lrpres = Util.LowerBound(rp.ReplayFrames, x =>
                  x.Time >= time - preTime
                );
                _lrptime = time;
            }
            return _lrpres;
        }

        private long _lbmtime = -1;
        private int _lbmres;
        /// <summary>
        /// gets latest object which will be on screen at current time
        /// </summary>
        /// <param name="time">current time</param>
        /// <returns>latest object which will be on screen</returns>
        public int bmIdx(long time)
        {
            if (time != _lbmtime)
            {
                _lbmres = Util.LowerBound(bm.HitObjects, x =>
                     x.StartTime - preempt > time
                  ) - 1;
                _lbmres = Math.Max(_lbmres, 0);
                _lbmtime = time;
            }
            return _lbmres;
        }
    }

}
