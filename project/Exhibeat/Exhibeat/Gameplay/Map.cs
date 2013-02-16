using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Gameplay
{

    public enum difficulty
    {
        EASY,
        MEDIUM,
        HARD,
        INSANE
    }

    public class Map
    {
        // Section [NOTE] - note list
        private List<Note> _timeMap;

        // Section [SONG] - song info
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Length { get; set; }

        // Section [MAP] - map specific info
        public difficulty Difficulty { get; set; }
        public string Path { get; set; }
        public int Offset { get; set; }
        public float Bpm { get; set; }


        public Map(List<Note> TimeMap)
        {
            _timeMap = TimeMap;
        }

        public Map(List<Note> TimeMap, string title, string artist, int length, difficulty difficulty,
                    string path, int offset, float bpm)
        {
            _timeMap = TimeMap;
            Title = title;
            Artist = artist;
            Length = length;
            Difficulty = difficulty;
            Path = path;
            Offset = offset;
            Bpm = bpm;
        }

        public List<Note> GetNotesFromIndex(int index, int nb)
        {
            List<Note> result = new List<Note>();
            if (index < _timeMap.Count)
            {
                for (int idx = index; idx < index + nb && idx < _timeMap.Count; idx++)
                {
                    result.Add(_timeMap[idx]);
                }
            }
            return result.Count > 0 ? result : null;
        }
        public List<Note> GetNotesBetweenOffsets(int offset1, int offset2)
        {
            List<Note> result = new List<Note>();
            foreach (Note currentNote in _timeMap)
            {
                if (currentNote.Offset >= offset1 && currentNote.Offset <= offset2)
                    result.Add(currentNote);
            }
            return result.Count > 0 ? result : null;
        }
        public void showClassData()
        {
            string output = "";

            output += "[SONG]\n";
            output += this.Title + "\n";
            output += this.Artist + "\n";
            output += this.Length + "\n";
            output += "[MAP]\n";
            output += this.Difficulty + "\n";
            output += this.Path + "\n";
            output += this.Offset + "\n";
            output += this.Bpm + "\n";
            output += "[NOTES]\n";

            foreach (Note note in this._timeMap)
            {
                output += note.Offset + ", ";
                output += note.Length + ", ";
                output += note.Button + ", ";
                output += "\n";
            }

            Console.Write(output);
        }
    }
}
