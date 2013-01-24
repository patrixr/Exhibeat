using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer
{
    interface ISong
    {
        public void     init(String path);
        public void     destroy();
        public void     play();
        public void     stop();
        public void     pause();
        public uint     getCurrentPosMs();
        public uint     getCurrentPosS();
        public String   getTitle();
        public uint     getLengthMs();
        public uint     getLengthS();
        public String   getArtist();
        public bool     isPlaying();
    }
}
