﻿using System;
using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

namespace FrameWork.Util
{
	class MathUtil
	{
        //180 넘어서는 두벡터의 각도 구하기 
        static public float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
        {
            // angle in [0,180]
            float angle = Vector3.Angle(a, b);
            float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

            // angle in [-179,180]
            float signed_angle = angle * sign;

            // angle in [0,360] (not used but included here for completeness)
            //float angle360 =  (signed_angle + 180) % 360;

            return signed_angle;
        }

	}
}
