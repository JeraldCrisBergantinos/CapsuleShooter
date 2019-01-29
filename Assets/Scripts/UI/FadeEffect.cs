using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FadeEffect : MonoBehaviour {
	
	public GUITexture fadeTexture;
	public float fadeDelay = 5f;
	public GUIStyle style;
	
	bool isGameOver = false;

	// Use this for initialization
	void Start () {
		//StartCoroutine( Fade( Color.clear, Color.black, fadeDelay ) );
		Player player = FindObjectOfType( typeof(Player) ) as Player;
		player.OnDeath += OnGameOver;
		
		fadeTexture.pixelInset = new Rect( 0, 0, Screen.width, Screen.height );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if ( isGameOver )
			ShowGameOver();
	}
	
	IEnumerator Fade( Color from, Color to, float time ) {
		float speed = 1 / time;
		float percent = 0;
		
		while ( percent < 1 ) {
			percent += Time.deltaTime * speed;
			fadeTexture.color = Color.Lerp( from, to, percent );
			yield return null;
		}
	}
	
	void OnGameOver() {
		isGameOver = true;
		StartCoroutine( Fade( Color.clear, new Color( 0, 0, 0, 0.4f ), fadeDelay ) );
	}
	
	void StartNewGame() {
		Application.LoadLevel( "Game" );
	}
	
	void BackToMenu() {
		Application.LoadLevel( "Menu" );
	}
	
	void ShowGameOver() {
		float spacerHeight = 50f;
		
		string labelStr = "Game Over";
		float labelWidth = labelStr.Length * 10f;
		float labelHeight = 30f;
		float labelX = (Screen.width - labelWidth) * 0.5f;
		float labelY = (Screen.height - spacerHeight - labelHeight * 2f) * 0.5f;
		GUI.Label( new Rect( labelX, labelY, labelWidth, labelHeight ), labelStr, style );
		
		string playButtonStr = "PLAY AGAIN";
		float playButtonWidth = playButtonStr.Length * 10f;
		float playButtonHeight = 30f;
		float playButtonX = (Screen.width - playButtonWidth) * 0.5f;
		float playButtonY = labelY + labelHeight + spacerHeight;
		if ( GUI.Button( new Rect( playButtonX, playButtonY, playButtonWidth, playButtonHeight ), playButtonStr ) ) {
			StartNewGame();
		}
		
		string menuButtonStr = "RETURN TO MENU";
		float menuButtonWidth = menuButtonStr.Length * 10f;
		float menuButtonHeight = 30f;
		float menuButtonX = (Screen.width - menuButtonWidth) * 0.5f;
		float menuButtonY = playButtonY + playButtonHeight + spacerHeight;
		if ( GUI.Button( new Rect( menuButtonX, menuButtonY, menuButtonWidth, menuButtonHeight ), menuButtonStr ) ) {
			BackToMenu();
		}
	}
}
