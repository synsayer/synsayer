using System;



namespace FrameWork.Util
{
    public delegate void FunctionPointer();
    public class CustomTimer
    {

        private bool m_bStop = true;
        private float m_fTimeElapsed = 0.0f;
        private float m_fInterval = 0.0f;

        private FunctionPointer m_fnTimer = null;

        public bool m_bReservedKill = false;

        public void SetInterval(float fInterval)
        {
            m_fInterval = fInterval;

        }

        public void KillReserve()
        {
            m_bReservedKill = true;
        }

        public bool DoesKillReserve()
        {
            return m_bReservedKill;
        }

        public void SetTimerFunction(FunctionPointer fnTimer)
        {
            m_fnTimer = fnTimer;

        }

        public int Update(float deltaTime)
        {

            if (m_bStop)
                return 0;

            m_fTimeElapsed += deltaTime;

            if (m_fTimeElapsed > m_fInterval)
            {
                int nLoop = (int)(m_fTimeElapsed / m_fInterval);
                m_fTimeElapsed = 0.0f;

                if (m_fnTimer != null)
                {
                    for (int i = 0; i < nLoop; ++i)
                    {
                        if (m_bStop)
                            return 0;
                        m_fnTimer();
                    }
                }

                return nLoop;
            }

            return 0;


        }

        public void Stop()
        {
            m_bStop = true;
        }

        public void Start()
        {
            m_bStop = false;
            m_fTimeElapsed = 0.0f;
        }
    }

}


