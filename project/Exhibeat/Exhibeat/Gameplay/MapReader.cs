using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exhibeat.Parser;
using Exhibeat.AudioPlayer;
using Exhibeat.Rhythm;

namespace Exhibeat.Gameplay
{
    class MapReader
    {
        protected ITimeEventEmitter _eventEmitter;
        protected ITimeEventReciever _eventReceiver;
        protected Map _currentMap;
        protected IAudioManager _audioManager;
        protected ISong _song;
        protected List<Note> _upcomingNotes;
        protected bool notePlayed = false;
        protected int _line;

        public MapReader(ITimeEventReciever EventReceiver, string songPath)
        {
            _eventReceiver = EventReceiver;
            EXParser parser = new EXParser();
            _currentMap = parser.parse(songPath);
            _upcomingNotes = _currentMap.GetNotesFromIndex(0, 5);
            _line = 0;
        }

        #region GAME OVERRIDE

        public void Initialize(ContentManager Content)
        {
        }

        // Les notes sont celles parsées dans le fichier flower.exi
        public void Update(GameTime gameTime)
        {

            #region old_update
            /*notePlayed = false;
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            foreach (Note current in _upcomingNotes)
            {
                int delay = current.getOffset() - _timer;
                if (delay == ExhibeatConfiguration.GLOW_DURATION)
                {
                    _eventEmitter.MakeGlow(current.getButtons(), ExhibeatConfiguration.GLOW_DURATION);
                }
                else if (_timer == current.getOffset())
                {
                    notePlayed = true;
                    List<int> buttons = current.getButtons();
                    SendEvent(buttons);
                    _line++;
                    break;
                }
            }
            if (notePlayed == true)
                _upcomingNotes = _currentMap.GetNotesFromIndex(_line, 5);*/
            #endregion
        }

        #endregion

        public void SendEvent(List<int> indexes)
        {
        }
    }
}
