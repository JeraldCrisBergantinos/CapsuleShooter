using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {
	
	public float flashTime;
	
	void Start() {
		Deactivate();
	}

	public void Activate() {
		transform.gameObject.SetActive( true );
		Invoke( "Deactivate", flashTime );
	}
	
	void Deactivate() {
		transform.gameObject.SetActive( false );
	}
}
