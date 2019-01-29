using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]
public class Player : LivingEntity {
	
	public float moveSpeed = 3;
	public CrossHair crossHair;
	public float aimThreshold = 1.68f;
	
	PlayerController controller;
	GunController gunController;
	Camera viewCamera;
	
	void Awake() {
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		viewCamera = Camera.main;
		
		Spawner spawner = FindObjectOfType( typeof(Spawner) ) as Spawner;
		spawner.OnNewWave += OnNewWave;
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	void OnNewWave( int waveNumber ) {
		health = startingHealth;
		gunController.EquipGun( waveNumber - 1 );
	}
	
	// Update is called once per frame
	void Update () {
		//movement input
		Vector3 moveInput = new Vector3( Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical") );
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move( moveVelocity );
		
		//look input
		Ray ray = viewCamera.ScreenPointToRay( Input.mousePosition );
		Plane plane = new Plane( Vector3.up, Vector3.up * gunController.GunHeight );
		float rayDistance;
		
		if ( plane.Raycast( ray, out rayDistance ) ) {
			Vector3 point = ray.GetPoint( rayDistance );
//			Debug.DrawLine( ray.origin, point, Color.red );
			controller.LookAt( point );
			crossHair.transform.position = point;
			crossHair.DetectTargets( ray );
			
			//square magnitude is faster to compute
			float sqrDistance = ( new Vector2( point.x, point.z ) - new Vector2( transform.position.x, transform.position.z ) ).sqrMagnitude;
			//print( distance );
			if ( sqrDistance > Mathf.Pow( aimThreshold, 2 ) ) {
				gunController.Aim( point );
			}
		}
		
		//weapon input
		if ( Input.GetMouseButton(0) ) {
			gunController.OnTriggerHold();
		}
		else if ( Input.GetMouseButtonUp(0) ) {
			gunController.OnTriggerRelease();
		}
		
		if ( Input.GetKeyDown( KeyCode.R ) ) {
			gunController.Reload();
		}
		
		if ( transform.position.y < -10 )
			TakeDamage( health );
	}
	
	protected override void Die() {
		AudioManager.instance.PlaySound( "Player Death", transform.position );
		base.Die();
	}
}
