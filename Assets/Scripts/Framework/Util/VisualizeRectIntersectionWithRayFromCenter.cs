using UnityEngine;
using System;
using System.Collections.Generic;

public class VisualizeRectIntersectionWithRayFromCenter : MonoBehaviour 
{
    [SerializeField]
    Rect rect;

    [SerializeField]
    Vector2 point;



    [Serializable]
    class Colors
    {
        public Color rect, point, intersection, line;
    }







    [SerializeField]
    Colors colors;

    [SerializeField]
    Line2 line;


    //Camera m_renderCamera = null;

    void OnDrawGizmos()
    {
        Gizmos.color = colors.rect;
        Vector2[] corners = {new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMin, rect.yMax), rect.Max(), new Vector2(rect.xMax, rect.yMin)};
        
        
        int i = 0;
        while (i < 3) 
            Gizmos.DrawLine(corners[i], corners[++i]);
        
        Gizmos.DrawLine(corners[3], corners[0]);

        Gizmos.color = colors.point;
        Gizmos.DrawLine(rect.center, point);

        Gizmos.color = colors.intersection;

        // 특정 매개변수 지정시 a:b로 쓴다!! 신기방기
        Gizmos.DrawLine(rect.center, rect.IntersectionWithRayFromCenter(pointOnRay: point));


        Gizmos.color = colors.line;
        //Gizmos.DrawLine(line.start, line.end);



        //List<Vector2> ps = rect.GetIntersectionPoint(line);

        ////최종 목표는 사각형에 라인을 넣었을 때 


        ////Gizmos.DrawLine(ps[0], ps[1]);djs

        ////Gizmos.color = colors.point;
        ////Gizmos.DrawLine(ps[2], ps[3]);
        //foreach (Vector2 p in ps)
        //{
        //    Gizmos.DrawSphere(p, 1f);
        //    //Debug.Log(p);
        //}


        //List<Line2> splits = line.SplitByPointOnIt(ps);
        //foreach(Line2 split in splits)
        //{
        //    Gizmos.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
        //    Gizmos.DrawLine(split.start, split.end);
        //}

        List<Line2> splits = rect.GetLinesClippingOutside(line);
        foreach (Line2 split in splits)
        {
            Gizmos.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
            Gizmos.DrawLine(split.start, split.end);
        }

    }





    //void Start()
    //{

    //}


    //void AddLineObject(int nPayLine, List<Vector2> dots)
    //{
    //    // duplicate dots for line renderer
    //    List<Vector3> duplicatedLocalPosition = new List<Vector3>();
    //    for (int i = 0; i < dots.Count; i++)
    //    {
    //        Vector3 position = dots[i];


    //        // if i want to make line, i must pass parameter pair each lines
    //        // so i duplicate each dots except first one and last one
    //        if (i > 0 && i < dots.Count - 1)
    //        {
    //            // duplicate each dots
    //            duplicatedLocalPosition.Add(position);
    //            duplicatedLocalPosition.Add(position);
    //        }
    //        else
    //        {

    //            duplicatedLocalPosition.Add(position);
    //        }
    //    }

    //    // Convert local position to world position
    //    List<Vector3> worldPositions = new List<Vector3>();
    //    foreach (Vector3 localPosition in duplicatedLocalPosition)
    //    {
    //        Vector3 worldPosition = transform.TransformPoint(localPosition);
    //        worldPositions.Add(worldPosition);

    //        Debug.Log("World Position : " + worldPosition);
    //    }

    //    List<Vector3> viewportPositions = new List<Vector3>();
    //    foreach (Vector3 worldPosition in worldPositions)
    //    {
    //        if (m_renderCamera != null)
    //        {
    //            Vector3 viewportPosition = m_renderCamera.WorldToViewportPoint(worldPosition);
    //            viewportPositions.Add(viewportPosition);

    //            Debug.Log("View Position : " + viewportPosition);

    //        }
    //    }

    //    List<Vector2> screenPositions = new List<Vector2>();
    //    foreach (Vector3 viewportPosition in viewportPositions)
    //    {
    //        Vector2 screenPosition = new Vector2(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height);
    //        screenPositions.Add(screenPosition);

    //        Debug.Log("Screen Position : " + screenPosition);
    //    }






    //    VectorLine line = new VectorLine("PayLine" + nPayLine.ToString(), screenPositions.ToArray(), GetLineMaterial(nPayLine), 2f);
    //    line.Draw();
    //    line.vectorObject.transform.parent = transform;

    //    if (!m_dicPayLines.ContainsKey(nPayLine))
    //    {
    //        m_dicPayLines.Add(nPayLine, line);
    //    }
    //    else
    //    {
    //        m_dicPayLines[nPayLine] = line;
    //    }
    //}
}
