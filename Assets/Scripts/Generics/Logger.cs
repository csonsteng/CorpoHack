using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : Singleton<Logger>
{
	public static void Log(string message) => Instance.LogPrivate(message);

	private void LogPrivate(string message)
	{
		Debug.Log(message);
	}

}
