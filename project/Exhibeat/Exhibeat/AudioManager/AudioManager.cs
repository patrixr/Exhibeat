using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.AudioPlayer
{
    using FMOD;

    public class AudioManager : IAudioManager
    {
        private FMOD.System _lib;
        private List<ISong> _songList;
        private List<int>   _toForget;
        private int         _index;

        private void ErrorChecker(FMOD.RESULT res)
        {
            if (res != FMOD.RESULT.OK)
                throw new ApplicationException("Fmod Error : " + res + " - " + Error.String(res));
        }

        public void init()
        {
            FMOD.RESULT res;
            uint libVersion = 0;
            
            _songList = new List<ISong>();
            _toForget = new List<int>();
            _index = 0;
            res = FMOD.Factory.System_Create(ref _lib);
            ErrorChecker(res);
            res = _lib.getVersion(ref libVersion);
            ErrorChecker(res);
            if (libVersion < FMOD.VERSION.number)
                throw new ApplicationException("Error! You are using an old version of FMOD " + libVersion.ToString("X") + ". This program requires " + VERSION.number.ToString("X") + ".");
            res = _lib.init(32, INITFLAGS.NORMAL, (IntPtr)null);
            ErrorChecker(res);
        }

        public void destroy()
        {
            _songList = null;
            _toForget = null;
            _lib.release();
        }

        public void update()
        {
            _lib.update();
        }

        public int open(string path)
        {
            Song nSong = new Song(ref _lib, path);
            int i;

            for (i = 0; i < _songList.Count; i++)
            {
                if (_songList[i] == null)
                {
                    _songList[i] = nSong;
                    return (i);
                }
            }
            _songList.Add(nSong);
            _index++;
            return (_index - 1);
        }

        public void close(int id)
        {
            _songList[id].destroy();
            _songList[id] = null;
        }

        public void play(int id)
        {
            _songList[id].play();
        }

        public void toForgetCallback()
        {
            int i;
            for (i = 0; i < _toForget.Count; i++)
            {
                if (_songList[_toForget[i]].getLengthMs() == _songList[_toForget[i]].getLengthMs())
                {
                    close(_toForget[i]);
                    _toForget.RemoveAt(i);
                    break;
                }
            }
        }

        public void playAndForget(int id)
        {
            myCallback cbForget = new myCallback(toForgetCallback);

            _toForget.Add(id);
            _songList[id].setOnEndCallBack(cbForget);
            _songList[id].play();
        }

        public void stop(int id)
        {
            _songList[id].stop();
        }

        public void pause(int id)
        {
            _songList[id].pause();
        }

        public uint getCurrentPosMs(int id)
        {
            return (_songList[id].getCurrentPosMs());
        }

        public uint getCurrentPosS(int id)
        {
            return (_songList[id].getCurrentPosS());
        }

        public string getTitle(int id)
        {
            return (_songList[id].getTitle());
        }

        public uint getLengthMs(int id)
        {
            return (_songList[id].getLengthMs());
        }

        public uint getLengthS(int id)
        {
            return (_songList[id].getLengthS());
        }

        public string getArtist(int id)
        {
            return (_songList[id].getArtist());
        }

        public bool isPlaying(int id)
        {
            return (_songList[id].isPlaying());
        }

        public void setOnStartCallBack(int index, myCallback startCall)
        {
            _songList[index].setOnStartCallBack(startCall);
        }

        public void setOnStopCallBack(int index, myCallback stopCall)
        {
            _songList[index].setOnStopCallBack(stopCall);
        }

        public void setOnEndCallBack(int index, myCallback endCall)
        {
            _songList[index].setOnEndCallBack(endCall);
        }

        public void setSyncpointCallBack(int index, uint ms, myCallback syncCall)
        {
            _songList[index].setSyncpointCallBack(ms, syncCall);
        }

        public void increaseVolume(int id)
        {
            _songList[id].increaseVolume();
        }

        public void decreaseVolume(int id)
        {
            _songList[id].decreaseVolume();
        }

        public void setVolume(int id, float volume)
        {
            _songList[id].setVolume(volume);
        }

        public float getVolume(int id)
        {
            return (_songList[id].getVolume());
        }
    }
}
