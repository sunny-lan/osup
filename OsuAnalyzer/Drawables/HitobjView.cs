using OsuParsers.Beatmaps.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuAnalyzer.Drawables
{
    public abstract class HitobjView<T>:Drawable where T:HitObject
    {
        protected T obj;
        protected MapWrap mw;
        public HitobjView(T obj, MapWrap mw)
        {
            this.obj = obj;
            this.mw = mw;
        }

        public double getAlpha(long time)
        {

            double st = obj.StartTime - mw.preempt;
            double a = (time - st) / mw.fade_in;
            double fadeOut =1- (time - obj.EndTime)/mw.hit50;
            return Math.Min(Math.Max(Math.Min(a, fadeOut), 0), 1);
        }

        public virtual bool isOver(long time)
        {
            return time > obj.EndTime + mw.hit50;
        }

        public int getAlphaInt(long time)
        {
            return (int)Math.Round(getAlpha(time)*255);
        }

        public abstract void draw(Graphics g, long time);
    }
}
