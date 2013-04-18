using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExhiBeat.KeyReader
{
    public interface IKeyReader
    {
        Key getNextKeyEvent();
    }
}
