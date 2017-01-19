using LitJson;
using System;
using System.Reflection;


public class JsonUtil{

    public static bool GetInt(JsonData jsonData, string key, ref int value)
    {
        if (false == jsonData.Keys.Contains(key))
        {
            return false;
		}

		if( null == jsonData[key] )
		{
			return false;
		}

        if (false == jsonData[key].IsInt)
        {
            return false;
        }
        value = (int)jsonData[key];
        return true;
    }

    public static bool GetDouble(JsonData jsonData, string key, ref double value)
    {
        if (false == jsonData.Keys.Contains(key))
        {
            return false;
        }

		if( null == jsonData[key] )
		{
			return false;
		}

        if (false == jsonData[key].IsDouble)
        {
            return false;
        }
        value = (double)jsonData[key];
        return true;
    }

    public static bool GetString(JsonData jsonData, string key, ref string value)
    {
        if (false == jsonData.Keys.Contains(key))
        {
            return false;
        }

		if( null == jsonData[key] )
		{
			return false;
		}

        if (false == jsonData[key].IsString)
        {
            return false;
        }
        value = (string)jsonData[key];
        return true;
    }

    public static bool GetObject(JsonData jsonData, string key, ref JsonData value)
    {
        if (false == jsonData.Keys.Contains(key))
        {
            return false;
        }

		if( null == jsonData[key] )
		{
			return false;
		}

        if (false == jsonData[key].IsObject)
        {
            return false;
        }
        value = (string)jsonData[key];
        return true;
    }

    public static bool GetLong(JsonData jsonData, string key, ref long value)
    {
        if (false == jsonData.Keys.Contains(key))
        {
            return false;
        }

		if( null == jsonData[key] )
		{
			return false;
		}

        if (false == jsonData[key].IsLong && false == jsonData[key].IsInt)
        {
            return false;
        }
        value = long.Parse(jsonData[key].ToString());
        return true;
    }

    public static void JsonToObject(object obj, string jsonString)
    {
        JsonData oJson = JsonMapper.ToObject(jsonString);
        Type objType = obj.GetType();
        FieldInfo[] fields = objType.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            _SetField(fields[i].Name, oJson, obj);
        }
    }

    public static string ObjectToJson(object obj)
    {
        return JsonMapper.ToJson(obj);
    }

    /// <summary>
    /// Json 데이터로 필드 초기화. 
    /// Json에 필드 데이터가 없으면 기본값으로 set.
    /// </summary>
    private static void _SetField(string _strFieldName, JsonData _oJson, object obj)
    {
        FieldInfo field = obj.GetType().GetField(_strFieldName);
        object outVal;

        if (_oJson == null)
        {
            return;
        }
        if (_oJson.TryGetValue(_strFieldName, field, out outVal) == true)
        {
            field.SetValue(obj, outVal);
        }
    }
}


/// <summary>
/// LitJson의 JsonData 확장확장.
/// </summary>
public static class JsonDataExtension
{
	public static bool TryGetValue(this JsonData _oData, string _strKey, out JsonData _oOutData)
	{
		_oOutData	= null;
		if( _oData.Keys.Contains( _strKey ) == true )
		{
			_oOutData	= _oData[ _strKey ];
			return true;
		}

		return false;
	}


	public static bool TryGetValue(this JsonData _oData, string _strKey, out string _strData)
	{
		_strData	= null;
		if( _oData.Keys.Contains( _strKey ) == true )
		{
			_strData	= _oData[ _strKey ].ToString();
			return true;
		}

		return false;
	}

	public static bool TryGetValue(this JsonData _oData, string _strKey, FieldInfo info, out object _outData)
	{
		_outData	= null;
		
		if( _oData.Keys.Contains( _strKey ) == true )
		{
			try
			{
				// 일반 Key/Value.
				if( _oData[ _strKey ].IsArray == false )
				{
					_outData	= Convert.ChangeType( _oData[ _strKey ].ToString(), info.FieldType );
				}

				// 배열로 되어 있음.
				else
				{
					JsonData	data	= _oData[ _strKey ];	
					if( data.Count <= 0 )
					{
						return false;
					}
					
					if( data[0].IsBoolean )
					{
						bool[]		retArr	= data.ToArray<bool>();
						_outData			= Convert.ChangeType( retArr, info.FieldType );
					}
					else if( data[0].IsDouble )
					{
						double[]	retArr	= data.ToArray<double>();
						_outData			= Convert.ChangeType( retArr, info.FieldType );
					}
					else if( data[0].IsInt )
					{
						int[]		retArr	= data.ToArray<int>();
						_outData			= Convert.ChangeType( retArr, info.FieldType );
					}
					else if( data[0].IsLong )
					{
						long[]		retArr	= data.ToArray<long>();
						_outData			= Convert.ChangeType( retArr, info.FieldType );
					}
					else if( data[0].IsString )
					{
						string[]	retArr	= data.ToArray<string>();
						_outData			= Convert.ChangeType( retArr, info.FieldType );
					}
										
					return true;
				}
			}

			// stirng을 string[]으로 캐스팅 하는등 잘못된 캐스팅.
			catch( InvalidCastException e )
			{
				UnityEngine.Debug.LogError( "JsonUtil.cs: TryGetValue() -> 잘못된 캐스팅." );
				return false;
			}

			catch( NullReferenceException e )
			{
				UnityEngine.Debug.LogWarning( string.Format( "JsonUtil.cs: TryGetValue() -> Dictionary Value is Null. Key is: {0}", _strKey) );
			}
			return true;
		}

		return false;
	}

	static T[] ToArray<T>(this JsonData _oData)
	{
		T[]	retArr	= new T[ _oData.Count ];
		for(int i=0; i<_oData.Count; i++)
		{
			retArr[i]	= (T)Convert.ChangeType( _oData[i].ToString(), typeof(T) );
		}
		
		return retArr;
	}

	public static bool ContainsKey(this JsonData _oData, string key)
	{
		JsonData	outData	= null;
		return TryGetValue( _oData, key, out outData );
	}
}
