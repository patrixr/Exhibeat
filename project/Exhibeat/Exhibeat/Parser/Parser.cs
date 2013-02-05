using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Exhibeat.Gameplay;

namespace Exhibeat.Parser
{
    public class Parser
    {
        private List<Note> _timeMap;
        private Map _map;
        public string[] labels = { "[SONG]", "[MAP]", "[NOTES]" };

        public Map parse(string path)
        {
            _timeMap = new List<Note>();
            _map = new Map(_timeMap);
            string buffer = System.Text.Encoding.UTF8.GetString(parser(path));
            string[] spitedBuffer = buffer.Split('\n');
   
            spitedBuffer = cleanStringArray(spitedBuffer);
            if (checkLabelExistence(spitedBuffer))
            {
                foreach (string label in labels)
                {
                    string output = getSongInfo(spitedBuffer, label);
                    setSongInfo(label, output);
                }
            }
            return _map;
        }
        private void setSongInfo(string label, string info)
        {
            switch (label)
            {
                case "[SONG]" :
                    setSongLabel(info);
                    break;
                case "[MAP]" :
                    setMapLabel(info);
                    break;
                case "[NOTES]" :
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
        private difficulty getDifficulty(string diff)
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

            for (int u = 0  ; u < infos.Length; u++)
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
                            else
                                note.Button = int.Parse(nbList[i]);
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
            foreach (string label in labels)
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
            while (pos < buffer.Length && !labels.Contains(buffer[pos]))
            {
                if (label.CompareTo("[NOTES]") == 0)
                    buffer[pos] += "\n";
                output += buffer[pos];
                pos++;
            }
            return output;
        }

        public byte[] parser(String path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            byte[] buffer;

            try
            {
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;

                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
    }
}