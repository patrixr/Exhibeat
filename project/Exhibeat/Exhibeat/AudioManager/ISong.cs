using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.AudioPlayer
{
    public interface ISong
    {
        void     init(String path);
        void     destroy();
        void     play();
        void     stop();
        void     pause();
        uint     getCurrentPosMs();
        uint     getCurrentPosS();
        String   getTitle();
        uint     getLengthMs();
        uint     getLengthS();
        String   getArtist();
        bool     isPlaying();
    
        void setOnStartCallBack(myCallback startCall);
        void setOnStopCallBack(myCallback startCall);
        void setOnEndCallBack(myCallback startCall);
        void setSyncpointCallBack(uint ms, myCallback syncCall);
    }
}
