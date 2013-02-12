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
<<<<<<< HEAD
        public void NewSongEvent(songEvent ev, Object param);
=======
        void NewSongEvent(songEvent ev, int param);
>>>>>>> a7a1f5b9cc71b87c4fafb1fae1e9fb18046b1d49

        /// <summary>
        /// The reciever is notified of a new user event, a key press
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="param"></param>
<<<<<<< HEAD
        public void NewUserEvent(userEvent ev, Object param);
=======
        void NewUserEvent(userEvent ev, int param);
>>>>>>> a7a1f5b9cc71b87c4fafb1fae1e9fb18046b1d49
    }
}
