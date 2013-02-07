using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exhibeat.Rhythm
{
    public interface ITimeEventEmitter
    {
        /// <summary>
        /// The emitter will save the reciever in order to notify future events to it
        /// </summary>
        /// <param name="rcv"></param>
        /// <returns></returns>
        bool RegisterNewReciever(ITimeEventReciever rcv, bool songEvents = true, bool userEvents = true);
        bool UnregisterReciever(ITimeEventReciever rcv);
        void ClearAllReceivers();
    }
}
