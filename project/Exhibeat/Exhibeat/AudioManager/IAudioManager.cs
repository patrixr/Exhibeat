using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.AudioPlayer
{
    public delegate void myCallback();

    public interface IAudioManager
    {
        void     init();
        void     destroy();
        int      open(String path);
        void     close(int id);
        void     play(int id);
        void     playAndForget(int id);
        void     stop(int id);
        void     pause(int id);
        uint     getCurrentPosMs(int id);
        uint     getCurrentPosS(int id);
        String   getTitle(int id);
        uint     getLengthMs(int id);
        uint     getLengthS(int id);
        String   getArtist(int id);
        bool     isPlaying(int id);
        void     update();

        void setOnStartCallBack(int index, myCallback startCall);
        void setOnStopCallBack(int index, myCallback stopCall);
        void setOnEndCallBack(int index, myCallback endCall);
        void setSyncpointCallBack(int index, uint ms, myCallback syncCall);
    }
}
