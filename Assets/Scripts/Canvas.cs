using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Canvas : MonoBehaviour {
	
	public GUITexture fadeTexture;
	public float fadeDelay = 5f;
	
	public GUITexture bannner;
	public Rect bannerRect;
	
	bool isGameOver = false;

	// Use this for initialization
	void Start () {
		//StartCoroutine( Fade( Color.clear, Color.black, fadeDelay ) );
		Player player = FindObjectOfType( typeof(Player) ) as Player;
		player.OnDeath += OnGameOver;
		
		fadeTexture.pixelInset = new Rect( 0, 0, Screen.width, Screen.height );
		bannner.pixelInset = new Rect( 0, bannner.pixelInset.y, Screen.width, bannner.pixelInset.height );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if ( isGameOver )
			ShowGameOver();
		
		ShowWaveNumber();
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
		StartCoroutine( Fade( Color.clear, Color.black, fadeDelay ) );
	}
	
	void StartNewGame() {
		Application.LoadLevel( "Game" );
	}
	
	void ShowGameOver() {
		float spacerHeight = 50f;
		
		string labelStr = "Game Over";
		float labelWidth = labelStr.Length * 10f;
		float labelHeight = 30f;
		float labelX = (Screen.width - labelWidth) * 0.5f;
		float labelY = (Screen.height - spacerHeight - labelHeight * 2f) * 0.5f;
		GUI.Label( new Rect( labelX, labelY, labelWidth, labelHeight ), labelStr );
		
		string buttonStr = "Play Again";
		float buttonWidth = buttonStr.Length * 10f;
		float buttonHeight = 30f;
		float buttonX = (Screen.width - buttonWidth) * 0.5f;
		float buttonY = labelY + labelHeight + spacerHeight;
		if ( GUI.Button( new Rect( buttonX, buttonY, buttonWidth, buttonHeight ), buttonStr ) ) {
			StartNewGame();
		}
	}
	
	void ShowWaveNumber() {
		string waveStr = "- Wave One -";
		float labelWidth = waveStr.Length * 10f;
		float labelHeight = 30f;
		float labelX = (Screen.width - labelWidth) * 0.5f;
		float labelY = (Screen.height - labelHeight) * 0.5f;
		GUI.Label( bannerRect, waveStr );
		
		string enemyStr = "Enemies: 50";
	}
}
