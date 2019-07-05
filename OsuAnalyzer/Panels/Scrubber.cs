using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OsuAnalyzer.Panels
{
    public delegate void SeekEvt(long time);

    public class Scrubber : Panel
    {
        public MapAnalysisContext mw;

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
                    audio.CurrentTime = TimeSpan.FromMilliseconds(time);
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
            time = (long)(DateTime.Now - lstPlay).TotalMilliseconds + lstTime;
            onSeek?.Invoke(time);
            Invalidate();
        }

        public void loadReplay(MapAnalysisContext mw)
        {
            this.mw = mw;

            loadAudio();

            seek(0);
        }

        public Scrubber()
        {
            DoubleBuffered = true;
        }

        Font drawFont = new Font("Arial", 16);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            if (mw == null)
            {
                g.DrawString("No replay loaded",drawFont, Brushes.DarkRed, 0, 0);
            }
            else
            {
                float cursorX;

                foreach (var ok in mw.okJudgements)
                {
                    cursorX = timeToCursor(ok.time);
                    g.DrawLine(mw.th.okIndicator, cursorX, 0, cursorX, Height);
                }

                foreach (var bad in mw.badJudgements)
                {
                    cursorX = timeToCursor(bad.time);
                    var id = mw.th.badIndicator;
                    if (bad is Judgement.Bad.SliderBreak)
                        id = mw.th.breakIndicator;
                    g.DrawLine(id, cursorX, 0, cursorX, Height);
                }

                cursorX = timeToCursor(time);
                g.DrawLine(mw.th.cursor, cursorX, 0, cursorX, Height);
            }
        }

        private bool mouseDown = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
        }

        private void updateTime(MouseEventArgs e)
        {
            if (mouseDown)
            {
                seek(cursorToTime(e.X));
            }
        }

        public void seek(long time)
        {
            if (mw == null) return;
            Playing = false;
            this.time = time;
            onSeek?.Invoke(time);
            Invalidate();
        }

        private long cursorToTime(float cursorX)
        {
            cursorX = Math.Max(0, Math.Min(cursorX, Width));
            return (long)(cursorX * audioLen / Width);
        }

        private float timeToCursor(long time)
        {
            return time * Width / (float)audioLen;
        }

        long zoom = 10;

        public void step(int amt)
        {
            if (mw == null) return;
            seek(time + amt*zoom);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0) step(-1);
            else if (e.Delta < 0) step(1);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            updateTime(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            updateTime(e);
            mouseDown = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
