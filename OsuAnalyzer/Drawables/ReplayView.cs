using OsuParsers.Replays;
using OsuParsers.Replays.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsuAnalyzer.Drawables
{
    public class ReplayView : Drawable
    {
        public Replay r;
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
                ans = Color.White;
            return Color.FromArgb(a, ans);
        }

        public bool draw(Graphics g, long time)
        {
            var frames = r.ReplayFrames;
            int idx = Util.LowerBound(frames, x =>
              x.Time >= time - preTime
            );
            //TODO proper following cursor circle
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

                if (delta <= mnDelt)
                {   
                    cf = b;
                    dn = lhold || rhold;
                    mnDelt = delta;
                    rr = (int)Math.Min(50, Math.Max(0, time - lt));
                    pp = new Pen(getCol(ldb, rdb, alpha), 2);
                }

                g.DrawLine(new Pen(getCol(lhold, rhold, alpha), 3), a.X , a.Y , b.X  , b.Y);
                idx++;
            }
            if (cf != null)
            {
                if(dn)
                g.DrawEllipse(pp, cf.X - rr, cf.Y - rr, rr * 2, rr * 2);
            }
            return true;
        }
    }
}
