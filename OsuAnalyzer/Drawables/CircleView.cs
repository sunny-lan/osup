using OsuParsers.Beatmaps.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuAnalyzer.Drawables
{
    public class CircleView : HitobjView<Circle>
    {
        private const float cw=10, aw=2;

        public CircleView(Circle obj, MapWrap mw) : base(obj, mw)
        {
        }

        public override void draw(Graphics g, long time)
        {
            int a = getAlphaInt(time);

            float rr = mw.radius-cw/2;
            Color c = Color.White;
            var delta = Math.Abs(time - obj.StartTime);
            //if (delta <= mw.hit300)
            //    c = Color.Blue;
            //else if (delta <= mw.hit100)
            //    c = Color.Green;
            //else if (delta <= mw.hit50)
            //    c = Color.Yellow;
            //else
            //    c = Color.Red;
            Pen cp = new Pen(Color.FromArgb(a, c), cw);

            g.DrawEllipse(cp, obj.Position.X-rr, obj.Position.Y-rr,rr*2, rr*2);

            //approach circle
            Pen ap = new Pen(Color.FromArgb(a, Color.White), aw);
            rr = (float)(mw.radius + mw.radius*2 * Math.Max(0, obj.StartTime-time) / mw.preempt - aw / 2);
            g.DrawEllipse(ap, obj.Position.X - rr, obj.Position.Y - rr, rr * 2, rr * 2);
        }
    }
}
