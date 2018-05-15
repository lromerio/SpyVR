using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBlinder : MonoBehaviour {

	HashSet<Collider> colliders = new HashSet<Collider>();
	HashSet<Collider> current_walls = new HashSet<Collider>();
	public List<Collider> ignore_list;
	public List<Collider> walls;
	public float transition_time;
	public float repulse_fac;
	public GameObject camera_rig;
	private Collider wall;
	private Vector3 last_pos;
	private Vector3 last_free_pos;

	// Use this for initialization
	void Start () {
		
	}


	void UpdateFade() {
		if (colliders.Count == 0)
			SteamVR_Fade.View (Color.clear, transition_time);
		else
			SteamVR_Fade.View (Color.black, transition_time);
		
	}

	bool inWalls() {
		return current_walls.Count != 0;
	}

	void OnTriggerEnter(Collider col) {
		if(!ignore_list.Contains(col))
			colliders.Add (col);
		if (walls.Contains (col)) {
			current_walls.Add (col);
		}
		UpdateFade ();
	}

	void OnTriggerExit(Collider col) {
		colliders.Remove (col);
		current_walls.Remove (col);
		UpdateFade ();
	}


	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		if (inWalls()) {
			Vector3 delta = pos - last_pos;
			delta.y = 0;
			camera_rig.transform.position -= repulse_fac*delta;
		}
		last_pos = pos;
	}
}
