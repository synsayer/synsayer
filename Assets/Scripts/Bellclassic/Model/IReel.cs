// Made by Paul Shin at Dadasoft

using UnityEngine;
using System.Collections.Generic;

public interface IReel
{
    void Initialize(List<int> lstSource);

    int GetSymbolId(int nIndex);

    int GetNextSymbolIdExcept(int nSpecificId);

    int GetTurnTableIndex(int nIndex);

    List<int> GetReelSnapShot(int nStopPositionIndex, int nVisibleRow);

    int nStylus
    {
        get;
        set;
    }

    int nNextSymbolId
    {
        get;
    }

    int nSymbolCount
    {
        get;
    }

    List<int> lstSource
    {
        get;
    }
}
