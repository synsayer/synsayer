using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public partial class LocalPrefs
{
	public class Game : LocalPrefsContainer
	{
		public string	m_strGameLanguage					= "English";	// English, Korean....

		// 홈 -> 테이블게임 초기 세팅에 사용되는 값들.
		public bool		m_bTexasHoldemIsRecommend			= false;	// 텍사스 홀덤 Recommended 설정 여부.
		public double	m_fTexasHoldemSliderValue			= 0f;		// 텍사스 홀덤 슬라이더 값.
		public int		m_iTexasHoldemPlayTable				= 0;		// 텍사스 홀덤 플레이 테이블. (몇인용인지.)
		public int		m_iTexasHoldemChannelCode			= 0;		

		public bool		m_bPineAppleHoldemIsRecommend		= false;	// 파인애플 홀덤.
		public int		m_iPineAppleHoldemPlayTable			= 0;
		public double	m_fPineAppleHoldemSliderValue		= 0f;
		public int		m_iPineAppleHoldemChannelCode		= 0;

		public bool		m_bBlackjackIsRecommend				= false;	// 블랙잭.
		public double	m_fBlackjackSliderValue				= 0f;
		public int		m_iBlackjackPlayTable				= 0;
		public int		m_iBlackjackChannelCode				= 0;


		// 플래쉬에 있던 로컬 값들.
		public string	m_strShoppingHolic					= string.Empty;
		public string	m_strFlagOfHoldForAutoSpinVisible	= string.Empty;
		public string	m_strBellClassicCoinSize			= string.Empty;
		public string	m_strBellClassic					= string.Empty;
		public string	m_strWinnerPaymentTime_null			= string.Empty;
		public string	m_strNoti2							= string.Empty;
		public string	m_strWinnerPaymentTime				= string.Empty;
	}
}
