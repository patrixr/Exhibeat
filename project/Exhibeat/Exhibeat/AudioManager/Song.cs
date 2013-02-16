using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.AudioPlayer
{
    using FMOD;

    public class Song : ISong
    {
        private FMOD.System     _lib;
        private FMOD.Sound      _song;
        private FMOD.Channel    _chanel;
        private FMOD.RESULT     _res;
        private String          _path;
        private bool            _play;
        private myCallback      _startCall;
        private myCallback      _stopCall;
        private myCallback      _endCall;
        private myCallback      _syncCall;
        private IntPtr          _ptr;
        private CHANNEL_CALLBACK _call;

        public Song(ref FMOD.System lib)
        {
            _lib = lib;
        }

        public Song(ref FMOD.System lib, String path)
        {
            _lib = lib;
            init(path);
        }

        public void init(string path)
        {
            _startCall = null;
            _stopCall = null;
            _endCall = null;
            _syncCall = null;
            _path = path;
            _play = false;
            _lib.createSound(_path, MODE.HARDWARE, ref _song);
            _call = new CHANNEL_CALLBACK(onChanelCallBack);
        }

        private FMOD.RESULT onChanelCallBack(IntPtr chan, FMOD.CHANNEL_CALLBACKTYPE type, IntPtr p1, IntPtr p2)
        {
            if (type == FMOD.CHANNEL_CALLBACKTYPE.END)
            {
                if (_endCall != null)
                    _endCall();
            }
            else if (type == FMOD.CHANNEL_CALLBACKTYPE.SYNCPOINT)
            {
                if (_syncCall != null)
                    _syncCall();
            }
            return (FMOD.RESULT.OK);
        }

        public void destroy()
        {
            _song.release();
        }

        public void play()
        {
            if (_startCall != null)
                _startCall();
            if (_chanel != null)
                _chanel.setPaused(false);
            else
            {
                _res = _lib.playSound(CHANNELINDEX.FREE, _song, false, ref _chanel);
                if (_res != FMOD.RESULT.OK)
                {
                    throw new Exception("Song : File not found");
                }
                
                _chanel.setCallback(_call);
            }
            _play = true;
        }

        public void stop()
        {
            if (_stopCall != null)
                _stopCall();
            if (_chanel != null)
            {
                _chanel.stop();
                _chanel = null;
            }
            _play = false;
        }

        public void pause()
        {
            if (_chanel != null)
                _chanel.setPaused(true);
            _play = false;
        }

        public uint getCurrentPosMs()
        {
            uint pos = 0;

            if (_chanel != null)
                _chanel.getPosition(ref pos, TIMEUNIT.MS);
            return (pos);
        }

        public uint getCurrentPosS()
        {
            uint pos = 0;

            if (_chanel != null)
                _chanel.getPosition(ref pos, TIMEUNIT.MS);
            pos /= 1000;
            return (pos);
        }

        public string getTitle()
        {
            FMOD.TAG tag = new FMOD.TAG();

            _song.getTag("TITLE", 0, ref tag);
            return (Marshal.PtrToStringAnsi(tag.data));
        }

        public uint getLengthMs()
        {
            uint length = 0;

            if (_song != null)
                _song.getLength(ref length, TIMEUNIT.MS);
            return (length);
        }

        public uint getLengthS()
        {
            uint length = 0;

            if (_song != null)
                _song.getLength(ref length, TIMEUNIT.MS);
            length /= 1000;
            return (length);
        }

        public string getArtist()
        {
            FMOD.TAG tag = new FMOD.TAG();
            
            _song.getTag("ARTIST", 0, ref tag);
            return (Marshal.PtrToStringAnsi(tag.data));
        }

        public bool isPlaying()
        {
            return (_play);
        }

        public void setOnStartCallBack(myCallback startCall)
        {
            _startCall = startCall;
        }

        public void setOnStopCallBack(myCallback stopCall)
        {
            _stopCall = stopCall;
        }

        public void setOnEndCallBack(myCallback endCall)
        {
            _endCall = endCall;
        }

        public void setSyncpointCallBack(uint ms, myCallback syncCall)
        {
            _song.addSyncPoint(ms, TIMEUNIT.MS, "EndSong", ref _ptr);
            _syncCall = syncCall;
        }
    }
}
