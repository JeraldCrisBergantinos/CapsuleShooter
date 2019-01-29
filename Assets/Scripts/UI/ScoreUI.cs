using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScoreUI : MonoBehaviour {
	
	public Rect rect;
	public string str;
	public GUIStyle style;

	void OnGUI() {
		rect.x = ( Screen.width - rect.width ) * 0.5f;
		GUI.Label( rect, str, style );
	}
	
	void Update() {
		str = ScoreKeeper.score.ToString("D6");
	}
}
