using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp
{
    public class TrackInfo
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public TimeSpan Duration { get; set; }
        public string UniqueId { get; set; }
        public DateOnly Date { get; set; }
    }
}
