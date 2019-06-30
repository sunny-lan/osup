using OsuParsers.Beatmaps.Objects;
using OsuParsers.Replays.Objects;
using System;
using System.Drawing;

namespace OsuAnalyzer.HitObjects
{
    public abstract class HitObjectP : Drawable
    {
        protected MapAnalysisContext mw;
        public Judgement judgement;

        /// <summary>
        /// this tells the editor when the object should stop being shown
        /// </summary>
        /// <returns>the time when this object is considered missed</returns>
        public abstract long deathTime();

        /// <summary>
        /// guarenteed to be called in increasing order of replay frames
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public abstract Judgement judge(
            ReplayFrame cf,
            bool keyDown,
            bool hit,
            TimingPoint currentTiming
        );

        //inherited methods
        
        public abstract void draw(Graphics g, long time);
    }

    public abstract class HitObjectP<T> : HitObjectP where T : HitObject
    {
        protected T obj;
        public HitObjectP(T obj, MapAnalysisContext mw)
        {
            this.obj = obj;
            this.mw = mw;
        }

        public double getAlpha(long time)
        {
            double st = obj.StartTime - mw.preempt;
            double a = (time - st) / mw.fade_in;
            var dt = deathTime();
            double fadeOut = 1 - (time - obj.EndTime) / Math.Max(0.1, dt - obj.EndTime);
            //Console.WriteLine($"k={this.GetType()} t={time} f={fadeOut}");
            return Math.Min(Math.Max(Math.Min(a, fadeOut), 0), 1);
        }

        public override long deathTime()
        {
            return (long)(obj.EndTime + mw.hit0);
        }

        public int getAlphaInt(long time)
        {
            return (int)Math.Round(getAlpha(time) * 255);
        }

        public override void draw(Graphics g, long time)
        {
            if(judgement!=null )
            g.DrawString(judgement.GetType().Name, mw.th.debugFont, new SolidBrush(Color.FromArgb(
                getAlphaInt(time),
                Color.White
                )), obj.Position);
        }


    }
}
