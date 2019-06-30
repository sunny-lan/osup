using OsuParsers.Beatmaps.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.Drawables
{
    public class SpinnerView:HitobjView<Spinner>
    {
        private Pen pn = new Pen(Color.White, 5);

        public SpinnerView(Spinner obj, MapAnalysisContext mw) : base(obj, mw)
        {
        }

        public override void draw(Graphics g, long time)
        {
            float cx = 512 / 2, cy=384/2;
            long a = Math.Max(0,obj.EndTime - time), b = obj.EndTime - obj.StartTime;
            float r = (384/2 - 20)*a/b;
            g.DrawEllipse(pn, cx - r, cy - r, r * 2, r * 2);
        }

        public override long deathTime()
        {
            return obj.EndTime;
        }
    }
}
