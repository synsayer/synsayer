using System;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork.Event
{
    public class BasicEventHandler<T>
	{        
		public delegate void EventHandlerDelegte(T e);
		
		Dictionary<string, EventHandlerDelegte> m_dicHandlers = new Dictionary<string, EventHandlerDelegte>();
		
		public BasicEventHandler ()		
        {
     
		}


        virtual public void AddEventListener(string strEventID, EventHandlerDelegte pFn)
		{
			if (m_dicHandlers.ContainsKey(strEventID)==false)
			{
				m_dicHandlers.Add(strEventID, null );				
			}
								
            m_dicHandlers[strEventID] += pFn;
				
		}
		
		virtual public void DispatchEvent(string strEventID, T e)
		{
			
			if (m_dicHandlers.ContainsKey(strEventID)==true)
			{
                if (m_dicHandlers[strEventID] != null)
                {
                    m_dicHandlers[strEventID](e);
                }
			}
		}

        virtual public void RemoveEventListener(string strEventID, EventHandlerDelegte pFn)
		{
			if (m_dicHandlers.ContainsKey(strEventID)==true)
			{	
                m_dicHandlers[strEventID] -= pFn;             
			}		
			
		}

        virtual public void RemoveAllEventListener()
        {            
            m_dicHandlers.Clear();
        }
		
		
	}
}

