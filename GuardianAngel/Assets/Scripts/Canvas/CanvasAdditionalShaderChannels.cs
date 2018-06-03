using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasAdditionalShaderChannels : MonoBehaviour 
{
	public AdditionalCanvasShaderChannels canvaschannels;
	public Canvas[] canvases;

	void Awake ()
	{
		UpdateCanvasState ();
	}

	void Start ()
	{
		InvokeRepeating ("UpdateCanvasState", 0, 3);
	}

	#if UNITY_EDITOR
	void Update ()
	{
		UpdateCanvasState ();
	}
	#endif

	void UpdateCanvasState ()
	{
		foreach (Canvas canvas in canvases) 
		{
			if (canvas.additionalShaderChannels != canvaschannels) 
			{
				canvas.additionalShaderChannels = canvaschannels;
			}
		}
	}
}