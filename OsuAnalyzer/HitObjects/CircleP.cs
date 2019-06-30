using OsuParsers.Beatmaps.Objects;
using OsuParsers.Replays.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.HitObjects
{
    public class CircleP : HitObjectP<Circle>
    {
        private float cw, aw;

        public CircleP(Circle obj, MapAnalysisContext mw) : base(obj, mw)
        {
            cw = mw.th.circleBodyWidth;
            aw = mw.th.approachCircleWidth;
        }

        public override void draw(Graphics g, long time)
        {
            base.draw(g, time);
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

        public override Judgement judge(ReplayFrame cf, bool keyDown, bool hit, TimingPoint currentTiming)
        {
            if (hit)//check only if player taps
            {
                var dx = obj.Position.X - cf.X;
                var dy = obj.Position.Y - cf.Y;
                //check only if mouse is on circle
                if (dy * dy + dx * dx <= mw.radius*mw.radius)
                {
                    var delta = Math.Abs(cf.Time - obj.StartTime);
                    if (delta <= mw.hit300)
                        return new Judgement.Good();
                    else if (delta <= mw.hit100)
                        return new Judgement.Ok.Timing100();
                    else if (delta <= mw.hit50)
                        return new Judgement.Ok.Timing50();
                    else if(delta <= mw.hit0)
                        return new Judgement.Bad.Miss();
                }
            }
            return null;
        }
    }
}
