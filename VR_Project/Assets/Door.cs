using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {
	private bool moving = false;
	public Transform door;

	IEnumerator<int> MoveFromTo(Vector3 pointA, Vector3 pointB, float time){
		if (!moving){ // do nothing if already moving
			moving = true; // signals "I'm moving, don't bother me!"
			float t = 0f;
			while (t < 1f){
				t += Time.deltaTime / time; // sweeps from 0 to 1 in time seconds
				door.transform.position = Vector3.Lerp(pointA, pointB, t); // set position proportional to t
				yield return 0; // leave the routine and return here in the next frame
			}
			moving = false; // finished moving
		}
	}

	public void move_y (float new_y) {
		Vector3 old_pos = door.position;
		Vector3 new_pos = new Vector3 (old_pos.x, new_y, old_pos.z);
		StartCoroutine(MoveFromTo(old_pos, new_pos, 5f));
	}
}
