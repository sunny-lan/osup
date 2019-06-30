using OsuParsers.Beatmaps.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.Drawables
{
    public abstract class HitobjView<T>:Drawable where T:HitObject
    {
        protected T obj;
        protected MapAnalysisContext mw;
        public HitobjView(T obj, MapAnalysisContext mw)
        {
            this.obj = obj;
            this.mw = mw;
        }

        public double getAlpha(long time)
        {
            double st = obj.StartTime - mw.preempt;
            double a = (time - st) / mw.fade_in;
            var dt = deathTime();
            double fadeOut =1- (time-obj.EndTime)/Math.Max(0.1,dt-obj.EndTime);
            //Console.WriteLine($"k={this.GetType()} t={time} f={fadeOut}");
            return Math.Min(Math.Max(Math.Min(a, fadeOut), 0), 1);
        }

        public virtual long deathTime()
        {
            return (long)(obj.EndTime + mw.hit50);
        }

        public int getAlphaInt(long time)
        {
            return (int)Math.Round(getAlpha(time)*255);
        }

        public abstract void draw(Graphics g, long time);
    }
}
