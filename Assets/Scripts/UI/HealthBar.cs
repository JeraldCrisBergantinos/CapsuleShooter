using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	
	public GUITexture healthBar;
	
	Player player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType( typeof(Player) ) as Player;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		float healthPercent = 0;
		if ( player != null ) {
			healthPercent = player.health / player.startingHealth;
		}
		
		Rect rect = healthBar.pixelInset;
		rect.width = Screen.width * healthPercent;
		healthBar.pixelInset = rect;
	}
}
