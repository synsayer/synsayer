using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using FrameWork.Event;

namespace FrameWork.Event
{
	public class StaticBasicEventDispatcher
	{
        static BasicEventHandler<BasicEventArgs> s_dispacher = null;
        public static BasicEventHandler<BasicEventArgs> Dispacher
        {
            get
            {
                if (s_dispacher == null)
                {
                    s_dispacher = new BasicEventHandler<BasicEventArgs>();
                }
                return s_dispacher;
            }
        }

        static public void AddEventListener(string strEventID, BasicEventHandler<BasicEventArgs>.EventHandlerDelegte pFn)
        {
            Dispacher.AddEventListener(strEventID, pFn);
        }

        static public void DispatchEvent(string strEventID, BasicEventArgs e)
        {
            Dispacher.DispatchEvent(strEventID, e);
        }

        static public void RemoveEventListener(string strEventID, BasicEventHandler<BasicEventArgs>.EventHandlerDelegte pFn)
        {
            Dispacher.RemoveEventListener(strEventID, pFn);
        }
        static public void RemoveAllEventListener()
        {

            Dispacher.RemoveAllEventListener();
        }
	}

    
}
