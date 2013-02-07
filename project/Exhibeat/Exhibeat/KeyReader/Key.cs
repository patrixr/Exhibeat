using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExhiBeat.KeyReader
{
    public enum keyType
    {
        pressed,
        realeased,
        none
    };

    class Key
    {
        public int pos;
        public keyType type;
    }
}
