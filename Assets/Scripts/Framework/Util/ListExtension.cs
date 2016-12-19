//using UnityEngine;
using System.Collections.Generic;

public static class ListExtension 
{

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void ShiftLeft<T>(this IList<T> list)
    {
        if (list.Count > 1)
        {
            T backup = list[0];

            for (int i = 1; i < list.Count; i++)
            {
                list[i - 1] = list[i];
            }

            list[list.Count - 1] = backup;
        }

    }

    
    public static void ShiftRight<T>(this IList<T> list)
    {
        if (list.Count > 1)
        {
            T backup = list[list.Count-1];

            for (int i = list.Count-2; i >= 0; i--)
            {
                list[i + 1] = list[i];
            }

            list[0] = backup;
        }

    }


    //public static T Pop<T>(this IList<T> list)
    //{
    //    if (list.Count > 1)
    //    {
    //        T backup = list[list.Count - 1];
    //        list.RemoveAt(list.Count - 1);
    //        return backup;
    //    }
    //    return new T();
    //}
  
}
