using System.Collections.Generic;
using UnityEngine;
namespace UserDefine
{
	public struct ReelInfo
	{
		public List<int> lstSymbolId;
	}

	public struct StopSymbol
	{
		public int m_nSymbolId;
		public int m_nState;
	}


	public struct TweeningProperty
	{
		public float x0;  // drag
		public float x1;    // speed up duration
		public float x2; // max speed duration
		public float x3; // slow down speed duration
		public float x4;   // bounce
		public float z;
		public float s0;
		public float s1;
		public float s2;
		public float s3;
		public float s4;       // bounce
		public float a;
		public float a2;
		public float t;        // total time
		public float lastPosY;
		public State curState;
		public float x2Bonus;   // Scatter 가능 이펙트가 나오는 동안 더 오래 돌기 위한 값
		public float x2Correction;	// 업데이트를 마쳤을 때 심볼이 정수개만큼 딱 맞춰서 이동할 수 있게 보정해주는 값

		public void Init()
		{
			x0 = 0f;  // drag
			x1 = 0f;    // speed up duration
			x2 = 1f; // max speed duration
			x3 = 0; // slow down speed duration
			x4 = 0;   // bounce
			z = 0;
			s0 = 0;
			s1 = 0;
			s2 = 0;
			s3 = 0;
			s4 = 0;       // bounce
			a = 0;
			a2 = 0;
			t = 0;        // total time
			lastPosY = 0;
			curState = State.Wait;
			x2Bonus = 0;
			x2Correction = 0;
		}

		public void InitSpinTime(float fSymbolHeight, float fFrameStart, float fFrameEnd, float fMaxSpeed, float fEndSpeed)
		{
			// 시작 후 뒤로 땡기는 거리, 시간
			s0 = fSymbolHeight * fFrameStart;
			x0 = s0 / fMaxSpeed;

			// 종료 후 릴 바운스 거리, 시간
			s4 = fSymbolHeight * fFrameEnd;
			x4 = s4 / fEndSpeed;

			// 릴 위치 계산용
			if (x1 > 0)
				a = -fMaxSpeed / (x1 * x1);
			if (x3 > 0)
				a2 = -(fMaxSpeed - z) / (x3 * x3);

			// x1 구간에서의 이동 거리
			s1 = 0;
			if (x1 > 0)
			{
				s1 = a / 3 * (x1 * x1 * x1) + x1 * fMaxSpeed;
			}

			// x2 구간에서의 이동 거리
			s2 = fMaxSpeed * (x2 + x2Bonus);
			//s2 = m_fMaxSpeed * (x2 + x2Bonus) + s0;	// 뒤로 땡긴 만큼 보정

			// x3 구간에서의 이동 거리
			s3 = 0;
			if (x3 > 0)
			{
				s3 = ((a2 * (x3 * x3 * x3)) / 3) + fMaxSpeed * x3;
			}
		}

		public float GetTotalS()
		{
			return s1 + s2 + s3;
		}


		public float GetYPosition(float fMaxSpeed, float fEndSpeed)
		{
			switch (curState)
			{
			case State.SpinStart:
				{
					return t * -fMaxSpeed;
				}
			case State.SpinSpeedUp:
				{
					float t1 = t - x0;
					return -s0 + 1f / 3f * t1 * (a * (3f * x1 * x1 - 3f * x1 * t1 + t1 * t1) + 3f * fMaxSpeed);
				}
			case State.Spin:
				{
					return s1 + fMaxSpeed * (t - x1 - x0) - s0;
				}
			case State.SpinSlow:
				{
					float t3 = t - x0 - x1 - x2 - x2Bonus - x2Correction;
					return s1 + s2 + (a2 * (t3 * t3 * t3)) / 3f + fMaxSpeed * t3;
				}
			case State.SpinBounceDown:
				{
					float t4 = t - (x0 + x1 + x2 + x2Bonus + x2Correction + x3);
					return s1 + s2 + s3 + (t4 * fEndSpeed);
				}
			case State.SpinBounceUp:
				{
					float t4 = t - (x0 + x1 + x2 + x2Bonus + x2Correction + x3 + x4);
					return s1 + s2 + s3 + s4 - (t4 * fEndSpeed);
				}
			case State.SpinEnd:
				{
					return s1 + s2 + s3;
				}
			}
			return 0;
		}

		public void Update()
		{
			t += Time.deltaTime;

			if (x0 > 0 && t <= x0)
			{
				Debug.Log("E");
			}
			else if (x1 > 0 && t <= x0 + x1)    // speed up
			{

				curState = State.SpinSpeedUp;
			}
			else if (t <= x0 + x1 + x2 + x2Bonus + x2Correction)        // max speed
			{
				if (curState != State.Spin)
				{
					curState = State.Spin;
				}
			}
			else if (x3 > 0 && t <= x0 + x1 + x2 + x2Bonus + x2Correction + x3)     // slow down
			{
				if (curState != State.SpinSlow)
				{
					curState = State.SpinSlow;
				}
			}
			else if (t <= x0 + x1 + x2 + x2Bonus + x2Correction + x3 + x4)
			{
				curState = State.SpinBounceDown;
			}
			else if (t <= x0 + x1 + x2 + x2Bonus + x2Correction + x3 + x4 + x4)
			{
				curState = State.SpinBounceUp;
			}
			else if (t > x0 + x1 + x2 + x2Bonus + x2Correction + x3 + x4 + x4)
			{
				curState = State.SpinEnd;
			}
		}

	}
}



