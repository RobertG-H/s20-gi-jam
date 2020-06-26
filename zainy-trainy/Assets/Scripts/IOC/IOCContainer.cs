using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public static class IOCContainer
{
	public static bool isDebugging = false;
	private static Dictionary<Type, Dictionary<string, object>> registeredProviders = new Dictionary<Type, Dictionary<string, object>>();

	//field info for each client type. To prevent gathering initial information every time you initialize a similar object
	private static Dictionary<Type, FieldInfo[]> typeFields = new Dictionary<Type, FieldInfo[]>();


	public static void RegisterService<T>(this MonoBehaviour serviceToRegister, string key = null)
	{
		Register<T>(serviceToRegister, key);
	}

	public static void Register<T>(object serviceToRegister, string key = null) 
	{
		Register(typeof(T), serviceToRegister, key);
	}

	public static void Register(Type type, object service, string key = null)
	{
		if (!IsRegistered(type, key))
		{
			if (registeredProviders.ContainsKey(type))
			{
				registeredProviders[type].Add(key ?? string.Empty, service);
			}
			else
			{
				registeredProviders.Add(type, new Dictionary<string, object>() { { key ?? string.Empty, service } });
			}
		}
		else
			Debug.LogError("Tried to register a duplicate type and key! \n TYPE: " + type + "   KEY: \"" + key + "\"    object: " + service);
	}

	public static void UnRegister<T>(this MonoBehaviour serviceToRegister, string key = null)
	{
		UnRegister<T>(serviceToRegister, key);
	}

	public static void UnRegister<T>(object serviceToRegister, string key = null)
	{
		UnRegister(typeof(T), serviceToRegister, key);
	}

	private static void UnRegister(Type type, object service, string key)
	{
		if(IsRegistered(type, key))
		{
			registeredProviders[type].Remove(key);
		}
	}





	private static bool IsRegistered(Type t, string key)
	{
		if(registeredProviders.ContainsKey(t))
			return registeredProviders[t].ContainsKey(key);

		return false;
	}


	public static void ResolveDependencies(this MonoBehaviour client)
	{
		Resolve(client);
	}

	public static void Resolve(object client) 
	{
		Resolve(client.GetType(), client);
	}

	public static void Resolve(Type clientType, object client)
	{
		FieldInfo[] fields;

		//Searches to see if we have already parsed and prepared an object with the same client type. If so, just duplicate the same thing
		if(!typeFields.TryGetValue(clientType, out fields))
		{
			//This finds all the fields, public, nonpublic, instance 
			fields = clientType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(
					f => f.IsDefined(typeof(DependencyAttribute)
					)).ToArray();

			typeFields[clientType] = fields;

			if (fields.Length == 0)
				Debug.LogError("IOC ERROR -- Tried to resolve a type with no dependencies: " + clientType);
		}

		foreach(FieldInfo field in fields)
		{
			object provider = null;

			//this is to support the insane idea that we might want to be able to have a field with multiple dependency keys. 
			Dictionary<string, object> typeProviders;
			if (registeredProviders.TryGetValue(field.FieldType, out typeProviders))
			{
				IEnumerable<DependencyAttribute> dependencies = field.GetCustomAttributes<DependencyAttribute>();
				foreach (DependencyAttribute dependency in dependencies)
				{
					if (typeProviders.TryGetValue(dependency.key ?? string.Empty, out provider))
						break;
				}

				if(provider == null)
					Debug.LogError("IOC ERROR -- Tried to resolve a type but found no registered providers with the Key  FIELD TYPE: " + field.FieldType + "     CLIENT TYPE: " + clientType);
			}
			else
				Debug.LogError("IOC ERROR -- Tried to resolve a type but found no registered providers : " + field.FieldType);

			if (provider != null)
				field.SetValue(client, provider);
		}
	}
}
