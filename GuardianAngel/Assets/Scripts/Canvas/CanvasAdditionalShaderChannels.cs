using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasAdditionalShaderChannels : MonoBehaviour 
{
	public AdditionalCanvasShaderChannels[] canvaschannels;
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
			canvas.additionalShaderChannels |= canvaschannels[0];
			canvas.additionalShaderChannels |= canvaschannels[1];
			canvas.additionalShaderChannels |= canvaschannels[2];
			canvas.additionalShaderChannels |= canvaschannels[3];
			canvas.additionalShaderChannels |= canvaschannels[4];

		}
	}
}