using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Exhibeat.Gameplay;
using System.Text.RegularExpressions;

namespace Exhibeat.Parser
{
    public class EXParser
    {
        private List<Note> _timeMap;
        private Map _map;
        public string[] _labels = { "[SONG]", "[MAP]", "[NOTES]" };

        public Map parse(string path)
        {
            string buffer = null;

            try
            {
                string[] spitedBuffer = null;
                buffer = System.Text.Encoding.UTF8.GetString(parser(path));

                _timeMap = new List<Note>();
                _map = new Map(_timeMap);
                spitedBuffer = buffer.Split('\n');
                spitedBuffer = cleanStringArray(spitedBuffer);
                if (checkLabelExistence(spitedBuffer))
                {
                    foreach (string label in _labels)
                    {
                        string output = getSongInfo(spitedBuffer, label);
                        setSongInfo(label, output);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                throw e;

            }
            return _map;
        }
        private void setSongInfo(string label, string info)
        {
            switch (label)
            {
                case "[SONG]":
                    setSongLabel(info);
                    break;
                case "[MAP]":
                    setMapLabel(info);
                    break;
                case "[NOTES]":
                    setNotesLabel(info);
                    break;
            }
        }
        private string[] removeLabel(string[] labelList, string[] infos)
        {
            foreach (string element in labelList)
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    infos[i] = infos[i].Replace(element, "");
                }
            }
            return infos;
        }
        private void setSongLabel(string info)
        {
            string[] elementToRemove = { "Title ", "Artist ", "Lenght " };
            string[] infos = info.Split('#');
            // verification sur la taille du tabeau == 3 param

            infos = removeLabel(elementToRemove, infos);
            _map.Title = infos[1];
            _map.Artist = infos[2];
            _map.Length = int.Parse(infos[3]);
        }
        private static difficulty getDifficulty(string diff)
        {
            string[] enums = Enum.GetNames(typeof(difficulty));
            foreach (string difficulty in enums)
            {
                if (difficulty.CompareTo(diff) == 0)
                {
                    return (difficulty)Enum.Parse(typeof(difficulty), diff);
                }
            }
            return 0;
        }
        private void setMapLabel(string info)
        {
            string[] elementToRemove = { "Difficulty ", "MP3 ", "Offset ", "BPM " };
            string[] infos = info.Split('#');

            infos = removeLabel(elementToRemove, infos);
            _map.Difficulty = getDifficulty(infos[1]);
            _map.Path = infos[2];
            _map.Offset = int.Parse(infos[3]);
            _map.Bpm = float.Parse(infos[4]);
        }

        private void setNotesLabel(string info)
        {
            string[] infos = info.Split('\n');
            int nbNotes = infos.Length / 5;

            for (int u = 0; u < infos.Length; u++)
            {
                if (infos[u].Length >= 2)
                {
                    Note note = new Note();
                    string[] nbList = infos[u].Split(',');
                    for (int i = 0; i < nbList.Length; i++)
                    {
                        if (nbList[i].CompareTo("") != 0)
                        {
                            if (i == 0)
                                note.Offset = int.Parse(nbList[i]);
                            else if (i == 1)
                                note.Length = int.Parse(nbList[i]);
                            else if (i == 2)
                            {
                                note.Button = int.Parse(nbList[i]);
                                _timeMap.Add(note);
                            }
                            else
                            {
                                Note note2 = new Note();
                                note2.Offset = note.Offset;
                                note2.Length = note.Length;
                                note2.Button = int.Parse(nbList[i]);
                                _timeMap.Add(note2);
                            }
                        }
                    }
                    _timeMap.Add(note);
                }
            }
        }

        private string[] cleanStringArray(string[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = buffer[i].Replace("\r", "");
            }
            return buffer;
        }

        private bool checkLabelExistence(string[] buffer)
        {
            foreach (string label in _labels)
            {
                if (!buffer.Contains(label))
                    return false;
            }
            return true;
        }

        private string getSongInfo(string[] buffer, string label)
        {
            int pos = System.Array.IndexOf(buffer, label);
            string output = "";

            pos += 1;
            while (pos < buffer.Length && !_labels.Contains(buffer[pos]))
            {
                if (label.CompareTo("[NOTES]") == 0)
                    buffer[pos] += "\n";
                output += buffer[pos];
                pos++;
            }
            return output;
        }
        public static MapPreview getSongInfo(string path)
        {
            StreamReader myFile = new StreamReader(path);
            string myString = myFile.ReadToEnd();
            myFile.Close();

            string title = extractValueFromString("#Title ", myString);
            string artist = extractValueFromString("#Artist ", myString);
            string lenght = extractValueFromString("#Lenght ", myString);

            string difficulty = extractValueFromString("#Difficulty ", myString);
            string mp3Path = extractValueFromString("#MP3 ", myString);
            string offset = extractValueFromString("#Offset ", myString);
            string bpm = extractValueFromString("#BPM ", myString);

            return (new MapPreview(path, title, artist, int.Parse(lenght), getDifficulty(difficulty), mp3Path, int.Parse(offset), int.Parse(bpm)));
        }
        public byte[] parser(String path)
        {
            FileStream fileStream = null;
            byte[] buffer = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Open);
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;

                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;
            }
            catch (IOException e)
            {
                throw e;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return buffer;
        }
        public static string extractValueFromString(string key, string str)
        {
            //#Title ([a-zA-Z0-9\\-\\.\\'\\(\\) ]+)
            string pattern = key + "([a-zA-Z0-9\\-\\.\\'\\(\\) ]+)";
            Regex myRegex = new Regex(pattern);
            Match m = myRegex.Match(str);
            string value = null;
            if (m.Success)
            {
                if (m.Groups.Count >= 1)
                {
                    value = m.Groups[1].Value;
                }
            }
            return value;
        }
        /*public static MapPreview getSongInfo(string path)
        {
            StreamReader myFile = new StreamReader(path);
            string myString = myFile.ReadToEnd();
            myFile.Close();

            string title = extractValueFromString("#Title ", myString);
            string artist = extractValueFromString("#Artist ", myString);
            string lenght = extractValueFromString("#Lenght ", myString);

            string difficulty = extractValueFromString("#Difficulty ", myString);
            string mp3Path = extractValueFromString("#MP3 ", myString);
            string offset = extractValueFromString("#Offset ", myString);
            string bpm = extractValueFromString("#BPM ", myString);

            return (new MapPreview(path, title, artist, int.Parse(lenght), getDifficulty(difficulty), mp3Path, int.Parse(offset), int.Parse(bpm)));
        }*/
    }
}