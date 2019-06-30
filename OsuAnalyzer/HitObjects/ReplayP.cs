using OsuParsers.Replays.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.HitObjects
{
    public class ReplayP : Drawable
    {
        public MapAnalysisContext mw;
        public long preTime = 1000, postTime = 0;

        private Color getCol(bool r, bool l, int a = 255)
        {
            Color ans;
            if (r && l)
                ans = Color.Yellow;
            else if (r)
                ans = Color.Red;
            else if (l)
                ans = Color.Green;
            else
                ans = Color.Pink;
            return Color.FromArgb(a, ans);
        }

        public void draw(Graphics g, long time)
        {
            var frames = mw.rp.ReplayFrames;
            int idx = mw.rpIdx(time);
            ReplayFrame cf=null;
            var mnDelt = long.MaxValue;
            int rr=-1;
            Pen pp = null ;
            bool dn=false;
            long lt=-1;
            while (idx + 1 < frames.Count && frames[idx + 1].Time <= time + postTime)
            {
                var a = frames[idx];
                var b = frames[idx + 1];
                bool lda = (a.StandardKeys & OsuParsers.Enums.StandardKeys.K1) > 0;
                bool rda = (a.StandardKeys & OsuParsers.Enums.StandardKeys.K2) > 0;
                bool ldb = (b.StandardKeys & OsuParsers.Enums.StandardKeys.K1) > 0;
                bool rdb = (b.StandardKeys & OsuParsers.Enums.StandardKeys.K2) > 0;

                bool ldown = !lda && ldb, rdown = !rda && rdb;

                var delta = Math.Abs(time - b.Time);
                int alpha = (int)Math.Max(0, 255 - delta);

                bool lhold = lda && ldb, rhold = rda && rdb;
                if (ldown || rdown)
                {
                    lt = b.Time;
                }

                //TODO show side thing

                if (delta <= mnDelt)
                {   
                    cf = b;
                    dn = lhold || rhold;
                    mnDelt = delta;
                    rr = (int)Math.Min(mw.radius/3, Math.Max(0, time - lt));
                    pp = new Pen(getCol(ldb, rdb, alpha), 2);
                }

                //draw actual trail
                var xd = new Pen(getCol(lhold, rhold, alpha), 3);
                g.DrawLine(xd, a.X , a.Y , b.X  , b.Y);
                idx++;
            }
            if (cf != null)
            {
                //draw hitcircle
                if (dn)
                    g.DrawEllipse(pp, cf.X - rr, cf.Y - rr, rr * 2, rr * 2);
            }
        }

        public long deathTime()
        {
            return long.MaxValue;
        }
    }
}
