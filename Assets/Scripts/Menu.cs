using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Menu : MonoBehaviour {
	
	public GUISkin skin;
	public float spacerHeight = 10;

	public Label title;	
	public Button play;
	public Button options;
	public Button quit;
	
	public bool mainMenuFlag = true;
	
	public Volume masterVolume;
	public Volume musicVolume;
	public Volume sfxVolume;
	[Range(0,1)]
	public float sliderScreenPos = 0.25f;

	public ResolutionUI _800X600;
	public ResolutionUI _1024X768;
	public ResolutionUI _1366X768;
	public ResolutionUI fullScreen;
	[Range(0,1)]
	public float toggleScreenPos = 0.75f;
	
	public Button back;
	
	ResolutionUI[] resolutionValues;
	int activeResolutionIndex;
	
	[System.Serializable]
	public class Label {
		public string name;
		public Rect rect;
	}
	
	[System.Serializable]
	public class Button : Label {
		
	}
	
	[System.Serializable]
	public class Volume {
		public string name;
		public float value;
		public Rect labelRect;
		public Rect sliderRect;
	}
	
	[System.Serializable]
	public class ResolutionUI {
		public string name;
		public bool flag;
		public Rect rect;
		public int index;
		public int width;
		public int height;
	}

	// Use this for initialization
	void Start () {
		resolutionValues = new ResolutionUI[3];
		resolutionValues[0] = _800X600;
		resolutionValues[1] = _1024X768;
		resolutionValues[2] = _1366X768;
		
		activeResolutionIndex = PlayerPrefs.GetInt( "Screen Resolution Index", 0 );
		bool isFullScreen = ( PlayerPrefs.GetInt( "Full Screen", 0 ) == 1 );
		
		masterVolume.value = AudioManager.instance.masterVolumePercent;
		musicVolume.value = AudioManager.instance.musicVolumePercent;
		sfxVolume.value = AudioManager.instance.sfxVolumePercent;
		
		fullScreen.flag = isFullScreen;
		for ( int i = 0; i < resolutionValues.Length; ++i ) {
			if ( !isFullScreen && i == activeResolutionIndex )
				resolutionValues[i].flag = true;
			else
				resolutionValues[i].flag = false;
		}
		//SetFullScreen( isFullScreen );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = skin;
		
		ShowTitle();
		
		if ( mainMenuFlag ) {
			ShowPlay();
			ShowOptions();
			ShowQuit();
		}
		else {
			ShowMasterVolume();
			ShowMusicVolume();
			ShowSfxVolume();
			
			Show800X600Resolution();
			Show1024X768Resolution();
			Show1366X768Resolution();
			ShowFullScreenResolution();
			
			ShowBack();
		}
	}
	
	void ShowTitle() {
		title.rect.x = (Screen.width - title.rect.width) * 0.5f;
		title.rect.y = (Screen.height - title.rect.height) * 0.2f;
		GUI.Label( title.rect, title.name, "Title" );
	}
	
	void ShowPlay() {
		play.rect.x = (Screen.width - play.rect.width) * 0.5f;
		play.rect.y = (Screen.height - play.rect.height) * 0.5f;
		if ( GUI.Button( play.rect, play.name ) )
			Play();
	}
	
	void ShowOptions() {
		options.rect.x = (Screen.width - options.rect.width) * 0.5f;
		options.rect.y = play.rect.y + play.rect.height + spacerHeight;
		if ( GUI.Button( options.rect, options.name ) )
			OptionsMenu();
	}
	
	void ShowQuit() {
		quit.rect.x = (Screen.width - quit.rect.width) * 0.5f;
		quit.rect.y = options.rect.y + options.rect.height + spacerHeight;
		if ( GUI.Button( quit.rect, quit.name ) )
			Quit();
	}
	
	void ShowMasterVolume() {
		masterVolume.labelRect.x = (Screen.width - masterVolume.labelRect.width) * sliderScreenPos;
		masterVolume.labelRect.y = (Screen.height - masterVolume.labelRect.height) * 0.5f;
		GUI.Label( masterVolume.labelRect, masterVolume.name );
		
		masterVolume.sliderRect.x = (Screen.width - masterVolume.sliderRect.width) * sliderScreenPos;
		masterVolume.sliderRect.y = masterVolume.labelRect.y + masterVolume.labelRect.height;
		float temp = GUI.HorizontalSlider( masterVolume.sliderRect, masterVolume.value, 0, 1 );
		if ( temp != masterVolume.value ) {
			masterVolume.value = temp;
			SetMasterVolume( masterVolume.value );
		}
	}
	
	void ShowMusicVolume() {
		musicVolume.labelRect.x = (Screen.width - musicVolume.labelRect.width) * sliderScreenPos;
		musicVolume.labelRect.y = masterVolume.sliderRect.y + masterVolume.sliderRect.height + spacerHeight;
		GUI.Label( musicVolume.labelRect, musicVolume.name );
		
		musicVolume.sliderRect.x = (Screen.width - musicVolume.sliderRect.width) * sliderScreenPos;
		musicVolume.sliderRect.y = musicVolume.labelRect.y + musicVolume.labelRect.height;
		float temp = GUI.HorizontalSlider( musicVolume.sliderRect, musicVolume.value, 0, 1 );
		if ( temp != musicVolume.value ) {
			musicVolume.value = temp;
			SetMusicVolume( musicVolume.value );
		}
	}
	
	void ShowSfxVolume() {
		sfxVolume.labelRect.x = (Screen.width - sfxVolume.labelRect.width) * sliderScreenPos;
		sfxVolume.labelRect.y = musicVolume.sliderRect.y + musicVolume.sliderRect.height + spacerHeight;
		GUI.Label( sfxVolume.labelRect, sfxVolume.name );
		
		sfxVolume.sliderRect.x = (Screen.width - sfxVolume.sliderRect.width) * sliderScreenPos;
		sfxVolume.sliderRect.y = sfxVolume.labelRect.y + sfxVolume.labelRect.height;
		float temp = GUI.HorizontalSlider( sfxVolume.sliderRect, sfxVolume.value, 0, 1 );
		if ( temp != sfxVolume.value ) {
			sfxVolume.value = temp;
			SetSfxVolume( sfxVolume.value );
		}
	}
	
	void Show800X600Resolution() {
		_800X600.rect.x = (Screen.width - _800X600.rect.width) * toggleScreenPos;
		_800X600.rect.y = (Screen.height - _800X600.rect.height) * 0.5f;
		bool temp = GUI.Toggle( _800X600.rect, _800X600.flag, _800X600.name );
		if ( temp != _800X600.flag ) {
			_800X600.flag = temp;
			if ( _800X600.flag ) {
				_1024X768.flag = false;
				_1366X768.flag = false;
				fullScreen.flag = false;
				
				SetScreenResolution( _800X600.index );
			}
		}
	}
	
	void Show1024X768Resolution() {
		_1024X768.rect.x = (Screen.width - _1024X768.rect.width) * toggleScreenPos;
		_1024X768.rect.y = _800X600.rect.y + _800X600.rect.height + spacerHeight;
		bool temp = GUI.Toggle( _1024X768.rect, _1024X768.flag, _1024X768.name );
		if ( temp != _1024X768.flag ) {
			_1024X768.flag = temp;
			if ( _1024X768.flag ) {
				_800X600.flag = false;
				_1366X768.flag = false;
				fullScreen.flag = false;
			
				SetScreenResolution( _1024X768.index );
			}
		}
	}
	
	void Show1366X768Resolution() {
		_1366X768.rect.x = (Screen.width - _1366X768.rect.width) * toggleScreenPos;
		_1366X768.rect.y = _1024X768.rect.y + _1024X768.rect.height + spacerHeight;
		bool temp = GUI.Toggle( _1366X768.rect, _1366X768.flag, _1366X768.name );
		if ( temp != _1366X768.flag ) {
			_1366X768.flag = temp;
			if ( _1366X768.flag ) {
				_800X600.flag = false;
				_1024X768.flag = false;
				fullScreen.flag = false;
			
				SetScreenResolution( _1366X768.index );
			}
		}
	}
	
	void ShowFullScreenResolution() {
		fullScreen.rect.x = (Screen.width - fullScreen.rect.width) * toggleScreenPos;
		fullScreen.rect.y = _1366X768.rect.y + _1366X768.rect.height + spacerHeight;
		bool temp = GUI.Toggle( fullScreen.rect, fullScreen.flag, fullScreen.name );
		if ( temp != fullScreen.flag ) {
			fullScreen.flag = temp;
			if ( fullScreen.flag ) {
				_800X600.flag = false;
				_1024X768.flag = false;
				_1366X768.flag = false;
			}
			SetFullScreen( fullScreen.flag );
		}
	}
	
	void ShowBack() {
		back.rect.x = (Screen.width - back.rect.width) * 0.5f;
		back.rect.y = sfxVolume.sliderRect.y + sfxVolume.sliderRect.height + spacerHeight;
		if ( GUI.Button( back.rect, back.name ) ) {
			PlayerPrefs.SetInt( "Screen Resolution Index", activeResolutionIndex );
			PlayerPrefs.SetInt( "Full Screen", ( fullScreen.flag ? 1 : 0 ) );
			PlayerPrefs.Save();
			MainMenu();
		}
	}
	
	public void Play() {
		Application.LoadLevel( "Game" );
	}
	
	public void Quit() {
		Application.Quit();
	}
	
	public void OptionsMenu() {
		mainMenuFlag = false;
	}
	
	public void MainMenu() {
		mainMenuFlag = true;
	}
	
	public void SetScreenResolution( int i ) {
		activeResolutionIndex = i;
		Screen.SetResolution( resolutionValues[i].width, resolutionValues[i].height, false );
		PlayerPrefs.SetInt( "Screen Resolution Index", activeResolutionIndex );
		PlayerPrefs.Save();
	}
	
	public void SetFullScreen( bool isFullScreen ) {
		//Screen.fullScreen = isFullScreen;
		
		if ( isFullScreen ) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions[ allResolutions.Length-1 ];
			Screen.SetResolution( maxResolution.width, maxResolution.height, true );
		}
		else {
			SetScreenResolution( activeResolutionIndex );
		}
		
		PlayerPrefs.SetInt( "Full Screen", ( isFullScreen ? 1 : 0 ) );
		PlayerPrefs.Save();
	}
	
	public void SetMasterVolume( float value ) {
		AudioManager.instance.SetVolume( value, AudioManager.AudioChannel.Master );
	}
	
	public void SetMusicVolume( float value ) {
		AudioManager.instance.SetVolume( value, AudioManager.AudioChannel.Music );
	}
	
	public void SetSfxVolume( float value ) {
		AudioManager.instance.SetVolume( value, AudioManager.AudioChannel.Sfx );
	}
}
