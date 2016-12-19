// Made by Paul Shin at Dadasoft
//using System.Collections;
using System.Collections.Generic;

public class Reel : IReel
{
	#region Member
	List<int> m_nTurnTableOfSymbolId;
	int m_nStylusOfSymbolIndex;
	#endregion

	#region Initialize Methods
	public Reel()
	{
		m_nTurnTableOfSymbolId = new List<int>();
		m_nStylusOfSymbolIndex = 0;
	}

	public Reel(List<int> lstSource)
	{
		m_nTurnTableOfSymbolId = new List<int>();
		m_nStylusOfSymbolIndex = 0;

		Initialize(lstSource);
	}

	public void Initialize(List<int> lstSource)
	{
		m_nTurnTableOfSymbolId.Clear();
		m_nTurnTableOfSymbolId.AddRange(lstSource);

		m_nStylusOfSymbolIndex = 0;
	}

	#endregion Initialize Methods

	#region Utility Methods

	public int GetSymbolId(int nIndex)
	{
		if (nSymbolCount > 0 && nIndex >= 0)
		{
			return m_nTurnTableOfSymbolId[GetTurnTableIndex(nIndex)];
		}
		return 0;
	}

	public int GetTurnTableIndex(int nIndex)
	{
		if (nSymbolCount > 0 && nIndex >= 0)
		{
			return nIndex % nSymbolCount;
		}
		return 0;
	}

	public List<int> GetReelSnapShot(int nStopPositionIndex, int nVisibleRow = 3)
	{
		List<int> snapShot = new List<int>();

		if (nSymbolCount > 0 && nStopPositionIndex >= 0)
		{
			for (int i = 0; i < nVisibleRow; i++)
			{
				snapShot.Add(m_nTurnTableOfSymbolId[nStopPositionIndex]);

				nStopPositionIndex++;

				if (nStopPositionIndex >= nSymbolCount)
					nStopPositionIndex = 0;
			}
		}

		return snapShot;
	}

	public int GetNextSymbolIdExcept(int nSpecificId)
	{
		int nSymbolId = nSpecificId;
		//int nSearchCnt = nSymbolCount;
		int nSearchCnt = 0;

		/*
		while (nSymbolId == nSpecificId)
		{
			if (nSearchCnt < 0)
				return nSymbolId;

			nSymbolId = nNextSymbolId;
			nSearchCnt--;
		}
		 */

		while (nSymbolId == nSpecificId && nSearchCnt < nSymbolCount)
		{
			nSymbolId = m_nTurnTableOfSymbolId[nStylus + nSearchCnt];

			nSearchCnt++;
			if (nSearchCnt >= nSymbolCount)
				break;
		}

		nStylus--;

		return nSymbolId;
	}

	//List<int> GetReelSnapShot(int nStopPositionIndex)
	//{
	//    List<int> snapShot = new List<int>();

	//    return snapShot;
	//}

	#endregion Utility Methods

	#region Get/Set Methods

	public int nStylus
	{
		get { return m_nStylusOfSymbolIndex; }
		set
		{
			if (nSymbolCount > 0)
			{
				m_nStylusOfSymbolIndex = value;

				if (m_nStylusOfSymbolIndex < 0)
				{
					m_nStylusOfSymbolIndex = nSymbolCount - 1;
				}
				else
				{
					m_nStylusOfSymbolIndex %= nSymbolCount;
				}
			}
			else
				m_nStylusOfSymbolIndex = 0;
		}
	}

	public int nNextSymbolId
	{
		get
		{
			int nSymbolId = 0;
			if (nSymbolCount > 0)
			{
				nSymbolId = m_nTurnTableOfSymbolId[nStylus];
			}

			nStylus--;
			return nSymbolId;
		}
	}

	public int nSymbolCount
	{
		get { return m_nTurnTableOfSymbolId.Count; }
	}

	public List<int> lstSource
	{
		get { return m_nTurnTableOfSymbolId; }
	}

	#endregion Get/Set Methods
}