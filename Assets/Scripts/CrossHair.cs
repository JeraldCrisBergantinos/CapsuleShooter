using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {
	
	public LayerMask layerMask;
	public Transform node;
	public Color highLightNodeColor;
	
	Color originalNodeColor;
	Material nodeMaterial;
	
	void Start() {
		Screen.showCursor = false;
		nodeMaterial = node.GetComponent<Renderer>().material;
		originalNodeColor = nodeMaterial.color;
		
		Player player = FindObjectOfType( typeof(Player) ) as Player;
		player.OnDeath += OnGameOver;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate( Vector3.up * 40 * Time.deltaTime );
	}
	
	public void DetectTargets( Ray ray ) {
		if ( Physics.Raycast( ray, 100, layerMask ) ) {
			nodeMaterial.color = highLightNodeColor;
		}
		else {
			nodeMaterial.color = originalNodeColor;
		}
	}
	
	void OnGameOver() {
		Screen.showCursor = true;
	}
}
