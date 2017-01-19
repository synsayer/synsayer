using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Random = UnityEngine.Random;
using TweenParams = Holoville.HOTween.TweenParms;
using TweenCallback = Holoville.HOTween.Core.TweenDelegate.TweenCallback;

public class SymbolTweenerParam
{
	public Vector3?			gs_vMoveFrom		{ get; private set; }
	public Vector3?			gs_vMoveTo			{ get; private set; }
	public Vector3?			gs_vScaleFrom		{ get; private set; }	
	public Vector3?			gs_vScaleTo			{ get; private set; }
	public Vector3?			gs_vRotateFrom		{ get; private set; }
	public Vector3?			gs_vRotateTo		{ get; private set; }
	public EaseType			gs_eEaseType		{ get; private set; }
	public AnimationCurve	gs_oAnimCurve		{ get; private set; }
	public float?			gs_fDuration		{ get; private set; }
	public float			gs_fDelay			{ get; private set; }
	public bool				gs_bWorldSpace		{ get; private set; }
	public TweenCallback	m_onFinish;
	
	public SymbolTweenerParam()
	{
		gs_vMoveFrom		= null;
		gs_vMoveTo			= null;
		gs_vScaleFrom		= null;	
		gs_vScaleTo			= null;
		gs_vRotateFrom		= null;
		gs_vRotateTo		= null;
		gs_eEaseType		= Holoville.HOTween.EaseType.Linear;
		gs_oAnimCurve		= null;
		gs_fDuration		= null;
		gs_fDelay			= 0f;
		gs_bWorldSpace		= true;
	}

	#region 00. MoveFrom&To: Tween 대상의 시작&도착 위치 지정.
	public SymbolTweenerParam MoveFrom(Transform _oTarget)
	{
		MoveFrom( _oTarget.position );
		return this;
	}

	public SymbolTweenerParam MoveFrom(Vector3 _vTarget)
	{
		gs_vMoveFrom	= _vTarget;
		return this;
	}

	public SymbolTweenerParam MoveTo(Transform _oTarget)
	{
		MoveTo( _oTarget.position );
		return this;
	}

	public SymbolTweenerParam MoveTo(Vector3 _vTarget)
	{
		gs_vMoveTo	= _vTarget;
		return this;
	}
	#endregion


	#region 01. ScaleFrom&To: Tween 대상의 시작&목표 크기 지정.
	public SymbolTweenerParam ScaleFrom(Transform _oTarget)
	{
		ScaleFrom( _oTarget.localScale );
		return this;
	}

	public SymbolTweenerParam ScaleFrom(Vector3 _vTarget)
	{
		gs_vScaleFrom	= _vTarget;
		return this;
	}

	public SymbolTweenerParam ScaleTo(Transform _oTarget)
	{
		ScaleTo( _oTarget.localScale );
		return this;
	}

	public SymbolTweenerParam ScaleTo(Vector3 _vTarget)
	{
		gs_vScaleTo	= _vTarget;
		return this;
	}
	#endregion


	#region 02. RotateFrom&To: Tween 대상의 시작&목표 회전량 지정.
	public SymbolTweenerParam RotateFrom(Transform _oTarget)
	{
		RotateFrom( _oTarget.eulerAngles );
		return this;
	}
	
	public SymbolTweenerParam RotateFrom(Vector3 _vTarget)
	{
		gs_vRotateFrom	= _vTarget;
		return this;
	}

	public SymbolTweenerParam RotateTo(Transform _oTarget)
	{
		RotateTo( _oTarget.eulerAngles );
		return this;
	}
	
	public SymbolTweenerParam RotateTo(Vector3 _vTarget)
	{
		gs_vRotateTo	= _vTarget;
		return this;
	}
	#endregion


	#region 03. ETC
	/// <summary>
	/// Tween 지속시간.
	/// </summary>
	public SymbolTweenerParam Duration(float _f)
	{
		gs_fDuration	= _f;
		return this;
	}

	/// <summary>
	/// Tween 시작전 딜레이.
	/// </summary>
	public SymbolTweenerParam Delay(float _f)
	{
		gs_fDelay		= _f;
		return this;
	}

	/// <summary>
	/// Tween의 움직임 방식.
	/// </summary>
	public SymbolTweenerParam EaseType(EaseType _eType)
	{
		gs_eEaseType	= _eType;
		return this;
	}


	/// <summary>
	/// Tween에 움직임 방식.
	/// </summary>
	public SymbolTweenerParam EaseType(AnimationCurve _oCurve)
	{
		gs_oAnimCurve	= _oCurve;
		return this;
	}
	

	/// <summary>
	/// Tween 이동시 월드/로컬 공간 사용 결정.
	/// </summary>
	public SymbolTweenerParam UseWorldSpace(bool _b)
	{
		gs_bWorldSpace	= _b;
		return this;
	}
	#endregion
}


public class SymbolTweener
{
	public SymbolTweenerParam	gs_oParam	{ get; private set; }
	Transform					m_oOwner;

	public SymbolTweener(Transform _oOwner)
	{
		gs_oParam	= new SymbolTweenerParam();
		m_oOwner	= _oOwner;
	}

	/// <summary>
	/// Tweener 플레이~!
	/// </summary>
	public void Play(SymbolTweenerParam _oParam, Action _onFinish=null)
	{
		gs_oParam	= _oParam;
		gs_oParam.m_onFinish	= () =>
		{
			if( _onFinish != null )
			{
				_onFinish();
			}
		};

		MoveTo();
		ScaleTo();
		RotateTo();
	}


	/// <summary>
	/// Move Tween
	/// </summary>
	void MoveTo()
	{
		if( gs_oParam.gs_vMoveFrom == null )
		{
			bool	worldSpace	= gs_oParam.gs_bWorldSpace;
			if( worldSpace == true )
			{
				gs_oParam.MoveFrom( m_oOwner.position );
			}
			else
			{
				gs_oParam.MoveFrom( m_oOwner.localPosition );
			}
		}

		if( gs_oParam.gs_vMoveTo == null || gs_oParam.gs_fDuration == null )
		{
			return;
		}

		string		propTarget	= string.Empty;
		if( gs_oParam.gs_bWorldSpace == true )
		{
			m_oOwner.position		= gs_oParam.gs_vMoveFrom.Value;
			propTarget				= "position";
		}
		else
		{
			m_oOwner.localPosition	= gs_oParam.gs_vMoveFrom.Value;
			propTarget				= "localPosition";
		}
		
		TweenParams	param	= new TweenParams().Prop( propTarget, gs_oParam.gs_vMoveTo.Value, false ).
												Delay( gs_oParam.gs_fDelay ).
												OnStepComplete( gs_oParam.m_onFinish ).
												AutoKill( true );
		AddEaseTypeOrAnimCurve( param );

		HOTween.To( m_oOwner, gs_oParam.gs_fDuration.Value, param );
	}


	/// <summary>
	/// Scale Tween
	/// </summary>
	void ScaleTo()
	{
		if( gs_oParam.gs_vScaleFrom == null )
		{
			gs_oParam.ScaleFrom( m_oOwner );
		}

		if( gs_oParam.gs_vScaleTo == null || gs_oParam.gs_fDuration == null )
		{
			return;
		}

		m_oOwner.localScale	= gs_oParam.gs_vScaleFrom.Value;
		TweenParams	param	= new TweenParams().Prop( "localScale", gs_oParam.gs_vScaleTo.Value, false ).
												Delay( gs_oParam.gs_fDelay ).
												OnStepComplete( gs_oParam.m_onFinish ).
												AutoKill( true );
		AddEaseTypeOrAnimCurve( param );	
		
		HOTween.To( m_oOwner, gs_oParam.gs_fDuration.Value, param );
	}


	/// <summary>
	/// Rotate Tween
	/// </summary>
	void RotateTo()
	{
		if( gs_oParam.gs_vScaleFrom == null )
		{
			gs_oParam.ScaleFrom( m_oOwner );
		}

		if( gs_oParam.gs_vRotateTo == null || gs_oParam.gs_fDuration == null )
		{
			return;
		}

		m_oOwner.localEulerAngles	= gs_oParam.gs_vRotateFrom.Value;
		TweenParams		param		= new TweenParams().Prop( "rotation", gs_oParam.gs_vRotateTo.Value, false ).
														Delay( gs_oParam.gs_fDelay ).
														OnStepComplete( gs_oParam.m_onFinish ).
														AutoKill( true );
		AddEaseTypeOrAnimCurve( param );

		HOTween.To( m_oOwner, gs_oParam.gs_fDuration.Value, param );
	}


	void AddEaseTypeOrAnimCurve(TweenParams _oParam)
	{
		if( gs_oParam.gs_oAnimCurve != null  )
		{
			if(  gs_oParam.gs_oAnimCurve.keys.Length > 0 )
			{
				_oParam.Ease( gs_oParam.gs_oAnimCurve );
			}
		}
		else
		{
			_oParam.Ease( gs_oParam.gs_eEaseType );
		}
	}
}