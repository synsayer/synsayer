using UnityEngine;
public class Symbol : MonoBehaviourCache
{
    #region Member
    public int m_nSymbolId;
    public int m_nState;

    // Just used for debuging, Any other can remove this variable. - paul
    public int index;

	protected Sprite m_oSprite;

	protected bool forceHide = false;
    #endregion
    #region Reserve
    virtual protected void Awake()
    {
		if (m_oSprite == null)
        {
			m_oSprite = GetComponent<Sprite>();
        }	
    }
    #endregion
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
//        NGUITools.MarkParentAsChanged(gameObject); 
    }


    #region Get/Set Methods

//    public Vector2 size
//    {
//        get
//        {
//            if (m_oSprite != null)
//                return m_oSprite.localSize;
//            return Vector2.zero;
//        }
//    }
//
//    public float height
//    {
//        get { return size.y; }
//    }
//
//
//    public float localTop
//    {
//        get
//        {
//            if (m_oSprite != null)
//            {
//                switch(m_oSprite.pivot)
//                {
//                    case UIWidget.Pivot.Top:
//                    case UIWidget.Pivot.TopLeft:
//                    case UIWidget.Pivot.TopRight:
//                        return transform.localPosition.y;
//                    case UIWidget.Pivot.Left:
//                    case UIWidget.Pivot.Center:
//                    case UIWidget.Pivot.Right:
//                        return transform.localPosition.y + height / 2f;
//                    case UIWidget.Pivot.BottomLeft:
//                    case UIWidget.Pivot.Bottom:
//                    case UIWidget.Pivot.BottomRight:
//                        return transform.localPosition.y + height;
//                }
//            }
//
//            return 0f;
//        }
//        set
//        {
//            float fAdjust = 0f;
//
//            if (m_oSprite != null)
//            {
//                switch (m_oSprite.pivot)
//                {
//                    case UIWidget.Pivot.Top:
//                    case UIWidget.Pivot.TopLeft:
//                    case UIWidget.Pivot.TopRight:
//                        fAdjust = 0;
//                        break;
//                    case UIWidget.Pivot.Left:
//                    case UIWidget.Pivot.Center:
//                    case UIWidget.Pivot.Right:
//                        fAdjust = height / 2f;
//                        break;
//                    case UIWidget.Pivot.BottomLeft:
//                    case UIWidget.Pivot.Bottom:
//                    case UIWidget.Pivot.BottomRight:
//                        fAdjust = height;
//                        break;
//                }
//            }
//
//
//            transform.localPosition = new Vector3(transform.localPosition.x, value - fAdjust, transform.localPosition.z);
//            //transform.localPosition += Vector3.up * (value - fAdjust); 
//        }
//    }
//
//    public float localBottom
//    {
//        get
//        {
//            if (m_oSprite != null)
//            {
//                switch (m_oSprite.pivot)
//                {
//                    case UIWidget.Pivot.Top:
//                    case UIWidget.Pivot.TopLeft:
//                    case UIWidget.Pivot.TopRight:
//                        return transform.localPosition.y - height;
//                    case UIWidget.Pivot.Left:
//                    case UIWidget.Pivot.Center:
//                    case UIWidget.Pivot.Right:
//                        return transform.localPosition.y - height / 2f;
//                    case UIWidget.Pivot.BottomLeft:
//                    case UIWidget.Pivot.Bottom:
//                    case UIWidget.Pivot.BottomRight:
//                        return transform.localPosition.y;
//                }
//            }
//            return 0f;
//        }
//        set
//        {
//            float fAdjust = 0f;
//
//            if (m_oSprite != null)
//            {
//                switch (m_oSprite.pivot)
//                {
//                    case UIWidget.Pivot.Top:
//                    case UIWidget.Pivot.TopLeft:
//                    case UIWidget.Pivot.TopRight:
//                        fAdjust = height;
//                        break;
//                    case UIWidget.Pivot.Left:
//                    case UIWidget.Pivot.Center:
//                    case UIWidget.Pivot.Right:
//                        fAdjust = height / 2f;
//                        break;
//                    case UIWidget.Pivot.BottomLeft:
//                    case UIWidget.Pivot.Bottom:
//                    case UIWidget.Pivot.BottomRight:
//                        fAdjust = 0;
//                        break;
//                }
//            }
//
//
//            transform.localPosition = new Vector3(transform.localPosition.x, value + fAdjust, transform.localPosition.z);
//            //transform.localPosition += Vector3.up * (value + fAdjust); 
//        }
//    }
//
//
//    public bool visible
//    {
//        get 
//        {
//			m_oSprite.v
//            if (m_oSprite != null)
//                return m_oSprite.enabled;
//            return false;
//        }
//        set
//        {
//			if (forceHide) return;
//
//			if (m_oSprite != null)
//				m_oSprite.enabled = value;
//        }
//    }
//
//
//    public int depth
//    {
//        get
//        {
//            if (m_oSprite != null)
//                return m_oSprite.depth;
//            return 0;
//        }
//        set
//        {
//            if (m_oSprite != null)
//                m_oSprite.depth = value;
//        }
//    }
//    #endregion
//    public Color color
//    {
//        get
//        {
//            if (m_oSprite != null)
//                return m_oSprite.color;
//            return Color.white;
//        }
//        set
//        {
//            if (m_oSprite != null)
//                m_oSprite.color = value;
//        }
//    }
//
//
//    public int nSymbolId
//    {
//        get
//        {
//            return m_nSymbolId;
//        }
//    }
//
//	// 강제 (visible 예외 처리)
//	public void SetForceVisible(bool visible)
//	{		
//		m_oSprite.enabled = visible;
//		forceHide = !visible;
//	}
//
//	public float alpha
//	{
//		get { return m_oSprite.alpha; }
//		set { m_oSprite.alpha = value; }
//	}
	#endregion
}
