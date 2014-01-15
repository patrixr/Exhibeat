using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Exhibeat_SongDirectory_Scan
{
    public class ExhibeatDirectoryScan
    {
        private static string fileName = "ExhibeatSongFile.xml";
        private List<string> songPath = new List<string>();

        /// <summary>
        /// Scan the specific folder to get all the songs
        /// </summary>
        /// <param name="targetDirectory"></param>
        public void ScanSongDirectory(string targetDirectory)
        {
            ProcessDirectory(targetDirectory);
            if (songPath.Count > 0)
                savePathXML(targetDirectory);
        }

        public void ProcessDirectory(string targetDirectory) 
        {
            if (Directory.Exists(targetDirectory)) 
            {
                string [] fileEntries = Directory.GetFiles(targetDirectory);
                foreach(string fileName in fileEntries)
                    ProcessFile(fileName);

                string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach(string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
            }
            else 
            {
                Console.WriteLine("{0} is not a valid file or directory.", targetDirectory);
            }  

        }
        public void ProcessFile(string path)
        {
            if (Path.GetExtension(path) == ".exi")
            {
                songPath.Add(path);
            }
        }
       /// <summary>
       /// Load a XML with all the founded songs
       /// </summary>
       /// <param name="xmlPath"></param>
       /// <returns></returns>
        public List<string> loadPathXML(string xmlPath)
        {
            List<string> songPath = new List<string>();
            if (File.Exists(xmlPath + "/" + fileName))
            {
                XmlReader reader = XmlReader.Create(xmlPath + "/" + fileName);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "path")
                    {
                        reader.Read();
                        songPath.Add(reader.Value);
                    }
                }
            }
            return songPath;
        }
        /// <summary>
        /// Create a xml with all the founded songs
        /// </summary>
        /// <param name="xmlPath"></param>
        public void savePathXML(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("xml"));
            foreach (string path in songPath)
                el.AppendChild(doc.CreateElement("path")).InnerText = path;
            if (File.Exists(xmlPath + "/" + fileName))
            {
                File.WriteAllText(xmlPath + "/" + fileName, string.Empty);
                System.IO.File.WriteAllText(xmlPath + "/" + fileName, doc.OuterXml);
            }
            else
                System.IO.File.WriteAllText(xmlPath + "/" + fileName, doc.OuterXml);
        }

    }
}
