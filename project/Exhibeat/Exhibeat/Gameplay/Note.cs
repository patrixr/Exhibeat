using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Gameplay
{
    public class Note
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public int Button { get; set; }

        public Note()
        {
        }

        public Note(int offset, int length, int note)
        {
            this.Offset = offset;
            this.Length = length;
            this.Button = note;
        }
    }
}
