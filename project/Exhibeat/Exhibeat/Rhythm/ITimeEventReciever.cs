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
        void NewSongEvent(songEvent ev, Object param);

        /// <summary>
        /// The reciever is notified of a new user event, a key press
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="param"></param>
        void NewUserEvent(userEvent ev, Object param);
    }
}
