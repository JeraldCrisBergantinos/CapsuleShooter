using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public LayerMask collisionMask;
	public Color trailColor;
	
	float speed = 10;
	float damage = 1;
	float maxDistance = 10;
	float lifetime = 3.0f;
	float initialCollisionRadius = 0.1f;
	float skinWidth = 0.1f;
	
	void Start() {
		Destroy( gameObject, lifetime );
		
		Collider[] initialCollisions = Physics.OverlapSphere( transform.position,
															initialCollisionRadius,
															collisionMask );
		if ( initialCollisions.Length > 0 ) {
			OnHitObject( initialCollisions[0], transform.position );
		}
		
		GetComponent<TrailRenderer>().material.SetColor( "_TintColor", trailColor );
	}
	
	public void SetSpeed( float newSpeed ) {
		speed = newSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions( moveDistance );
		transform.Translate( Vector3.forward * moveDistance );
	}
	
	void CheckCollisions( float moveDistance ) {
		Ray ray = new Ray( transform.position, transform.forward );
		RaycastHit hit;
		
		if ( Physics.Raycast( ray, out hit, moveDistance + skinWidth, collisionMask ) ) {
			OnHitObject( hit.collider, hit.point );
		}
	}
	
	void OnHitObject( Collider c, Vector3 hitPoint ) {
		//print ( c.gameObject.name );
		IDamageable damageable = c.GetComponent<LivingEntity>();
		if ( damageable != null )
			damageable.TakeHit( damage, hitPoint, transform.forward );
		GameObject.Destroy( gameObject );
	}
}
