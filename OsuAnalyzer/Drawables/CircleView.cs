using OsuParsers.Beatmaps.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.Drawables
{
    public class CircleView : HitobjView<Circle>
    {
        private float cw, aw;

        public CircleView(Circle obj, MapAnalysisContext mw) : base(obj, mw)
        {
            cw = mw.th.circleBodyWidth;
            aw = mw.th.approachCircleWidth;
        }

        public override void draw(Graphics g, long time)
        {
            int a = getAlphaInt(time);

            float rr = mw.radius-cw/2;

            //draw body
            Pen cp = new Pen(Color.FromArgb(a, mw.th.circleBody), cw);
            g.DrawEllipse(cp, obj.Position.X-rr, obj.Position.Y-rr,rr*2, rr*2);

            //draw outline
            Pen cd = new Pen(Color.FromArgb(a, mw.th.circleOutline), 3);
            rr = mw.radius - mw.th.circleOutlineWidth / 2;
            g.DrawEllipse(cd, obj.Position.X - rr, obj.Position.Y - rr, rr * 2, rr * 2);

            //approach circle
            Color c = Color.White;
            var delta = Math.Abs(time - obj.StartTime);
            if (delta <= mw.hit300)
                c = mw.th.hit300;
            else if (delta <= mw.hit100)
                c = mw.th.hit100;
            else if (delta <= mw.hit50)
                c = mw.th.hit50;
            else
                c = mw.th.miss;

            Pen ap = new Pen(Color.FromArgb(a, c), aw);
            rr = (float)(mw.radius + mw.radius*2 * Math.Max(0, obj.StartTime-time) / mw.preempt - aw / 2);
            g.DrawEllipse(ap, obj.Position.X - rr, obj.Position.Y - rr, rr * 2, rr * 2);
        }
    }
}
