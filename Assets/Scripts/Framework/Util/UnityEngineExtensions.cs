//////////////////////////////////////////////
//      Geometry Extensions
// Copyright 2015-2025 Dadasoft
// Made by Paul Shin
////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{


    //public static class GameObjectExtensions
    //{
    //    /// <summary>
    //    /// Checks if a GameObject has been destroyed.
    //    /// </summary>
    //    /// <param name="gameObject">GameObject reference to check for destructedness</param>
    //    /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
    //    public static bool IsDestroyed(this GameObject gameObject)
    //    {
    //        // UnityEngine overloads the == opeator for the GameObject type
    //        // and returns null when the object has been destroyed, but 
    //        // actually the object is still there but has not been cleaned up yet
    //        // if we test both we can determine if the object has been destroyed.
    //        return gameObject == null && !ReferenceEquals(gameObject, null);
    //    }
    //}




    [System.Serializable]
    public struct Line2
    {
        public Vector2 start;
        public Vector2 end;

        public Line2(Vector2 _start, Vector2 _end)
        {
            start = _start;
            end = _end;
        }

        public Line2(float x1, float y1, float x2, float y2)
        {
            start = new Vector2(x1,y1);
            end = new Vector2(x2, y2);
        }

        public Vector2 GetIntersectionPoint(Line2 line)
        {
            Vector2 p = Vector2.zero;

            float x1, y1, x2, y2, x3, y3, x4, y4;
            x1 = start.x;
            y1 = start.y;
            x2 = end.x;
            y2 = end.y;

            x3 = line.start.x;
            y3 = line.start.y;
            x4 = line.end.x;
            y4 = line.end.y;

            float distance = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            // parelles
            if(distance != 0)
            {
                p.x = ((x3 - x4) * (x1 * y2 - y1 * x2) - (x1 - x2) * (x3 * y4 - y3 * x4)) / distance;
                p.y = ((y3 - y4) * (x1 * y2 - y1 * x2) - (y1 - y2) * (x3 * y4 - y3 * x4)) / distance;
            }

            return p;
        }

        public Vector2 mid
        {
            get { return new Vector2((start.x + end.x) / 2f, (start.y + end.y) / 2f); }
        }

        //두 점을 있는 선 전체에서 포인트 포함 여부 판독
        public bool ContainPoint(Vector2 point)
        {
            const float EPSILON = .001f;

            if (System.Math.Abs(start.x - end.x) < EPSILON)
            {
                // if vertical line, check x val only
                return System.Math.Abs(point.x - start.x) < EPSILON;
            }

            float m = (end.y - start.y) / (end.x - start.x);
            float b = start.y - m * start.x;

            return System.Math.Abs(point.y - (m * point.x + b)) < EPSILON;
        }

        public List<Line2> SplitByPointOnIt(List<Vector2> points)
        {

            //List<Vector2> vaildPoints = new List<Vector2>();
            Dictionary<float, Vector2> dicPointByDist = new Dictionary<float, Vector2>();
            foreach(Vector2 point in points)
            {
                if(ContainPointInSegment(point))
                {
                    //vaildPoints.Add(point);

                    float d = Vector2.Distance(start, point);
                    dicPointByDist.Add(d, point);
                }
            }


            var items = from pair in dicPointByDist orderby pair.Key ascending select pair;
            
            List<Line2> splits = new List<Line2>();


            Vector2 probe = start;
            foreach(KeyValuePair<float, Vector2> pair in items)
            {
                Line2 line = new Line2();

                line.start = probe;
                line.end = pair.Value;
                splits.Add(line);

                probe = pair.Value;
            }

            splits.Add(new Line2(probe, end));

            return splits;
        }

        public Rect GetBounds(float epsilon = 0f)
        {
            float xMin, xMax, yMin, yMax, width, height;
            
            if(start.x > end.x)
            {
                xMax = start.x;
                xMin = end.x;
            }
            else
            {
                xMin = start.x;
                xMax = end.x;
            }


            if (start.y > end.y)
            {
                yMax = start.y;
                yMin = end.y;
            }
            else
            {
                yMin = start.y;
                yMax = end.y;
            }

            width = xMax - xMin;
            height = yMax - yMin;

            return new Rect(xMin - epsilon, yMin - epsilon, width + 2 * epsilon, height + 2 * epsilon);
        }

        //주어진 길이의 선에서 포인트 포함 여부 판독
        public bool ContainPointInSegment(Vector2 point)
        {
            const float EPSILON = .1f;

            if (ContainPoint(point))
            {
                Rect bounds = GetBounds(EPSILON);
                return bounds.Contains(point);
            }

            return false;
        }


        public bool IsParallel(Line2 line)
        {
            float x1, y1, x2, y2, x3, y3, x4, y4;
            x1 = start.x;
            y1 = start.y;
            x2 = end.x;
            y2 = end.y;

            x3 = line.start.x;
            y3 = line.start.y;
            x4 = line.end.x;
            y4 = line.end.y;

            float distance = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            // if distance is zero, then line are parallel
            return distance == 0;
        }
    }


    public struct Line3
    {
        public Vector3 start;
        public Vector3 end;
    }



    
}



public static class UnityEngineExtensions 
{
    public static List<Line2> GetOutLines(this Rect rect)
    {
        List<Line2> lines = new List<Line2>();

        lines.Add(new Line2(rect.min, new Vector2(rect.xMin, rect.yMax)));  // left side
        lines.Add(new Line2(new Vector2(rect.xMax, rect.yMin), rect.max));  // right side
        lines.Add(new Line2(rect.min, new Vector2(rect.xMax, rect.yMin)));  // bottom side
        lines.Add(new Line2(new Vector2(rect.xMin, rect.yMax), rect.max));  // top side

        return lines;
    }

    public static List<Vector2> GetOutLinePositions(this Rect rect)
    {
        List<Vector2> dots = new List<Vector2>();

        dots.Add(rect.min);
        dots.Add(new Vector2(rect.xMin, rect.yMax));
        dots.Add(rect.max);
        dots.Add(new Vector2(rect.xMax, rect.yMin));

        return dots;
    }

    public static List<Line2> GetLinesClippingInside(this Rect rect, Line2 line)
    {
        List<Vector2> points = rect.GetIntersectionPoint(line);
        List<Line2> splits = line.SplitByPointOnIt(points);


        List<Line2> lines = new List<Line2>();


        foreach(Line2 split in splits)
        {
            if(rect.Contains(split.mid))
            {
                lines.Add(split);
            }
            
        }
        return lines;
        
    }

    public static List<Line2> GetLinesClippingOutside(this Rect rect, Line2 line)
    {
        List<Vector2> points = rect.GetIntersectionPoint(line);
        List<Line2> splits = line.SplitByPointOnIt(points);


        List<Line2> lines = new List<Line2>();


        foreach (Line2 split in splits)
        {
            if (!rect.Contains(split.mid))
            {
                lines.Add(split);
            }

        }
        return lines;
    }


    public static List<Vector2> GetIntersectionPoint(this Rect rect, Line2 line)
    {
        List<Vector2> p = new List<Vector2>();

        // Top line
        Line2 lineTop = new Line2(new Vector2(rect.xMin, rect.yMax), rect.max);
        Vector2 topPoint = line.GetIntersectionPoint(lineTop);
        if (lineTop.ContainPointInSegment(topPoint))
        {
            p.Add(topPoint);
        }

        // Bottom line
        Line2 lineBottom = new Line2(rect.min, new Vector2(rect.xMax, rect.yMin));
        Vector2 bottomPoint = line.GetIntersectionPoint(lineBottom);
        if (lineBottom.ContainPointInSegment(bottomPoint))
        {
            p.Add(bottomPoint);
        }

        // Left side
        Line2 lineLeft = new Line2(rect.min, new Vector2(rect.xMin, rect.yMax));
        Vector2 leftPoint = line.GetIntersectionPoint(lineLeft);
        if (lineLeft.ContainPointInSegment(leftPoint))
        {
            p.Add(leftPoint);
        }

        // Right side
        Line2 lineRight = new Line2(new Vector2(rect.xMax, rect.yMin), rect.max);
        Vector2 rightPoint = line.GetIntersectionPoint(lineRight);
        if (lineRight.ContainPointInSegment(rightPoint))
        {
            p.Add(rightPoint);
        }



        return p;
    }


    public static Vector3 GetVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y);
    }

    public static Vector2 Abs(this Vector2 vector)
    {
        for (int i = 0; i < 2; ++i) vector[i] = Mathf.Abs(vector[i]);
        return vector;
    }

    public static Vector2 DividedBy(this Vector2 vector, Vector2 divisor)
    {
        for (int i = 0; i < 2; ++i) vector[i] /= divisor[i];
        return vector;
    }

    public static Vector2 Max(this Rect rect)
    {
        return new Vector2(rect.xMax, rect.yMax);
    }

    public static Vector2 IntersectionWithRayFromCenter(this Rect rect, Vector2 pointOnRay)
    {
        // 포인트 시작지점 부터 사각형 중앙 까지의 벡터
        Vector2 pointOnRay_local = pointOnRay - rect.center;

        // 사각형 중앙을 관통하는 지점으로부터 중앙까지의 거리를 전체 거리로 나눈 비율
        Vector2 edgeToRayRatios = (rect.max - rect.center).DividedBy(pointOnRay_local.Abs());

        Vector2 v2Return = Vector2.zero;

        if (edgeToRayRatios.x < edgeToRayRatios.y)
        {
            if (pointOnRay_local.x > 0)
            {
                v2Return.x = rect.xMax;
            }
            else
            {
                v2Return.x = rect.xMin;
            }

            v2Return.y = pointOnRay_local.y * edgeToRayRatios.x + rect.center.y;
        }
        else
        {
            v2Return.x = pointOnRay_local.x * edgeToRayRatios.y + rect.center.x;


            if(pointOnRay_local.y > 0)
            {
                v2Return.y = rect.yMax;
            }
            else
            {
                v2Return.y = rect.yMin;
            }
        }

        return v2Return;
    }





    


    
}
