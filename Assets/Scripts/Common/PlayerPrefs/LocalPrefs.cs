using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public partial class LocalPrefs
{
	static Dictionary<Type, LocalPrefsContainer>	s_dicCategory;
	static readonly string							s_strLocalPrefsRoot		= "LocalPrefs/";
	
	
	static LocalPrefs()
	{
		Init();
	}

	/// <summary>
	/// 전체 초기화.
	/// </summary>
	static void Init()
	{
		s_dicCategory		= new Dictionary<Type, LocalPrefsContainer>();

		Type	thisType	= typeof( LocalPrefs );
		Type[]	nested		= thisType.GetNestedTypes();

		for(int i=0; i<nested.Length; i++)
		{
			if( nested[i].Name.Equals( "LocalPrefsContainer" ) == true )
			{
				continue;
			}

			Activator.CreateInstance( nested[i] );
		}
	}



	/// <summary>
	/// Preferenece 카테고리 리턴.
	/// </summary>
	public static T Get<T>() where T : LocalPrefsContainer
	{
		// 적절한 타입 찾아서 리턴.
		LocalPrefsContainer	container	= null;
		if( s_dicCategory.TryGetValue( typeof(T), out container ) == true )
		{
			container.Load();
			return (T)container;
		}		

		return null;
	}

	public static LocalPrefsContainer Get(Type t)
	{
		// 적절한 타입 찾아서 리턴.
		LocalPrefsContainer	container	= null;
		if( s_dicCategory.TryGetValue( t, out container ) == true )
		{
			container.Load();
			return container;
		}		

		return null;
	}
}
