using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Exhibeat.AudioPlayer
{
    class Song : ISong
    {
        private int             _channel;
        private String          _path;
        private bool            _isPlaying;
        private bool            _isPaused;
        private bool            _hasBeenPlayedOnce;
        private float           _volume;
        private myCallback      _startCall;
        private myCallback      _stopCall;
        private myCallback      _endCall;
        private myCallback      _syncCall;
        private IntPtr          _ptr;
        private SYNCPROC        _endSync;
        private SYNCPROC        _pointSync;

        public Song()
        {
            init(null);
        }

        public Song(String path)
        {
            init(path);
        }

        public void init(String path)
        {
            _startCall = null;
            _stopCall = null;
            _endCall = null;
            _syncCall = null;
            _path = path;
            _isPlaying = false;
            _isPaused = false;
            _hasBeenPlayedOnce = true;
            _endSync = new SYNCPROC(EndSync);
            _pointSync = new SYNCPROC(PointSync);
            _volume = 0.2f;
            if (path != null)
                _channel = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
            else
                _channel = 0;
        }

        public void setVolume(float volume)
        {
            if (volume >= 0 && volume <= 1 && _channel != 0)
            {
                _volume = volume;
                Bass.BASS_ChannelSetAttribute(_channel, BASSAttribute.BASS_ATTRIB_VOL, volume);
            }
        }

        public void increaseVolume()
        {
            if (_channel != 0)
            {
                if (_volume + 0.05 <= 1)
                    _volume += 0.05f;
                else
                    _volume = 1;
                Bass.BASS_ChannelSetAttribute(_channel, BASSAttribute.BASS_ATTRIB_VOL, _volume); 
            }
        }

        public void decreaseVolume()
        {
            if (_channel != 0)
            {
                if (_volume - 0.05 >= 0)
                    _volume -= 0.05f;
                else
                    _volume = 0;
                Bass.BASS_ChannelSetAttribute(_channel, BASSAttribute.BASS_ATTRIB_VOL, _volume);
            }
        }

        public float getVolume()
        {
            return (_volume);
        }

        public void destroy()
        {
            Bass.BASS_StreamFree(_channel);
        }

        public void play()
        {
            if (_channel != 0)
            {
                
                if (_isPaused == true)
                    Bass.BASS_ChannelPlay(_channel, false);
                else
                {
                    if (_startCall != null)
                        _startCall();
                    Bass.BASS_ChannelPlay(_channel, true);
                }
                _isPlaying = true;
                _isPaused = false;
            }
        }

        public void stop()
        {
            if (_channel != 0)
            {
                Bass.BASS_ChannelStop(_channel);
                _isPlaying = false;
                _isPaused = false;
                _stopCall();
            }
        }

        public void pause()
        {
            if (_channel != 0)
            {
                Bass.BASS_ChannelStop(_channel);
                _isPlaying = false;
                _isPaused = true;
            }
        }

        public uint getCurrentPosMs()
        {
            double pos = 0;

            pos = Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetPosition(_channel));
            pos *= 1000;
            return ((uint)pos);
        }

        public uint getCurrentPosS()
        {
            double pos = 0;

            pos = Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetPosition(_channel));
            return ((uint)pos);
        }

        public uint getLengthMs()
        {
            double length = 0;

            length = Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetLength(_channel));
            length *= 1000;
            return ((uint)length);
        }

        public uint getLengthS()
        {
            double length = 0;

            length = Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetLength(_channel));
            return ((uint)length);
        }

        public string getTitle()
        {
            String title = null;

            title = Bass.BASS_ChannelGetTagsID3V2(_channel)[0];
            title = title.Remove(0, 5);
            return (title);
        }

        public string getArtist()
        {
            String artist = null;

            artist = Bass.BASS_ChannelGetTagsID3V2(_channel)[1];
            artist = artist.Remove(0, 5);
            return (artist);
        }

        public bool isPlaying()
        {
            return (_isPlaying);
        }

        public bool hasBeenPlayedOnce()
        {
            return (_hasBeenPlayedOnce);
        }

        public void setOnStartCallBack(myCallback startCall)
        {
          _startCall = startCall;
        }

        public void setOnStopCallBack(myCallback stopCall)
        {
          _stopCall = stopCall;
        }

        private void EndSync(int handle, int channel, int data, IntPtr user)
        {
            _hasBeenPlayedOnce = true;
            _endCall();
        }

        private void PointSync(int handle, int channel, int data, IntPtr user)
        {
            _syncCall();
        }

        public void setOnEndCallBack(myCallback endCall)
        {
            _endCall = endCall;
            Bass.BASS_ChannelSetSync(_channel, BASSSync.BASS_SYNC_END, 0, _endSync, IntPtr.Zero);
        }

        public void setSyncpointCallBack(uint ms, myCallback syncCall)
        {
            _syncCall = syncCall;
            Bass.BASS_ChannelSetSync(_channel, BASSSync.BASS_SYNC_POS, Bass.BASS_ChannelSeconds2Bytes(_channel, ms / 1000), _pointSync, IntPtr.Zero); 
        }
    }
}
