using OsuParsers;
using OsuParsers.Database;
using OsuParsers.Database.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OsuAnalyzer
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            loadOsuDb();
            reloadReplays();
            scrubber1.onSeek += coolPanel.seek;
        }

        string osuRoot;

        OsuDatabase osuDb;
        Dictionary<string, DbBeatmap> mapsByHash;

        ScoresDatabase scoreDb;

        private void loadOsuDb()
        {
            osuRoot = File.ReadAllText(@"osudir.txt");

            osuDb = Parser.ParseOsuDatabase(Path.Combine(osuRoot, @"osu!.db"));
            mapsByHash = new Dictionary<string, DbBeatmap>();
            foreach(var map in osuDb.Beatmaps)
            {
                mapsByHash.Add(map.MD5Hash, map);
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            reloadReplays();
        }

        private void reloadReplays()
        {
            loadReplay.Enabled = false;
            replaySelect.Items.Clear();
            scoreDb = Parser.ParseScoresDatabase(Path.Combine(osuRoot, "scores.db"));
            scoreDb.Scores.Sort((a, b) =>
            {
                var aS = a.Item2;
                var bS =b.Item2;
                if (aS.Count == 0) return 1;
                if (bS.Count == 0) return -1;
                aS.Sort((x, y) => DateTime.Compare(y.ScoreTimestamp, x.ScoreTimestamp));
                bS.Sort((x, y) => DateTime.Compare(y.ScoreTimestamp, x.ScoreTimestamp));
                
                return DateTime.Compare(bS[0].ScoreTimestamp, aS[0].ScoreTimestamp);
            });
            mapSelect.Items.Clear();
            foreach(var tp in scoreDb.Scores)
            {
                var map = mapsByHash[tp.Item1];
                mapSelect.Items.Add(map.FileName);
            }
        }

        private void mapSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tp = scoreDb.Scores[mapSelect.SelectedIndex];
            var map = mapsByHash[tp.Item1];
            showMapInfo(map, tp.Item2);
            replaySelect.SetSelected(0, true);
        }

        string nl = Environment.NewLine;

        private void showMapInfo(DbBeatmap map, List<Score> scores)
        {
            mapInfo.Text = $"MapInfo{nl}" +
               $"Song title: {map.Title}{nl}" +
               $"Difficulty: {map.Difficulty}{nl}" +
               $"Hash: {map.MD5Hash}{nl}";

            replayInfo.Text = "";

            replaySelect.Items.Clear();
            foreach (var score in scores)
            {
                replaySelect.Items.Add($"x{score.Combo} [{score.ScoreTimestamp.ToLongDateString()} {score.ScoreTimestamp.ToLongTimeString()}]");
            }
        }

        private void replaySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadReplay.Enabled = true;
            showReplayInfo(scoreDb.Scores[mapSelect.SelectedIndex].Item2[replaySelect.SelectedIndex]);
        }

        private void showReplayInfo(Score score)
        {
            replayInfo.Text = $"Replay info{nl}" +
                $"ID: {score.ScoreId}{nl}" +
                $"Hash: {score.ReplayMD5Hash}{nl}" +
                $"Date achieved: {score.ScoreTimestamp} (UTC {score.ScoreTimestamp.ToFileTimeUtc()}){nl}";
        }
        
        private MapAnalysisContext mw;

        private void loadReplay_Click(object sender, EventArgs e)
        {
            loadCurrentReplay();
        }

        private void loadCurrentReplay()
        {
            loadReplay.Enabled = false;
            var tp = scoreDb.Scores[mapSelect.SelectedIndex];
            var map = mapsByHash[tp.Item1];
            var score = tp.Item2[replaySelect.SelectedIndex];
            mw = new MapAnalysisContext(map, score, osuRoot);
            coolPanel.loadReplay(mw);
            scrubber1.loadReplay(mw);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Space)
                scrubber1.Playing = !scrubber1.Playing;
            else if (keyData == Keys.Left)
                scrubber1.step(-1);
            else if (keyData == Keys.Right)
                scrubber1.step(1);
            else
                return base.ProcessCmdKey(ref msg, keyData);
            return true;    // indicate that you handled this keystroke
        }
    }
}
