using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Rhythm
{
    public interface ITimeEventReciever
    {
        /// <summary>
        /// The reciever is notified of a change in the music
        /// </summary>
        /// <param name="ev"></param>
        public void NewSongEvent(songEvent ev, int param);

        /// <summary>
        /// The reciever is notified of a new user event, a key press
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="param"></param>
        public void NewUserEvent(userEvent ev, int param);
    }
}
