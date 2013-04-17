using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.AudioPlayer
{
    public delegate void myCallback();

    interface IAudioManager
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
        void     increaseChannelVolume(int id);
        void     decreaseChannelVolume(int id);
        void     setChannelVolume(int id, float volume);
        float    getChannelVolume(int id);
        void     increaseVolume();
        void     decreaseVolume();
        void     setVolume(float volume);
        float    getVolume();

        void setOnStartCallBack(int index, myCallback startCall);
        void setOnStopCallBack(int index, myCallback stopCall);
        void setOnEndCallBack(int index, myCallback endCall);
        void setSyncpointCallBack(int index, uint ms, myCallback syncCall);
    }
}
