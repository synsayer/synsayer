using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FrameWork.Event;

namespace FrameWork.Behaviour
{   

    public class EventBehaviour : MonoBehaviour
	{
      
        private BasicEventHandler<BasicEventArgs> m_BasicEventHandler = null;


        public EventBehaviour()
        {
            
        }


        public void AddEventListener(string strEventID, BasicEventHandler<BasicEventArgs>.EventHandlerDelegte pFn)
        {            
            EventHandler.AddEventListener(strEventID, pFn);
        }

        public void DispatchEvent(string strEventID)
        {
            BasicEventArgs.argNone.sender = this;
            EventHandler.DispatchEvent(strEventID, BasicEventArgs.argNone);
        }
        public void DispatchEvent(string strEventID, BasicEventArgs e)
        {
            e.sender = this;
            EventHandler.DispatchEvent(strEventID, e);
        }

        public void RemoveEventListener(string strEventID, BasicEventHandler<BasicEventArgs>.EventHandlerDelegte pFn)
        {
            EventHandler.RemoveEventListener(strEventID, pFn);
        }

        public void RemoveAllEventListener()
        {

            EventHandler.RemoveAllEventListener();
        }

        protected BasicEventHandler<BasicEventArgs> EventHandler
        {
            //lazy init            
            get{
                if (m_BasicEventHandler == null)
                {

                    m_BasicEventHandler = new BasicEventHandler<BasicEventArgs>();
                }

                return m_BasicEventHandler; 
            }          
        }


        
	}
       

  
}
