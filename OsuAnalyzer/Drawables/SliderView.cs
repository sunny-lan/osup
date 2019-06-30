using osu.Framework.MathUtils;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OsuAnalyzer.Drawables
{
    public class SliderView : HitobjView<Slider>
    {
        private List<Vector2> approx;
        private CircleView initCircle;

        public SliderView(Slider obj, MapWrap mw):base(obj, mw)
        {

            initCircle = new CircleView(new Circle(
                obj.Position,
                obj.StartTime, obj.EndTime,
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
        }

        private  const float fc = 5;
        private static Pen fp = new Pen(Color.White, fc);
        

        public override void draw(Graphics g, long time)
        {
            var pn = new Pen(Color.FromArgb(getAlphaInt(time), Color.Gray), mw.radius * 2);
            pn.StartCap = LineCap.Round;
            pn.EndCap = LineCap.Round;
            var pn2 = new Pen(Color.Black, mw.radius * 2-10);
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
            double progress = (time - obj.StartTime) / (double)(obj.EndTime - obj.StartTime);
            if(progress>=0 && progress < 1) { 
                int ree = (int)(progress * (approx.Count-1));
                var a = approx[ree];
                float rr = 2 * mw.radius - fc / 2;
                if (obj.CurveType == CurveType.Linear)
                {
                    var b = approx[ree + 1];
                    double sub = ree / (approx.Count - 1);
                    double f = progress - sub;
                    float x = (float)(b.X * f + a.X * (1 - f));
                    float y = (float)(b.Y * f + a.Y * (1 - f));

                    g.DrawEllipse(fp, x - rr, y - rr, rr * 2, rr * 2);
                }
                else
                {
                    g.DrawEllipse(fp, a.X - rr, a.Y - rr, rr * 2, rr * 2);
                }
            }

            initCircle.draw(g, time);
        }

    }
}
