using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exhibeat.Parser;
using Exhibeat.AudioPlayer;
using Exhibeat.Rhythm;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

/// <summary>
/// en attendant la class
/// </summary>
namespace Exhibeat.Gameplay
{
    public class MapReader : ITimeEventEmitter
    {
        public class EventRecievers
        {
            public ITimeEventReciever recv;
            public bool songEvents;
            public bool userEvents;

            public EventRecievers(ITimeEventReciever receiver, bool songEvent, bool userEvent)
            {
                recv = receiver;
                songEvents = songEvent;
                userEvents = userEvent;
            }

        };

        protected UInt16                _delay = 1000;
        protected EXParser              _parser;
        protected Map                   _currentMap;
        protected List<Note>            _upcomingNotes;

        /// <summary>
        /// TODO CHANGER LE NOM
        /// </summary>
        protected List<EventRecievers>  _eventReceiverList;

        protected IAudioManager _audioManager;
        protected int           _songIndex;
        protected int           _songIndex2;

        protected bool _songPlaying = false;
        protected bool _notePlayed = false;
        protected int  _line;


        public MapReader()
        {
            _line = 0;
            _eventReceiverList = new List<EventRecievers>();
        }

        #region GAME OVERRIDE

        public void Initialize(ContentManager Content)
        {
            _parser = new EXParser();
            _audioManager = new AudioManager();
            _audioManager.init();
        }

        public bool Read(string songFilePath)
        {
            _currentMap  = _parser.parse(songFilePath);
            if (_currentMap == null)
                return false;
            _songIndex = _audioManager.open(_currentMap.Path);
            return true;
        }
        public bool Play()
        {
            try
            {
                _audioManager.play(_songIndex);
            }
            catch (Exception)
            {
                return false;
            }
            _songPlaying = true;
            return true;
        }
        public bool Pause()
        {
            try
            {
                _audioManager.pause(_songIndex);
            }
            catch (Exception)
            {
                return false;
            }
            _songPlaying = false;
            return true;
        }
        public bool Stop()
        {
            try
            {
                _audioManager.stop(_songIndex);
            }
            catch (Exception)
            {
                return false;
            }
            _songPlaying = false;
            return true;
        }

        public void Update(GameTime gameTime)
        {
            _upcomingNotes = _currentMap.GetNotesFromIndex(_line, 10);
            if (_upcomingNotes != null)
            {
                foreach (Note note in _upcomingNotes)
                {
                    int currentPos = (int)_audioManager.getCurrentPosMs(_songIndex);
                    int delay = note.Offset - currentPos;
                    if (delay <= 10)
                    {
                        SendEvent(songEvent.NEWNOTE, note.Length);
                        Console.WriteLine("playing button " + note.Length + " at " + note.Offset + " with a delay of " + delay);
                        _songIndex2 = _audioManager.open("taiko-normal-hitclap.wav");
                        _audioManager.playAndForget(_songIndex2);
                        _line++;
                    }
                }
            }
        }

        #endregion

        public void SendEvent(songEvent ev, Object param)
        {
            foreach  (EventRecievers eventReceiv in _eventReceiverList)
            {
                if (eventReceiv.songEvents == true)
                    eventReceiv.recv.NewSongEvent(ev, param);
            }
        }
        public void SendEvent(userEvent ev, Object param)
        {
            foreach (EventRecievers eventReceiv in _eventReceiverList)
            {
                if (eventReceiv.userEvents == true)
                    eventReceiv.recv.NewUserEvent(ev, param);
            }
        }

        public bool RegisterNewReciever(ITimeEventReciever rcv, bool songEvents = true, bool userEvents = true)
        {
            if (rcv != null && _eventReceiverList.FindIndex(item => item.recv == rcv) != -1)
                return false;
            _eventReceiverList.Add(new EventRecievers(rcv, songEvents, userEvents));
            return true;
        }

        public bool UnregisterReciever(ITimeEventReciever rcv)
        {
            if (rcv != null && _eventReceiverList.FindIndex(item => item.recv == rcv) == -1)
                return false;
            _eventReceiverList.RemoveAll(item => item.recv == rcv);
            return true;
        }

        public void ClearAllReceivers()
        {
            _eventReceiverList.Clear();
        }
    }
}
