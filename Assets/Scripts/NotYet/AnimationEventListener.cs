using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimationEventListener : MonoBehaviour
{
    #region Static
    public static List<AnimationClip> s_listRegisterClips = new List<AnimationClip>();  // 이벤트가 등록된 클립들.
    /// <summary>
    /// Animator에 콜백 등록.
    /// </summary>
    public static AnimationEventListener SetListener(Animator _oAnimator)
    {
        var listener = _oAnimator.GetComponent<AnimationEventListener>();
        if (listener == null)
        {
            listener = _oAnimator.gameObject.AddComponent<AnimationEventListener>();
        }

        listener.m_oAnimator = _oAnimator;
        listener.SetAnimationEvent();

        return listener;
    }
    #endregion
    #region Member
    public Action m_onFinish;
    Animator m_oAnimator;
    #endregion
    #region Unity Reserve
    void OnDestroy()
    {
        m_onFinish = null;
        m_oAnimator = null;
    }
    #endregion
    /// <summary>
    /// 각 클립의 끝자락에 AnimationEvent 추가.
    /// 이미 이벤트가 등록된 클립은 무시함.
    /// </summary>
    void SetAnimationEvent()
    {
        if (m_oAnimator == null) return;
        if (m_oAnimator.runtimeAnimatorController == null) return;

        var clips = m_oAnimator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (s_listRegisterClips.Contains(clips[i]) == true)
            {
                continue;
            }

            AnimationEvent evt = new AnimationEvent();
            evt.functionName = "OnPlayFinish";
            evt.time = clips[i].length;

            clips[i].AddEvent(evt);
            s_listRegisterClips.Add(clips[i]);
        }
    }


    /// <summary>
    /// 재생 후 콜백.
    /// </summary>
    void OnPlayFinish()
    {
        if (m_onFinish != null)
        {
            m_onFinish();
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(AnimationEventListener))]
public class AnimationEventListenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying == false)
        {
            return;
        }

        EditorGUILayout.LabelField("이벤트가 등록된 AnimtionClip List (static)");
        GUILayout.Space(10f);
        GUI.backgroundColor = Color.green;
        var clips = AnimationEventListener.s_listRegisterClips;
        using (IEnumerator<AnimationClip> enumer = clips.GetEnumerator())
        {
            while (enumer.MoveNext())
            {
                EditorGUILayout.TextField(string.Format("   {0}", enumer.Current.name));
            }
        }
    }
}
#endif