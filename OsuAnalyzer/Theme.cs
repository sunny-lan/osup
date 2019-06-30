using System.Drawing;

namespace OsuAnalyzer.Drawing
{
    public class Theme
    {
        public Color miss = Color.Red;
        public Color hit50 = Color.Yellow;
        public Color hit100 = Color.Green;
        public Color hit300 = Color.Blue;

        public Color circleOutline = Color.White;
        public Color circleBody = Color.Gray;
        public float circleBodyWidth = 10, circleOutlineWidth = 3;
        public float approachCircleWidth = 5;

        public Font debugFont = new Font("Arial", 16);

        //TODO move everything into theme
    }
}
