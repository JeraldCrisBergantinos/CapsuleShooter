using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIPath))]
public class Enemy : LivingEntity {
	
	public float attackDistanceThreshold = 1.5f;
	public float timeBetweenAttacks = 1;
	public float attackSpeed = 3;
	
	public ParticleSystem deathEffect;
	public bool devMode;
	public static event System.Action OnDeathStatic;
	
	AIPath aiPath;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;
	Color originalColor;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;
	Vector3 oldTargetPos;
	bool attacking;
	
	bool hasTarget = false;
	float damage = 1.0f;
	
	void Awake() {
		aiPath = GetComponent<AIPath>();
		
		GameObject temp = GameObject.FindGameObjectWithTag( "Player" );
		if ( temp != null ) {
			target = temp.transform;
			targetEntity = target.GetComponent<LivingEntity>();
			
			myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

			hasTarget = true;
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		aiPath.endReachedDistance = attackDistanceThreshold;
		
		if ( hasTarget ) {
			aiPath.target = target;
			targetEntity.OnDeath += OnTargetDeath;
			
			oldTargetPos = target.position;
			attacking = false;
		}
		else {
			enablePathFinder( false );
		}
	}
	
	public void SetCharacteristics( float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor ) {
		aiPath.speed = moveSpeed;
		
		if ( devMode ) {
			damage = 0;
		}
		else if ( hasTarget ) {
			damage = Mathf.Ceil( targetEntity.startingHealth / hitsToKillPlayer );
		}
		
		startingHealth = enemyHealth;
		
		deathEffect.startColor = new Color( skinColor.r, skinColor.g, skinColor.b, 1 );
		skinMaterial = GetComponent<Renderer>().material;
		skinMaterial.color = skinColor;
		originalColor = skinMaterial.color;
	}
	
	public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		AudioManager.instance.PlaySound( "Impact", transform.position );
		if ( damage >= health ) {
			if ( OnDeathStatic != null )
				OnDeathStatic();
			AudioManager.instance.PlaySound( "Enemy Death", transform.position );
			Object effect = Instantiate( deathEffect, hitPoint, Quaternion.FromToRotation( Vector3.forward, hitDirection ) );
			Destroy( effect, deathEffect.startLifetime );
		}
		base.TakeHit (damage, hitPoint, hitDirection);
	}
	
	void OnTargetDeath() {
		hasTarget = false;
		enablePathFinder( false );
	}
	
	// Update is called once per frame
	void Update () {
		/*Debug.Log( "target reached: " + aiPath.TargetReached );
		Debug.Log( "Time.time > nextAttackTime: " + (Time.time > nextAttackTime) );*/
		if ( hasTarget && Time.time > nextAttackTime && attacking == false ) {
			float sqrDstToTarget = ( target.position - transform.position ).sqrMagnitude;
			/*Debug.Log( "sqrDstToTarget <= Mathf.Pow(attackDistanceThreshold,2): " + (sqrDstToTarget <= Mathf.Pow(attackDistanceThreshold,2)) );
			Debug.Log( "sqrDstToTarget: " + sqrDstToTarget );
			Debug.Log( "Mathf.Pow(attackDistanceThreshold,2): " + Mathf.Pow(attackDistanceThreshold,2) );*/
			float diff = Mathf.Pow(attackDistanceThreshold,2) - sqrDstToTarget;
			//Debug.Log( "diff: " + diff );
			
			if ( diff > -0.001 && diff < 0.001 ) {
			//if ( sqrDstToTarget <= Mathf.Pow(attackDistanceThreshold,2) ) {
				nextAttackTime = Time.time + timeBetweenAttacks;
				AudioManager.instance.PlaySound( "Enemy Attack", transform.position );
				StartCoroutine( Attack() );
			}
			else if ( aiPath.TargetReached && target.position == oldTargetPos ) {
				//Debug.Log( "Repositioning" );
				Vector3 originalPosition = transform.position;
				Vector3 dirToTarget = ( transform.position - target.position ).normalized;
				Vector3 attackPosition = target.position + dirToTarget * attackDistanceThreshold;
				float speed = aiPath.speed;
				transform.position = Vector3.Lerp( originalPosition, attackPosition, speed );
			}
			
			oldTargetPos = target.position;
		}
		else if ( !hasTarget && (aiPath.canSearch || aiPath.canMove) ) {
			enablePathFinder( false );
		}
	}
	
	IEnumerator Attack() {
		attacking = true;
		enablePathFinder( false );
		skinMaterial.color = Color.red;
		
		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = ( target.position - transform.position ).normalized;
		Vector3 attackPosition = target.position - dirToTarget * attackDistanceThreshold/2;
		
		float percent = 0;
		bool hasAppliedDamage = false;
		
		while ( percent <= 1 ) {
			
			if ( percent >= 0.5f && !hasAppliedDamage ) {
				hasAppliedDamage = true;
				targetEntity.TakeDamage( damage );
			}
			
			percent += Time.deltaTime * attackSpeed;
			//y = 4( -x^2 + x )
			float interpolation = 4 * ( -Mathf.Pow(percent,2) + percent );
			transform.position = Vector3.Lerp( originalPosition, attackPosition, interpolation );
			
			yield return null;
		}
		
		skinMaterial.color = originalColor;
		enablePathFinder( true );
		attacking = false;
	}
	
	void enablePathFinder( bool flag ) {
		aiPath.canSearch = flag;
		aiPath.canMove = flag;
	}
}
