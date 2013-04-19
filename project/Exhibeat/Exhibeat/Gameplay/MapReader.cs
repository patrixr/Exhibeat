using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exhibeat.Parser;
using Exhibeat.AudioPlayer;
using Exhibeat.Rhythm;
using Exhibeat.Settings;
using ExhiBeat.KeyReader;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Exhibeat.Settings;

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

        protected int                   _timeElapsed;
        protected int                   _currentPos;

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

        protected IKeyReader _keyReader;


        public MapReader()
        {
            _line = 0;
            _eventReceiverList = new List<EventRecievers>();
            _keyReader = new KeyReader();
        }

        #region GAME OVERRIDE

        public void Initialize(ContentManager Content)
        {
            _parser = new EXParser();
            _audioManager = ExhibeatSettings.GetAudioManager();
        }

        public bool Read(string songFilePath)
        {
            _currentMap  = _parser.parse(songFilePath);

            //_currentMap = _parser.parse(songFilePath);

            if (_currentMap == null)
                return false;
            _songIndex = _audioManager.open(ExhibeatSettings.ResourceFolder + _currentMap.Path);
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
            if (_songPlaying && _upcomingNotes != null)
            {
                int tmp = (int)_audioManager.getCurrentPosMs(_songIndex);
                _timeElapsed = tmp - _currentPos;
                _currentPos = tmp;
                ExhibeatSettings.TimeElapsed = _timeElapsed;

                handleUserInput();
                foreach (Note note in _upcomingNotes)
                {
                    int delay;
                    delay = note.Offset - _currentPos;
                    if (delay <= ExhibeatSettings.TileGrowthDuration)
                    {
                        SendEvent(songEvent.NEWNOTE, new NoteEventParameter() { note = note.Button, delayms = delay });
                        Console.WriteLine("playing button " + note.Length + " at " + note.Offset + " with a delay of " + delay);

                        _line++;
                    }
                }
            }
        }

        #endregion

        private void handleUserInput()
        {
            Key _key = new Key();
            while ((_key = _keyReader.getNextKeyEvent()) != null)
            {
                if (_key.type == keyType.pressed)
                {
                    _currentPos = (int)_audioManager.getCurrentPosMs(_songIndex);
                    SendEvent(userEvent.NOTEPRESSED, new NoteEventParameter() { note = _key.pos, delayms = 0 });
                    foreach (Note note in _upcomingNotes)
                    {
                        //TIMING
                    }
                }
                else if (_key.type == keyType.realeased)
                    SendEvent(userEvent.NOTERELEASED, new NoteEventParameter() { note = _key.pos, delayms = 0 });
            }
        }

        public uint getMapCompletion()
        {
            uint totalSize = _audioManager.getLengthMs(_songIndex);
            uint currentPos = _audioManager.getCurrentPosMs(_songIndex);


            return ((currentPos * 100) / totalSize);
        }

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
