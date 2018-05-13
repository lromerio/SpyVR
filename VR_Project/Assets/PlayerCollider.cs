using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour {

	CapsuleCollider col;
	public LayerMask collision_mask;
	// Use this for initialization
	void Start () {
		col = GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.up = new Vector3 (0, -1, 0);
		RaycastHit hit;
		if (Physics.Raycast(transform.position,new Vector3(0,-1,0),out hit,100,collision_mask)) {
			col.height = Vector3.Distance (transform.position, hit.point);
		}
	}
}
