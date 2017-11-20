using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(CharacterController))]

public class WaypointChaser : MonoBehaviour {
	
	public float speed = 20.0f;
	public float minDist = 0f;
	public float change_dist = 1f;
	public string wapoints_name = "Waypoints";

	private Transform current_target;
	private int index;
	private List<Transform> targets = new List<Transform>();

	// Use this for initialization
	void Start () 
	{
		GameObject waypoints = GameObject.Find (wapoints_name);
		if (waypoints == null) {
			Debug.Log ("No Waypoints find !!!");
			return;
		}
		for (int i = 0; i < waypoints.transform.childCount; i++) {  
			targets.Add(waypoints.transform.GetChild(i));  
		}  
		index = 0;
		current_target = targets [index];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (current_target == null)
			return;

		// face the target
		transform.LookAt(current_target);

		//get the distance between the chaser and the target
		float distance = Vector3.Distance(transform.position,current_target.position);

		//so long as the chaser is farther away than the minimum distance, move towards it at rate speed.
		if (distance > minDist)	
			transform.position += transform.forward * speed * Time.deltaTime;
		if (distance <= change_dist) {
			current_target = targets[(++index) % targets.Count];
		}
	}

	// Set the target of the chaser
	public void SetTarget(Transform newTarget)
	{
		current_target = newTarget;
	}

}
