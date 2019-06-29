using OsuAnalyzer.Drawables;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Database.Objects;
using OsuParsers.Replays;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsuAnalyzer
{
    public interface Drawable
    {
        bool draw(Graphics g, long time);
    }

    public class CoolPanel : Panel
    {
        public CoolPanel()
        {
            DoubleBuffered = true;
        }

        Pen pn = new Pen(Color.White, 3);
        float wToH = 4f / 3f;
        Brush clr = Brushes.Black;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            //osupixel converted code here:

            float limW = ClientSize.Width;
            float limH = ClientSize.Height;
            float nw = Math.Min(limW, limH * wToH);
            float nh = nw / wToH;
            g.ScaleTransform(nw / 512, nh / 384);

            g.DrawRectangle(pn, 0, 0, 512, 384);

            if (ct != -1)
            {
                draw(g);
            }
        }
        
        private MapWrap mw;
        private ReplayView rv;

        private void drawHit(Graphics g, long time)
        {
            var objs = mw.map.HitObjects;
            int idx = Util.LowerBound(objs, x =>
                x.StartTime-mw.preempt > time
            )-1 ;
            idx = Math.Max(idx,0);
          
            while (idx>=0 )
            {
                bool dead = false;
                var obj = objs[idx];
                if (obj is Circle)
                {
                    CircleView cv = new CircleView
                    {
                        mw = mw,
                        obj = obj as Circle,
                    };

                    dead|=cv.draw(g, time);
                }
                if (dead) break; 
                idx--;
            }
        }

        private void draw(Graphics g)
        {
            if (mw == null) return;

            drawHit(g, ct);

            rv.draw(g, ct);
        }

        public void loadReplay(MapWrap mw)
        {
            this.mw = mw;
            rv = new ReplayView {
                r = mw.rp,
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
