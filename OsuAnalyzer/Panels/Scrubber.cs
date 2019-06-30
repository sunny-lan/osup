using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsuAnalyzer.Panels
{
    public delegate void SeekEvt(long time);

    public class Scrubber : Panel
    {
        public MapWrap mw;

        private long time;
        public event SeekEvt onSeek;

        private WaveOutEvent audioOut;
        private AudioFileReader audio;
        private long audioLen;
        private Timer playTimer;

        private DateTime lstPlay;
        private bool _playing;
        private long lstTime;
        public bool Playing
        {
            get { return _playing; }
            set
            {
                if (mw==null) return;
                if (value == _playing) return;
                _playing = value;
                if (value)
                {
                    lstPlay = DateTime.Now;
                    lstTime = time;
                    playTimer.Start();
                    audioOut.Play();
                }
                else
                {
                    audioOut.Stop();
                    playTimer.Stop();
                }
            }
        }

        private void loadAudio()
        {
            Playing = false;
            _playing = false;

            var pth = Path.Combine(mw.osuRoot, "Songs", mw.dmp.FolderName, mw.dmp.AudioFileName);
            audio = new AudioFileReader(pth);
            audioLen = mw.rp.ReplayFrames.Last().Time + 1000;
            audioOut = new WaveOutEvent();
            audioOut.Init(audio);

            playTimer = new Timer
            {
                Interval = 15,
            };
            playTimer.Tick += PlayTimer_Tick;
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            time = (long)(DateTime.Now - lstPlay).TotalMilliseconds+lstTime;
            onSeek?.Invoke(time);
            Invalidate();
        }

        public void loadReplay(MapWrap mw)
        {
            this.mw = mw;
            time = 0;

            loadAudio();

            Invalidate();
        }

        public Scrubber()
        {
            DoubleBuffered = true;
        }

        private static Pen cursor = new Pen(Color.White, 3);

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

                float cursorX = time *Width/ audioLen;
                g.DrawLine(cursor, cursorX, 0, cursorX, Height);
            }
        }

        private bool mouseDown = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
            Playing = false;
        }

        private void updateTime(MouseEventArgs e)
        {
            if (mouseDown && mw != null)
            {
                float cursorX = Math.Max(0, Math.Min(e.X, Width));
                time = (long)(cursorX * audioLen / Width);
                onSeek?.Invoke(time);
                Invalidate();
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            updateTime(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            updateTime(e);
            audio.CurrentTime = TimeSpan.FromMilliseconds(time);
            mouseDown = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
