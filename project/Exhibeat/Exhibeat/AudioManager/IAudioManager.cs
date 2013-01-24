using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer
{
    interface IAudioManager
    {
        public void     init();
        public void     destroy();
        public uint     open(String path);
        public void     close(uint id);
        public void     play(uint id);
        public void     playAndForget(uint id);
        public void     stop(uint id);
        public void     pause(uint id);
        public uint     getCurrentPosMs(uint id);
        public uint     getCurrentPosS(uint id);
        public String   getTitle(uint id);
        public uint     getLengthMs(uint id);
        public uint     getLengthS(uint id);
        public String   getArtist(uint id);
        public bool     isPlaying(uint id);
    }
}
