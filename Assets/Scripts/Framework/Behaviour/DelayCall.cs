// 2014 - Pixelnest Studio
using System;
using System.Collections;
using UnityEngine;

namespace FrameWork.Behaviour
{
    public class DelayCall
    {
        private bool repeat;
        private int nRepeatCount;
        private bool stop;
        private float duration;
        private Action callback;
        IEnumerator m_enumerator;
        public bool bCompelete;

        public DelayCall(float duration, Action callback, bool repeat, int nRepeatCount)
        {
            this.stop = false;
            this.repeat = repeat;
            this.duration = duration;
            this.callback = callback;
            this.nRepeatCount = nRepeatCount;

            m_enumerator = Start();
        }

        /// <summary>
        /// Start in co-routine
        /// </summary>
        /// <returns></returns>
        public IEnumerator Start()
        {
            int nRepeat = 0;
            do
            {
                if (nRepeatCount > 0 && nRepeat >= nRepeatCount)
                    break;

                if (stop == false)
                {
                    yield return new WaitForSeconds(duration);

                    if (callback != null && stop == false)
                        callback();
                }

                nRepeat++;
            } while (repeat && !stop);

            bCompelete = true;
        }

        /// <summary>
        /// Stop the timer (at next frame)
        /// </summary>
        public void Stop()
        {
            this.stop = true;
        }

        public bool Repeat
        {
            get { return repeat; }
        }

        public IEnumerator enumerator
        {
            get { return m_enumerator; }
        }
    }
}