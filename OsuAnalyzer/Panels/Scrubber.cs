using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsuAnalyzer.Panels
{
    public delegate void SeekEvt(long time);

    public class Scrubber : Panel
    {
        private float cursorX=0;
        public event SeekEvt onSeek;

        public MapWrap mw;
        private long bmL;

        public void loadReplay(MapWrap mw)
        {
            this.mw = mw;
            cursorX = 0;
            bmL = mw.rp.ReplayFrames.Last().Time+1000;
            Invalidate();
            onSeek?.Invoke(0);
        }

        public Scrubber()
        {
            DoubleBuffered = true;
        }

        private static Pen cursor=new Pen(Color.White, 3);

        Font drawFont = new Font("Arial", 16);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (mw == null)
            {
                g.DrawString("No replay loaded", drawFont, Brushes.DarkRed, 0, 0);
            }
            else
            {
                g.Clear(Color.Black);

                g.DrawLine(cursor, cursorX, 0, cursorX, Height);
            }
        }

        private bool mouseDown=false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown && mw!=null)
            {
                cursorX = Math.Max(0,Math.Min(e.X, Width));
                Invalidate();
                onSeek?.Invoke((long)(bmL*cursorX/Width));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
