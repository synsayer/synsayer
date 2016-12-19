using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FrameWork.Util;


namespace FrameWork.Behaviour
{
    public class CommonBehaviour : EventBehaviour
    {

        public CommonBehaviour()
        {

        }

        public GameObject createChildPrefabInstance(string strInstName, GameObject prefab, Vector3 pos, Quaternion rotation, float fDestroyDelayTime = 0.0f, bool bSingletone = false)
        {
            if (prefab == null)
                return null;

            if (bSingletone)
            {
                Transform child = transform.Find(strInstName);
                if (child != null)
                {
                    return child.gameObject;    //이미 존재하는경우
                }
            }

            GameObject inst = (GameObject)Instantiate(prefab, pos, rotation);
            inst.name = strInstName;
            inst.layer = gameObject.layer;
            inst.transform.parent = transform;

            if (fDestroyDelayTime > 0)
                Destroy(inst, fDestroyDelayTime);
            
            return inst;
        }
  
        //create prefeb instance as a child
        public GameObject createChildPrefabInstance(string strInstName, GameObject prefab, float fDestroyDelayTime=0.0f, bool bSingletone = false)
        {            
            if (prefab == null)
                return null;

            if (bSingletone)
            {
                Transform child = transform.Find(strInstName);
                if (child != null)
                {
                    return child.gameObject;    //이미 존재하는경우
                }
            }            

            GameObject inst = (GameObject)Instantiate(prefab);                        
            inst.name = strInstName;            
            inst.layer = gameObject.layer;
            inst.transform.parent = transform;

            if (fDestroyDelayTime > 0)
                Destroy(inst, fDestroyDelayTime);

            return inst;
        }



        //remove child 
        public void destroyChildGameObject(string strInstName, bool bImmdiate= false)
        {
            Transform trans = transform.Find(strInstName);
            if (trans != null)
            {
                if (bImmdiate)
                {
                    DestroyImmediate(trans.gameObject);
                }
                else
                {
                    Destroy(trans.gameObject);
                }
                
            }            
        }

//         private IEnumerator _destroyAfter(GameObject obj, float fDestroyDelay)
//         {           
//             yield return new WaitForSeconds(fDestroyDelay);
//             Destroy(obj);
//         }


        virtual protected void Update()
        {

        }



        //지연된 호출
        public DelayCall delayedCall(float duration, Action callback)
        {
            if (gameObject.activeSelf)
            {
                DelayCall delayCall = new DelayCall(duration, callback, false, 0);
                StartCoroutine(delayCall.Start());
                return delayCall;
            }

            return null;
        }

        //nRepeatCount 가 0이면 무한대 반복
        public DelayCall delayedCall(float duration, Action callback, int nRepeatCount)
        {
            if (gameObject.activeSelf)
            {
                DelayCall delayCall = new DelayCall(duration, callback, true, nRepeatCount);
            
                StartCoroutine(delayCall.Start());
                return delayCall;
            }
            else
            {
                return null;
            }          

        }

        ///타임아웃 대기
        public IEnumerator crt_waitWWWTimeout(WWW www, float fTimeout)
        {
            float sum = 0.0f;
            while (!www.isDone && string.IsNullOrEmpty(www.error) && sum < fTimeout) ///타임아웃 대기
            {
                sum += Time.deltaTime;
                yield return null;
            }

        }
    }
}