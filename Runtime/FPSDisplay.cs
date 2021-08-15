//
// Author: Alessandro Salani (Cippo)
//

using UnityEngine;
using UnityEngine.UI;

namespace CippSharp.Core.DeCa
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Text))]
	public class FPSDisplay : MonoBehaviour
	{
		//References:
		private Text fpsText = null;

		//Runtime
		private float deltaTime = 0.0f;
		private float msec;
		private float fps;
		private string text;

		private void Awake()
		{
			fpsText = GetComponent<Text>();
		}

		private void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			msec = deltaTime * 1000.0f;
			fps = 1.0f / deltaTime;
			text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			fpsText.text = text;
		}
	}
}