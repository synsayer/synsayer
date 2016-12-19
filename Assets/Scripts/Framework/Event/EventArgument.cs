using System;
using UnityEngine;

namespace FrameWork.Event
{
    public interface IEventArgumentBase 
    {

    }
    public class EventArgument<T> : IEventArgumentBase
	{
        T m_sender;        		
		
        public EventArgument()
		{           
			
		}
		
		
        public T sender
        {
            get { return m_sender; }
            set { this.m_sender = value; }
        }
	}
}

