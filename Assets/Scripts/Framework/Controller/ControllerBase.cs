using FrameWork.Event;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Controller
{

    public class ControllerBase : StaticBasicEventDispatcher
    {

        static protected List<FrameWork.Model.ModelBase> m_lstModel = new List<FrameWork.Model.ModelBase>();
        static protected List<FrameWork.View.ViewBase> m_lstView = new List<FrameWork.View.ViewBase>();

        static bool bInited = false;
        static public void init()
        {
            if (bInited == false)
            {
                bInited = true;

            }

        }

        public static int dummy()
        {
            return 0;
        }



        static public void addView(FrameWork.View.ViewBase view)
        {
            m_lstView.Add(view);
        }

        static public void removeView(FrameWork.View.ViewBase view)
        {
            m_lstView.Remove(view);
        }

        static public T getView<T>() where T : FrameWork.View.ViewBase
        {
            for (int i = 0; i < m_lstView.Count; i++)
            {
                if (null != m_lstView[i] && m_lstView[i] is T)
                {
                    return m_lstView[i] as T;
                }
            }
            return null;
        }

        static public T getViewWithoutInherit<T>() where T : FrameWork.View.ViewBase
        {
            for (int i = 0; i < m_lstView.Count; i++)
            {
                if (null != m_lstView[i] && m_lstView[i].GetType() == typeof(T))
                {
                    return m_lstView[i] as T;
                }
            }
            return null;
        }

        static public T addModel<T>() where T : FrameWork.Model.ModelBase
        {
            T inst = (T)System.Activator.CreateInstance(typeof(T));
            inst.init();

            m_lstModel.Add(inst);

            return inst;
        }

        static public T getModel<T>() where T : FrameWork.Model.ModelBase
        {
            for (int i = 0; i < m_lstModel.Count; i++)
            {
                if (m_lstModel[i] is T)
                {
                    return m_lstModel[i] as T;
                }
            }
            return null;
        }

        static public void removeModel<T>(T value) where T : FrameWork.Model.ModelBase
        {
            m_lstModel.Remove(value);
            value.destroy();
        }


        static public void removeAllModel()
        {
            for (int i = 0; i < m_lstModel.Count; i++)
            {
                m_lstModel[i].destroy();
            }
            m_lstModel.Clear();
        }


        static public void shutdown()
        {
            removeAllModel();


            Application.Quit();
        }

        static public void changeScene(string strSceneName)
        {
            Application.LoadLevel(strSceneName);
            //UnityEngine.SceneManagement.SceneManager.LoadScene(strSceneName);
            //Application.LoadLevel(strSceneName);
        }


        static public string getCurScene()
        {
            //return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return Application.loadedLevelName;
        }


        static public void AddEventListenerToAllModel(string strEventID, BasicEventHandler<ModelEventArgs>.EventHandlerDelegte pFn)
        {
            for (int i = 0; i < m_lstModel.Count; i++)
            {
                m_lstModel[i].AddEventListener(strEventID, pFn);
            }
        }

        static public void RemoveEventListenerFromAllModel(string strEventID, BasicEventHandler<ModelEventArgs>.EventHandlerDelegte pFn)
        {
            for (int i = 0; i < m_lstModel.Count; i++)
            {
                m_lstModel[i].RemoveEventListener(strEventID, pFn);
            }
        }



    }
}