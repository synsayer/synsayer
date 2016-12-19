using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FrameWork.Util;


namespace FrameWork.Behaviour
{   
	public class TimerBehaviour : EventBehaviour
	{
        
        //timer
        private Dictionary<string, CustomTimer> m_dicTimer = null;
        
        
        public TimerBehaviour()
        {

        }


        protected Dictionary<string, CustomTimer> gs_TimerDictionary
        {
            get {

                //lazy init
                if (m_dicTimer == null)
                {
                    m_dicTimer = new Dictionary<string, CustomTimer>();
                }
                return m_dicTimer;
            }

        }		
        

        protected void SetTimer(string strTimerID, float fInterval, FunctionPointer fnTimer)
        {
            CustomTimer ct = new CustomTimer();
            ct.SetInterval(fInterval);
            ct.SetTimerFunction(fnTimer);
            ct.Start();

            if (gs_TimerDictionary.ContainsKey(strTimerID))
            {
                gs_TimerDictionary[strTimerID].SetInterval(fInterval);

            }
            else
            {
                gs_TimerDictionary.Add(strTimerID, ct);
            }
        }

        protected void KillTimer(string strTimerID)
        {

            if (gs_TimerDictionary.ContainsKey(strTimerID))
            {
                gs_TimerDictionary[strTimerID].KillReserve();
                //m_dicTimer.Remove(strTimerID);
            }

        }



        void RemoveReservedTimer()
        {
            if (m_dicTimer == null)
                return;

            bool found = false;

            foreach (KeyValuePair<string, CustomTimer> p in gs_TimerDictionary)
            {
                if (p.Value.DoesKillReserve())
                {
                    gs_TimerDictionary.Remove(p.Key);

                    found = true;

                    break;	//exit loop    
                }


            }

            // Call again recursively

            if (found)
                RemoveReservedTimer();

        }

        virtual public void Update()
        {

            if (m_dicTimer != null)
            {
                RemoveReservedTimer();
                foreach (KeyValuePair<string, CustomTimer> p in gs_TimerDictionary)
                {

                    p.Value.Update(Time.deltaTime);
                }
            }

        }


        

	}
}
