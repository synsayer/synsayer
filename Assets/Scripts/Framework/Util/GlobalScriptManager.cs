using UnityEngine;
using System.Collections;


namespace FrameWork.Util
{

    class GlobalScriptManager
    {
        static private GameObject _objGameObject = null;
        static public GameObject objGameObject
        {
            get
            {
                if (_objGameObject == null)
                {
                    _objGameObject = new GameObject();
                    _objGameObject.name = "GlobalScriptManager";
                    Object.DontDestroyOnLoad(_objGameObject);
                }
                return _objGameObject;
            }
        }

        static public T AddComponent<T>() where T : MonoBehaviour
        {
            return objGameObject.AddComponent<T>() as T;
        }
        static public T GetComponent<T>() where T : MonoBehaviour
        {
            return objGameObject.GetComponent<T>();
        }
    }
}

