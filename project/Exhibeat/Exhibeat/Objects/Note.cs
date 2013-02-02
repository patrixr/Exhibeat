using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Objects
{
    class Note
    {
        private int _offset { get; set; }
        private int _lenght { get; set; }
        private int _button { get; set; }

        public Note()
        {
        }

        public Note(int offset, int lenght, int note)
        {
            this._offset = offset;
            this._lenght = lenght;
            this._button = note;
        }
    }
}
