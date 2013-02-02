using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Objects
{

enum difficulty
{
    EASY,
    MEDIUM,
    HARD,
    INSANE
}

    class Map
    {
        // Section [NOTE] - note list
        public List<Note> _timeMap { get; set; }

        // Section [SONG] - song info
        public string _titles { get; set; }
        public string _artist { get; set; }
        public int _lenght { get; set; }

        // Section [MAP] - map specific info
        public difficulty _difficulty { get; set; }
        public string _path { get; set; }
        public int _offset { get; set; }
        public float _bpm { get; set; }


        public Map()
        {
        }

        public List<Note> GetNotesFromIndex(int index, int nb)
        {
            List<Note> result = new List<Note>();

            try
            {
                while (nb > 0)
                {
                    if (index == _timeMap.Count - 1)
                        nb = 1;
                    else
                        result.Add(_timeMap[index]);
                    nb--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return result;
        }
        public void showClassData()
        {
            string output = "";

            output += "[SONG]\n";
            output += this._titles + "\n";
            output += this._artist + "\n";
            output += this._lenght + "\n";
            output += "[MAP]\n";
            output += this._difficulty + "\n";
            output += this._path + "\n";
            output += this._offset + "\n";
            output += this._bpm + "\n";
            output += "[NOTES]\n";

            foreach (Note note in this._timeMap)
            {
                output += note.getOffset() + ", ";
                output += note.getLenght() + ", ";
                output += note.getButtons() + ", ";
                output += "\n";
            }

            Console.Write(output);
        }
    }
}
