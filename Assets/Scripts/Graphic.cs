using UnityEngine;
using System.Collections;

public class Graphic : MonoBehaviour {
	
	public GUITexture texture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Rect rect = texture.pixelInset;
		float x = ( Screen.width - rect.width ) * 0.5f;
		float y = ( Screen.height - rect.height ) * 0.5f;
		rect.x = x;
		rect.y = y;
		texture.pixelInset = rect;
	}
}
