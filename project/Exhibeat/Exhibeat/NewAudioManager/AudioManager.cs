using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Exhibeat.AudioPlayer
{

    class AudioManager : IAudioManager
    {
        private List<ISong> _songList;
        private List<int>   _toForget;
        private int         _index;
        private float       _volume;

        public void init()
        {
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                _songList = new List<ISong>();
                _toForget = new List<int>();
                _volume = Bass.BASS_GetVolume();
            }
            else
                throw new Exception("Cannot initialize audio library");
        }

        public void destroy()
        {
            _songList = null;
            _toForget = null;
            Bass.BASS_Free();
        }

        public void update()
        {
          //  _lib.update();
        }

        public int open(string path)
        {
            Song nSong = new Song(path);
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
        {/*
            int i;
            for (i = 0; i < _toForget.Count; i++)
            {
                if (_songList[_toForget[i]].getLengthMs() == _songList[_toForget[i]].getLengthMs())
                {
                    close(_toForget[i]);
                    _toForget.RemoveAt(i);
                    break;
                }
            }*/
        }

        public void playAndForget(int id)
        {/*
            myCallback cbForget = new myCallback(toForgetCallback);

            _toForget.Add(id);
            _songList[id].setOnEndCallBack(cbForget);
            _songList[id].play();*/
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
        //    _songList[index].setOnStartCallBack(startCall);
        }

        public void setOnStopCallBack(int index, myCallback stopCall)
        {
          //  _songList[index].setOnStopCallBack(stopCall);
        }

        public void setOnEndCallBack(int index, myCallback endCall)
        {
            //_songList[index].setOnEndCallBack(endCall);
        }

        public void setSyncpointCallBack(int index, uint ms, myCallback syncCall)
        {
           // _songList[index].setSyncpointCallBack(ms, syncCall);
        }

        public void increaseChannelVolume(int id)
        {
            _songList[id].increaseVolume();
        }

        public void decreaseChannelVolume(int id)
        {
            _songList[id].decreaseVolume();
        }

        public void setChannelVolume(int id, float volume)
        {
            _songList[id].setVolume(volume);
        }

        public float getChannelVolume(int id)
        {
            return (_songList[id].getVolume());
        }

        public void increaseVolume()
        {
            if (_volume + 0.05 <= 1)
                _volume += 0.05f;
            else
                _volume = 1;
            Bass.BASS_SetVolume(_volume);   
        }

        public void decreaseVolume()
        {
            if (_volume - 0.05 >= 0)
                _volume -= 0.05f;
            else
                _volume = 0;
            Bass.BASS_SetVolume(_volume);
        }

        public void setVolume(float volume)
        {
            if (volume >= 0 && volume <= 1)
            {
                _volume = volume;
                Bass.BASS_SetVolume(volume);
            }
        }

        public float getVolume()
        {
            return Bass.BASS_GetVolume();
        }

    }
}