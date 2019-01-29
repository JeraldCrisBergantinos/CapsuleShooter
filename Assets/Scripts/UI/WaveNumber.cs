using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WaveNumber : MonoBehaviour {
	
	public GUITexture banner;
	
	Spawner spawner;
	string waveNumberStr;
	string enemyCountStr;
	
	string[] numberStr = { "One", "Two", "Three", "Four", "Five" };
	
	void Awake() {
		spawner = FindObjectOfType( typeof(Spawner) ) as Spawner;
		spawner.OnNewWave += OnNewWave;
	}
	
	void OnNewWave( int waveNumber ) {
		waveNumberStr = numberStr[ waveNumber - 1 ];
		enemyCountStr = spawner.waves[ waveNumber - 1 ].enemyCount.ToString();
		if ( spawner.waves[ waveNumber - 1 ].infinite ) {
			enemyCountStr = "Infinite";
		}
		
		StopCoroutine( "AnimateNewWaveBanner" );
		StartCoroutine( "AnimateNewWaveBanner" );
	}
	
	IEnumerator AnimateNewWaveBanner() {
		float delayTime = 1.5f;
		float speed = 3f;
		float animatePercent = 0;
		int dir = 1;
		
		float endDelayTime = Time.time + 1/speed + delayTime;
		
		while ( animatePercent >= 0 ) {
			animatePercent += Time.deltaTime * speed * dir;
			
			if ( animatePercent >= 1 ) {
				animatePercent = 1;
				if ( Time.time > endDelayTime ) {
					dir = -1;
				}
			}
			
			Rect rect = banner.pixelInset;
			rect = new Rect( rect.x, Mathf.Lerp( -40, 50, animatePercent ), Screen.width, rect.height );
			banner.pixelInset = rect;
			yield return null;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		ShowWaveNumber();
		ShowEnemyCount();
	}
	
	void ShowWaveNumber() {
		string labelStr = "- Wave " + waveNumberStr +" -";
		float labelWidth = labelStr.Length * 10f;
		float labelHeight = 20f;
		float labelX = (Screen.width - labelWidth) * 0.5f;
		float labelY = Screen.height - banner.pixelInset.y - banner.pixelInset.height;
		GUI.Label( new Rect( labelX, labelY, labelWidth, labelHeight ), labelStr );
	}
	
	void ShowEnemyCount() {
		string labelStr = "Enemies : " + enemyCountStr;
		float labelWidth = labelStr.Length * 10f;
		float labelHeight = 20f;
		float labelX = (Screen.width - labelWidth) * 0.5f;
		float labelY = Screen.height - banner.pixelInset.y - banner.pixelInset.height + labelHeight;
		GUI.Label( new Rect( labelX, labelY, labelWidth, labelHeight ), labelStr );
	}
}
