//
// Author: Alessandro Salani (Cippo)
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CippSharp.Core.DeCa
{
	public class DebugCanvas : MonoBehaviour
	{
		/// <summary>
		/// A nicer name for logs.
		/// </summary>
		public static readonly string LogName = $"[{typeof(DebugCanvas).Name}]: ";

		[Serializable]
		public class Settings
		{
			public bool errors = true;
			public bool asserts = true;
			public bool warnings = true;
			public bool logs = true;
			public bool exceptions = true;
		}
		
		[Header("Functionality:")]
		public bool dontDestroyOnLoad = true;
		[Tooltip("Log Prefab lifetime, -1 means infinite.")]
		public float logLifetime = 5f;
		
		[Header("Settings:")]
		[SerializeField] private Settings settings = new Settings();
		[SerializeField] private GameObject logPrefab = null;
		
		//NotEditableInPlay
		[SerializeField] private string logTextChildName = "LogText";
		//NotEditableInPlay
		[SerializeField] private string stackTraceTextChildName = "StackText";
		
		[Header("References:")] 
		[SerializeField] private RectTransform logsParent = null;
		
		private List<string> completeLogs = new List<string>();
		private List<GameObject> logsInstances = new List<GameObject>();
		
		/// <summary>
		/// The main instance of the debug canvas.
		/// </summary>
		public static DebugCanvas Instance { get; private set; }
		
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				if (dontDestroyOnLoad)
				{
					DontDestroyOnLoad(Instance.gameObject);
				}
			}
		}
		
		private void OnEnable()
		{
			Application.logMessageReceived += ApplicationOnLogMessageReceived;
		}

		#region Display Log Messages
		
		private void ApplicationOnLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			switch (type)
			{
				case LogType.Error:
					if (settings.errors)
					{
						LogInternal(condition.OfColor(Color.red), stacktrace);
					}
					break;
				case LogType.Assert:
					if (settings.asserts)
					{
						LogInternal(condition.OfColor(Color.red), stacktrace);
					}
					break;
				case LogType.Warning:
					if (settings.warnings)
					{
						LogInternal(condition.OfColor(Color.yellow), stacktrace);
					}
					break;
				case LogType.Log:
					if (settings.logs)
					{
						LogInternal(condition.OfColor(Color.white), stacktrace);
					}
					break;
				case LogType.Exception:
					if (settings.exceptions)
					{
						LogInternal(condition.OfColor(Color.red), stacktrace);
					}
					break;
			}
		}

		private void LogInternal(string message, string stacktrace = "")
		{
			if (!(logsParent.gameObject.activeSelf && logsParent.gameObject.activeInHierarchy))
			{
				Debug.LogWarning(LogName+"Canvas GameObject may be deactivated! Logs won't spawn.", this);
				return;
			}

			if (completeLogs.Contains(message + stacktrace))
			{
				return;
			}
			
			GameObject log = Instantiate(logPrefab, logsParent);
			
			log.transform.SetAsFirstSibling();

			Text logText = log.transform.Find(logTextChildName).GetComponent<Text>();
			logText.text = message;

			Text stackText = log.transform.Find(stackTraceTextChildName).GetComponent<Text>();
			stackText.text = stacktrace;
			
			completeLogs.Add(message+stacktrace);
			logsInstances.Add(log);
			
			if (logLifetime >= 0.0f)
			{
				StartCoroutine(RemoveLogAfterTime(log));
			}
		}

		private IEnumerator RemoveLogAfterTime(GameObject log)
		{
			yield return new WaitForSeconds(logLifetime);
			try
			{
				int index = logsInstances.IndexOf(log);
				logsInstances.RemoveAt(index);
				completeLogs.RemoveAt(index);
			}
			catch (Exception e)
			{
				Debug.LogError(LogName+$"Failed to remove log. Error {e.Message}", this);
			}
			
			Object.Destroy(log);
		}
		
		#endregion
		
		private void OnDisable()
		{
			Application.logMessageReceived -= ApplicationOnLogMessageReceived;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		#region Utils

		/// <summary>
		/// Public method log to print a custom message directly in debug canvas. 
		/// </summary>
		/// <param name="message"></param>
		public void Log(string message)
		{
			LogInternal(message, string.Empty);
		}

		/// <summary>
		/// Delete all spawned logs until now.
		/// </summary>
		public void ClearLogs()
		{
			if (!IsNullOrEmpty(logsInstances))
			{
				completeLogs.Clear();
			}

			if (!IsNullOrEmpty(logsInstances))
			{
				Array.ForEach(logsInstances.Cast<Object>().ToArray(), Destroy);
				logsInstances.Clear();
			}
		}

		private static bool IsNullOrEmpty<T>(List<T> list)
		{
			return list == null || list.Count < 1;
		}
		
		#endregion
	
	}
}