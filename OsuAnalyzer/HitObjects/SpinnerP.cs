using OsuParsers.Beatmaps.Objects;
using OsuParsers.Replays.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.HitObjects
{
    public class SpinnerP:HitObjectP<Spinner>
    {
        private Pen pn = new Pen(Color.White, 5);

        public SpinnerP(Spinner obj, MapAnalysisContext mw) : base(obj, mw)
        {
        }

        public override void draw(Graphics g, long time)
        {
            float cx = 512 / 2, cy=384/2;
            long a = Math.Max(0,obj.EndTime - time), b = obj.EndTime - obj.StartTime;
            float r = (384/2 - 20)*a/b;
            g.DrawEllipse(pn, cx - r, cy - r, r * 2, r * 2);
            base.draw(g, time);
        }

        public override long deathTime()
        {
            return obj.EndTime;
        }

        public override Judgement judge(ReplayFrame cf, bool keyDown, bool hit, TimingPoint currentTiming)
        {
            if (cf.Time >= obj.StartTime) return new Judgement.Good();
            return null;
        }
    }
}
