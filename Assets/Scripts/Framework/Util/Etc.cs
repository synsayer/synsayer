using System;
using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

namespace FrameWork.Util
{
	class Etc
	{

        public static  DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string URLAddData(string strURL, string key, string data)
        {
            if (strURL.IndexOf("?") < 0)
            {
                strURL += "?";
            }
            else
            {
                strURL += "&";
            }
            strURL += key + "=" + data;
            return strURL;
        }


        public static string URLWithNoCache(string strURL)
        {
            return URLAddData(strURL, "nocache", UnityEngine.Random.Range(1, Int32.MaxValue).ToString()); 
        }

        //자식(손자)들 중에서 첫번째로 발견되는 Component를 찾아준다.
        public static T FindComponentRecursively<T>(Transform trasnform) where T : UnityEngine.Behaviour
        {
            T animator = trasnform.GetComponent<T>() as T;
            if (animator != null)
                return animator;

            for (int i = 0; i < trasnform.childCount; i++)
            {
                T a = FindComponentRecursively<T>(trasnform.GetChild(i));
                if (a != null)
                {
                    return a;    
                }
            }

            return null;
        }


        //자식(손자)Layer 바꾸기
        public static void ChangeLayersRecursively(Transform trans, int nlayer)
        {
            trans.gameObject.layer = nlayer;
            foreach (Transform child in trans)
            {
                ChangeLayersRecursively(child, nlayer);
            }
        }

	}
}
