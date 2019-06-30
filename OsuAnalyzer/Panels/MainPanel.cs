using OsuAnalyzer.Drawables;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OsuAnalyzer
{
    public interface Drawable
    {
        void draw(Graphics g, long time);
        long deathTime();
    }

    public class MainPanel : Panel
    {
        public MainPanel()
        {
            DoubleBuffered = true;
        }

        Pen pn = new Pen(Color.FromArgb(100,Color.White), 3);
        float wToH = 4f / 3f;
        Brush clr = Brushes.Black;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (mw == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            float limW = ClientSize.Width;
            float limH = ClientSize.Height;
            float nw = Math.Min(limW, limH * wToH);
            float nh = nw / wToH;

            g.ScaleTransform(0.9f*nw / 512, 0.9f*nh / 384);
            g.TranslateTransform(0.05f * 512, 0.05f * 384);
            g.DrawRectangle(pn, 0, 0, 512, 384);

            if (ct != -1)
            {
                draw(g);
            }
        }
        
        private MapAnalysisContext mw;
        private ReplayView rv;

        private void drawHit(Graphics g, long time)
        {
            var objs = mw.mp.HitObjects;
            int idx = Util.LowerBound(objs, x =>
                x.StartTime-mw.preempt > time
            )-1 ;
            idx = Math.Max(idx,0);
            while (idx>=0 && mw.objs[idx].deathTime()>time)
            {
                mw.objs[idx].draw(g, time);
                idx--;
            }
        }

        private void draw(Graphics g)
        {
            if (mw == null) return;

            drawHit(g, ct);

            rv.draw(g, ct);
        }

        public void loadReplay(MapAnalysisContext mw)
        {
            this.mw = mw;
            rv = new ReplayView {
                mw = mw,
            };
        }

        long ct=-1;

        public void seek(long time)
        {
            ct = time;
            Invalidate();
        }
    }
}
