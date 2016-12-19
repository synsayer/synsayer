//using UnityEngine;
using System.Collections.Generic;

public static class StringExtension 
{
    public static string GetLast(this string source, int tail_length)
    {
        if (tail_length >= source.Length)
            return source;
        return source.Substring(source.Length - tail_length);
    }

    public static int ToInt(this string source)
    {
        int nReturnVal = 0;
        bool bCanConvert = int.TryParse(source, out nReturnVal);
        if (bCanConvert)
            return nReturnVal;
        return 0;
    }

    public static uint ToUInt(this string source)
    {

        uint uiReturnVal = 0;

        bool bCanConvert = uint.TryParse(source, out uiReturnVal);
        if (bCanConvert)
            return uiReturnVal;
        return 0;
    }

    public static long ToLong(this string source)
    {
        long llReturnVal = 0;

        bool bCanConvert = long.TryParse(source, out llReturnVal);
        if (bCanConvert)
            return llReturnVal;
        return 0;
    }

    public static string GetExtentionFromPath(this string source)
    {
        string[] strSplitWithDot = source.Split('.');

        if (strSplitWithDot.Length > 1)
        {
            string strLastSplit = strSplitWithDot[strSplitWithDot.Length - 1];
            return strLastSplit;
        }

        return "";
    }


    public static string GetFilenameFromPath(this string source)
    {
        string[] strSplitWithDot = source.Split('.');

        if (strSplitWithDot.Length > 1)
        {

            List<string> lstFilename = new List<string>(strSplitWithDot);
            lstFilename.RemoveAt(lstFilename.Count - 1);
            strSplitWithDot = lstFilename.ToArray();


            return string.Join(".", strSplitWithDot);
        }

        return "";
    }
}
