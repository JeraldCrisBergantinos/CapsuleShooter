using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
	
	public AudioClip mainTheme;
	public AudioClip menuTheme;
	
	string sceneName;

	// Use this for initialization
	void Start () {
		OnLevelWasLoaded( 0 );
	}
	
	void OnLevelWasLoaded( int sceneIndex ) {
		string newSceneName = Application.loadedLevelName;
		if ( newSceneName != sceneName ) {
			sceneName = newSceneName;
			Invoke( "PlayMusic", 0.2f );
		}
	}
	
	void PlayMusic() {
		AudioClip clipToPlay = null;
		
		if ( sceneName == "Menu" ) {
			clipToPlay = menuTheme;
		}
		else if ( sceneName == "Game" ) {
			clipToPlay = mainTheme;
		}
		
		if ( clipToPlay != null ) {
			AudioManager.instance.PlayMusic( clipToPlay, 2 );
			Invoke( "PlayMusic", clipToPlay.length );
		}
	}
}
