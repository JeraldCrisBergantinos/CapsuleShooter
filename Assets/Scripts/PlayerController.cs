using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	
	Rigidbody rigidBody;
	Vector3 velocity;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
	public void Move( Vector3 _velocity ) {
		velocity = _velocity;
	}
	
	void FixedUpdate() {
		rigidbody.MovePosition( rigidBody.position + velocity * Time.fixedDeltaTime );
	}
	
	public void LookAt( Vector3 lookPoint ) {
		Vector3 heightCorrectedPoint = new Vector3( lookPoint.x, transform.position.y, lookPoint.z );
		transform.LookAt( heightCorrectedPoint );
	}
}
