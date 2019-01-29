using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Canvas3 : MonoBehaviour {
	
	public string buttonText = "Play Again";
	
	public GUIText guiText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		//Debug.Log( Screen.width + " " + Screen.height );
		
		guiText.fontSize = (int)( ((float)40 / (float)353) * Screen.height );
		
		float textWidth = guiText.text.Length * guiText.fontSize;
		float textHeight = guiText.fontSize;
		float textX = (Screen.width - textWidth) * 0.5f;
		float textY = (Screen.height - textHeight) * 0.5f;
		
		guiText.pixelOffset = new Vector2( textX * -3.5f, textY * -0.7f );
		
		float buttonWidth = buttonText.Length * 10;
		float buttonHeight = (int)( ((float)30 / (float)353) * Screen.height );
		float buttonX = (Screen.width - buttonWidth) * 0.5f;
		float buttonY = Screen.height * 0.6f;
		if ( GUI.Button( new Rect( buttonX, buttonY, buttonWidth, buttonHeight ), buttonText ) ) {
			Debug.Log( "clicked" );
		}
	}
}
