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
        public bool RegisterNewReciever(ITimeEventReciever rcv, bool songEvents = true, bool userEvents = true);
        public bool UnregisterReciever(ITimeEventReciever rcv);
        public void ClearAllReceivers();
    }
}
