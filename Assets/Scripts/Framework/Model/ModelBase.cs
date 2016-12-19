using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.Event;

namespace FrameWork.Model
{
    public abstract class ModelBase : BasicEventHandler<ModelEventArgs>
	{

        public virtual void init()
        {

            registerEventListener();
        }

        public virtual void destroy()
        {
            unregisterEventListener();
        }

        protected virtual void registerEventListener()
        {
            
        }

        protected virtual void unregisterEventListener()
        {

        }

        public void DispatchEvent(string strEventID)
        {
            ModelEventArgs.argNone.sender = this;

            base.DispatchEvent(strEventID, ModelEventArgs.argNone);
        }

        override public void DispatchEvent(string strEventID, ModelEventArgs e)
        {
            e.sender = this;
            base.DispatchEvent(strEventID, e);
        }

	}
}