using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Gameplay
{

    public class MapPreview
    {
        public string FilePath { get; set; }

        // Section [SONG] - song info
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Length { get; set; } 

        // Section [MAP] - map specific info
        public difficulty Difficulty { get; set; }
        public string Path { get; set; }
        public int Offset { get; set; }
        public float Bpm { get; set; }

        public MapPreview(string filePath, string title, string artist, int length, difficulty difficulty,
                    string path, int offset, float bpm)
        {
            FilePath = filePath;
            Title = title;
            Artist = artist;
            Length = length;
            Difficulty = difficulty;
            Path = path;
            Offset = offset;
            Bpm = bpm;
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
            Console.Write(output);
        }
        public override string  ToString()
        {
            string output = "";

            output += "[SONG]\n";
            output += "Title : " + this.Title + "\n";
            output += "Artist : " + this.Artist + "\n";
            output += "Length : " + this.Length + "\n";
            output += "[MAP]\n";
            output += "Difficulty : " + this.Difficulty + "\n";
            output += "SongPath : " + this.Path + "\n";
            output += "Offset : " + this.Offset + "\n";
            output += "BPM : " + this.Bpm + "\n";

            return output;
        }
    }
}
