#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

public class LocalPrefsViewer : EditorWindow
{
	static LocalPrefsViewer	instance;
	static bool[]			categoryFold;
	Vector2					scroll;

	
	public static void Open()
	{
		instance	= GetWindow<LocalPrefsViewer>( false, "Prefs Viewer", true );
		instance.Show();
	}

	public static void Refresh()
	{
		if( instance != null )
		{
			instance.Repaint();
		}
	}

	void OnGUI()
	{
		Type		prefsBase	= typeof( LocalPrefs );
		FieldInfo	dicField	= prefsBase.GetField( "s_dicCategory", BindingFlags.Static | BindingFlags.NonPublic );
		var			dic			= dicField.GetValue( null ) as Dictionary<Type, LocalPrefs.LocalPrefsContainer>;
		
		Type[]		nested		= prefsBase.GetNestedTypes();
		if( categoryFold == null )
		{
			categoryFold		= new bool[ nested.Length ];
		}

		EditorGUILayout.HelpBox( "\n　Current Local Preference Value List\n", MessageType.None );
		scroll	= EditorGUILayout.BeginScrollView( scroll );
		for(int i=0; i<nested.Length; i++)
		{
			if( nested[i].Name.Equals( typeof(LocalPrefs.LocalPrefsContainer).Name ) == true )
			{
				continue;
			}

			EditorGUILayout.Space();
			if( categoryFold[i]	= EditorGUILayout.Foldout( categoryFold[i], nested[i].Name ) )
			{
				var			container	= LocalPrefs.Get( nested[i] ) as LocalPrefs.LocalPrefsContainer;
				FieldInfo[]	fields		= container.GetType().GetFields();
				for(int j=0; j<fields.Length; j++)
				{
					GUI.color	= Color.white;
					EditorGUILayout.BeginHorizontal();
					
					{
						string	val	= fields[j].GetValue( container ).ToString();
						EditorGUILayout.LabelField( "　" + fields[j].Name );

						if( string.IsNullOrEmpty( val ) == true )
						{
							GUI.color	= new Color( .8f, .8f, .8f );
						}
						EditorGUILayout.TextField( val );
					}
					
					EditorGUILayout.EndHorizontal();
				}
			}
		}
		EditorGUILayout.EndScrollView();
	}	
}
#endif