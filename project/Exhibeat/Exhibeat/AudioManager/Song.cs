using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer
{
    using FMOD;

    class Song : ISong
    {
        private FMOD.System     _lib;
        private FMOD.Sound      _song;
        private FMOD.Channel    _chanel;
        private FMOD.RESULT     _res;
        private String          _path;
        private bool            _play;

        public Song(FMOD.System lib)
        {
            _lib = lib;
        }

        public void init(string path)
        {
            _path = path;
            _play = false;
            _lib.createSound(_path, MODE.HARDWARE, ref _song);
        }

        public void destroy()
        {
            _song.release();
        }

        public void play()
        {
            if (_chanel != null)
                _chanel.setPaused(false);
            else
            {
                _res = _lib.playSound(CHANNELINDEX.FREE, _song, false, ref _chanel);
                if (_res != FMOD.RESULT.OK)
                {
                    throw new Exception("Song : File not found");
                }
            }
            _play = true;
        }

        public void stop()
        {
            if (_chanel != null)
                _chanel.stop();
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
            return (tag.ToString());
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
            return (tag.ToString());
        }

        public bool isPlaying()
        {
            return (_play);
        }
    }
}
