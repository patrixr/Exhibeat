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
        protected List<List<Note> >     _displayedNoteQueues;

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
            _displayedNoteQueues = new List<List<Note>>(7);
            for (int i = 0; i < 7; i++)
                _displayedNoteQueues.Add(new List<Note>());
        }

        #region GAME OVERRIDE

        public void Initialize(ContentManager Content)
        {
            _parser = new EXParser();
            _audioManager = ExhibeatSettings.GetAudioManager();
        }

        public bool Read(string songFilePath)
        {
            _currentMap  = _parser.parse(ExhibeatSettings.ResourceFolder + songFilePath);

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

                // WE CHECK THE DISPLAYED NOTES TO CHECK IF SOME HAVE BEEN MISSED
                _currentPos = (int)_audioManager.getCurrentPosMs(_songIndex);
                foreach (List<Note> note_queue in _displayedNoteQueues)
                {
                    while (note_queue.Count > 0)
                    {
                        Note note = note_queue[0];
                        int diff = Math.Abs(note.Offset - _currentPos);

                        // IS THE NOTE PAST AND FAILED ?
                        if ((note.Offset < _currentPos) && diff > 100)
                        {
                            SendEvent(userEvent.NOTEFAIL, new NoteEventParameter() { note = note.Button, delayms = diff });
                            note_queue.RemoveAt(0);
                            continue;
                        }
                        break;
                    }
                }

                //Console.WriteLine(_displayedNoteQueues[0].Count);
                // CHECK FOR BUTTON PRESS
                handleUserInput();

                // REFRESH NOTES DISPLAYED ON THE SCREEN
                foreach (Note note in _upcomingNotes)
                {
                    int delay;
                    delay = note.Offset - _currentPos;
                    if (delay <= ExhibeatSettings.TileGrowthDuration)
                    {
                        SendEvent(songEvent.NEWNOTE, new NoteEventParameter() { note = note.Button, delayms = delay });
                        //Console.WriteLine("playing button " + note.Length + " at " + note.Offset + " with a delay of " + delay);

                        // WE KEEP THE NOTES CURRENTLY BEING DISPLAYED IN _displayedNoteQueues
                        // @FIX double notes
                        if (_displayedNoteQueues[note.Button].Count == 0 ||
                            _displayedNoteQueues[note.Button].Last().Offset != note.Offset)
                        {
                            _displayedNoteQueues[note.Button].Add(new Note(note.Offset, note.Length, note.Button));
                        }
                        _line++;
                    }
                    else
                        break;
                }
            }
        }

        #endregion

        private void handleUserInput()
        {
            Key key;
            while ((key = _keyReader.getNextKeyEvent()) != null)
            {
                if (key.type == keyType.pressed)
                {
                    _currentPos = (int)_audioManager.getCurrentPosMs(_songIndex);
                    if (_displayedNoteQueues[key.pos].Count == 0)
                        SendEvent(userEvent.NOTEPRESSED, new NoteEventParameter() { note = key.pos, delayms = 0 });
                    while (_displayedNoteQueues[key.pos].Count > 0)
                    {
                        // TIMING
                        Note note = _displayedNoteQueues[key.pos][0];
                        int diff = Math.Abs(note.Offset - _currentPos);
                        Console.WriteLine(diff);
                        if (diff <= 100)
                            SendEvent(userEvent.NOTEVERYGOOD, new NoteEventParameter() { note = key.pos, delayms = diff });
                        else if (diff <= 200)
                            SendEvent(userEvent.NOTEGOOD, new NoteEventParameter() { note = key.pos, delayms = diff });
                        else if (diff <= 300)
                            SendEvent(userEvent.NOTENORMAL, new NoteEventParameter() { note = key.pos, delayms = diff });
                        else if (diff <= 500)
                            SendEvent(userEvent.NOTEBAD, new NoteEventParameter() { note = key.pos, delayms = diff });
                        else
                            SendEvent(userEvent.NOTEFAIL, new NoteEventParameter() { note = key.pos, delayms = diff });
                        _displayedNoteQueues[key.pos].RemoveAt(0);
                        break;
                    }
                }
                else if (key.type == keyType.realeased)
                    SendEvent(userEvent.NOTERELEASED, new NoteEventParameter() { note = key.pos, delayms = 0 });
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
