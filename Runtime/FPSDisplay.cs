//
// Author: Alessandro Salani (Cippo)
//

using UnityEngine;
using UnityEngine.UI;

namespace CippSharp.Core.DeCa
{
	[RequireComponent(typeof(Text))]
	public class FPSDisplay : MonoBehaviour
	{
		//References:
		private Text fpsText = null;

		//Runtime
		private float deltaTime = 0.0f;
		private float msec = 0;
		private float fps = 0;
		private string text = string.Empty;

		private void Awake()
		{
			fpsText = GetComponent<Text>();
		}

		private void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			msec = deltaTime * 1000.0f;
			fps = 1.0f / deltaTime;
			text = $"{msec:0.0} ms ({fps:0.} fps)";
			fpsText.text = text;
		}
	}
}
