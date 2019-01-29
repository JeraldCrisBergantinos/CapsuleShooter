using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use %Pathfinding
using Pathfinding;

public class AstarAI : MonoBehaviour {
	
	//The point to move to
	public Transform target;
	
	private Seeker seeker;
	private CharacterController controller;
	
	//The calculated path
	public Path path;
	
	//The AI speed per second
	public float speed = 100;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWayPointDistance = 3;
	
	//The waypoint we are currently moving towards
	private int currentWayPoint = 0;
	
	private Vector3 oldPos = new Vector3( 0, 0, 0 );

	// Use this for initialization
	void Start () {
		//Get a reference to the Seeker component we added earlier
		seeker = GetComponent<Seeker>();
		
		controller = GetComponent<CharacterController>();
		
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		//seeker.StartPath( transform.position, target.position, OnPathComplete );
	}
	
	public void OnPathComplete( Path p ) {
		Debug.Log( "We have a path back. Error: " + p.error );
		if ( !p.error ) {
			path = p;
			//Reset the current waypoint counter
			currentWayPoint = 0;
		}
	}
	
	public void FixedUpdate() {
		if ( path == null ) {
			//"We have no path to move over yet.
			return;
		}
		
		if ( currentWayPoint >= path.vectorPath.Count ) {
			Debug.Log( "End of path reached." );
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		controller.SimpleMove( dir );
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if ( Vector3.Distance( transform.position, path.vectorPath[currentWayPoint] ) < nextWayPointDistance ) {
			currentWayPoint++;
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( target.position != oldPos ) {
			oldPos = target.position;
			path = null;
			currentWayPoint = 0;
			seeker.StartPath( transform.position, target.position, OnPathComplete );
		}
	}
}
