using osu.Framework.MathUtils;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Enums;
using OsuParsers.Replays.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;

namespace OsuAnalyzer.HitObjects
{
    public class SliderP : HitObjectP<Slider>
    {
        private List<Vector2> approx;
        private CircleP initCircle;

        public SliderP(Slider obj, MapAnalysisContext mw):base(obj, mw)
        {
            obj.EndTime = obj.StartTime + (obj.EndTime - obj.StartTime) * obj.Repeats;
            initCircle = new CircleP(new Circle(
                obj.Position,
                obj.StartTime, obj.StartTime,
                obj.HitSound, null, obj.IsNewCombo, obj.ComboOffset
            ), mw);

            var kek = new List<Vector2>();
            var value= obj;
            var sld = value.SliderPoints.Select(x => new Vector2(x.X, x.Y)).ToList();
            sld.Insert(0, new Vector2(obj.Position.X, obj.Position.Y));
            
            if (value.CurveType == CurveType.Bezier)
                approx = PathApproximator.ApproximateBezier(sld);
            if (value.CurveType == CurveType.Catmull)
                approx = PathApproximator.ApproximateCatmull(sld);
            if (value.CurveType == CurveType.Linear)
                approx = PathApproximator.ApproximateLinear(sld);
            if (value.CurveType == CurveType.PerfectCurve)
                approx = PathApproximator.ApproximateCircularArc(sld);
            Debug.Assert(approx.Count > 0, "Approximation of slider failed");
        }

        private const float fc = 5;
        private static Pen fp = new Pen(Color.White, fc);
        

        public override void draw(Graphics g, long time)
        {
            var pn = new Pen(Color.FromArgb(getAlphaInt(time)/2, Color.Gray), (float)mw.radius * 2);
            pn.StartCap = LineCap.Round;
            pn.EndCap = LineCap.Round; 
            var pn2 = new Pen(Color.FromArgb(getAlphaInt(time), Color.Black), (float)mw.radius * 2-10);
            pn2.StartCap = LineCap.Round;
            pn2.EndCap = LineCap.Round;
            for (int i = 1; i < approx.Count; i++)
            {
                var a = approx[i - 1];
                var b = approx[i];
                g.DrawLine(pn, a.X, a.Y, b.X, b.Y);
            }
            for (int i = 1; i < approx.Count; i++)
            {
                var a = approx[i - 1];
                var b = approx[i];
                g.DrawLine(pn2, a.X, a.Y, b.X, b.Y);
            }
            var loli = approx.Count-1  ;
            double progress = (time - obj.StartTime) / (double)(obj.EndTime - obj.StartTime);
            if(progress>=0 && progress < 1) {
                double boost = progress * loli * obj.Repeats;
                int ree = (int)boost;
                int cnt = ree / loli;
                int bee=ree- cnt * loli;
                Vector2 a, b;
                if (cnt % 2 == 0)
                {
                    a = approx[bee];
                    b = approx[bee + 1];
                }
                else
                {
                    a = approx[approx.Count- bee - 1];
                    b = approx[approx.Count- bee - 2];
                }

                float rr = 2 * (float)mw.radius - fc / 2;
                double f =boost- ree;
                float x = (float)(b.X * f + a.X * (1 - f));
                float y = (float)(b.Y * f + a.Y * (1 - f));

                g.DrawEllipse(fp, x - rr, y - rr, rr * 2, rr * 2);
               
            }

            initCircle.draw(g, time);
            base.draw(g, time);
        }

        public override Judgement judge(ReplayFrame cf, bool keyDown, bool hit, TimingPoint currentTiming)
        {
            return new Judgement.Good();
        }
    }
}
