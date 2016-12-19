using System;
using UnityEngine;
using System.Collections;
using FrameWork.Behaviour;
using FrameWork.Event;


namespace FrameWork.Controls
{
    class Button : EventBehaviour 
	{        
        private bool m_bMouseDown = false;
        private bool m_bMouseEnter = false;

        protected void OnMouseEnter()
        {
            m_bMouseEnter = true;
            //guiTexture.color = new Color(0.8f, 0.8f, 0.8f, 1);
        }

        protected void OnMouseExit()
        {
            m_bMouseEnter = false;
            //guiTexture.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        protected void OnMouseDown()
        {            
            m_bMouseDown = true;
            effectDown();
        }

        protected void OnMouseUp()
        {
            if (m_bMouseDown && m_bMouseEnter)
            {                
                onClicked();                
            }

            effectUp();
            m_bMouseDown = false;
        }

        virtual protected void onClicked()
        {
            BasicEventArgs arg = new BasicEventArgs();
            arg.objParam1 = gameObject;
            this.DispatchEvent(EventID.ID_EVENT_BUTTON_CLICK, arg);
        }

        protected void effectUp()
        {

            GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }


        protected void effectDown()
        {
            GetComponent<GUITexture>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }

       
	}
}
