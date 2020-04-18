using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbsongPro.Models
{
    public class SongPart
    {

        public SongPart()
        {

        }
        public SongPart(string link, string from, string to)
        {
            Link = link;
            From = from;
            To = to;
        }

        public string Link { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
