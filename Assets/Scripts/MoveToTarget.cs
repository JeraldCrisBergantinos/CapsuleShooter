using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour {
	
	public Transform target;
	public float speed = 10;
	public float minDistance = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance( target.position, transform.position );
		
		if ( distance > minDistance ) {
			Vector3 dir = ( target.position - transform.position ).normalized;
			transform.Translate( dir * Time.deltaTime * speed );
		}
	}
}
