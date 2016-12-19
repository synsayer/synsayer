using System;
using UnityEngine;

namespace FrameWork.Event
{
    public class BasicEventArgs : EventArgument<FrameWork.Behaviour.EventBehaviour>
	{
        static public BasicEventArgs argNone = new BasicEventArgs();
        static public BasicEventArgs s_arg = new BasicEventArgs();

        int m_nParam1;
        int m_nParam2;
        int m_nParam3;
        string m_strParam1 = "";
        GameObject m_go = null;
        object m_objParam1 = null;

        public BasicEventArgs(int nParam1 = 0, int nParam2 = 0, string strParam1 = "", int nParam3 = 0) 
		{

            m_nParam1 = nParam1;
            m_nParam2 = nParam2;
            m_nParam3 = nParam3;
            m_strParam1 = strParam1;            
		}

        public int nParam1
        {
            get { return m_nParam1; }
            set { this.m_nParam1 = value; }
        }

        public int nParam2
        {
            get { return m_nParam2; }
            set { this.m_nParam2 = value; }
        }

        public int nParam3
        {
            get { return m_nParam3; }
            set { this.m_nParam3 = value; }
        }


        public string strParam1
        {
            get { return m_strParam1; }
            set { this.m_strParam1 = value; }
        }

        public object objParam1
        {
            get { return m_objParam1; }
            set { this.m_objParam1 = value; }
        }

        public GameObject go
        {
            get { return m_go; }
            set { this.m_go = value; }
        }

	}
}

