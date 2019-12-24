using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class TalkModel
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int Level { get; set; }
        public SpeakerModel Speaker { get; set; }
    }
}
