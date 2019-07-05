using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuAnalyzer
{
    
    public abstract class Judgement
    {
        public long time;
        public int bmIdx;
        /// <summary>
        /// Perfect
        /// </summary>
        public class Good : Judgement{}

        /// <summary>
        /// Not perfect but doesn't break combo
        /// </summary>
        public class Ok : Judgement{
            public class Timing50 :Ok{ }
            public class Timing100 : Ok { }
        }

        /// <summary>
        /// Breaks combo
        /// </summary>
        public class Bad : Judgement
        {
            public class NoClick : Bad{ }
            public class Miss : Bad { }
            public class SliderBreak : Bad { }
        }
    }
}
