using System.Collections.Generic;
using UnityEngine;

public class VRBlinder : MonoBehaviour {

	HashSet<Collider> colliders = new HashSet<Collider>();
	HashSet<Collider> currentWalls = new HashSet<Collider>();
	public List<Collider> ignoreList;
	public List<Collider> walls;
	public float transitionTime;
	public float repulseFac;
	public GameObject cameraRig;
	private Collider wall;
	private Vector3 lastPos;
	private Vector3 lastFreePos;

	void UpdateFade()
    {
		if (colliders.Count == 0)
			SteamVR_Fade.View (Color.clear, transitionTime);
		else
			SteamVR_Fade.View (Color.black, transitionTime);
	}

	bool InWalls()
    {
		return currentWalls.Count != 0;
	}

	void OnTriggerEnter(Collider col)
    {
		if(!ignoreList.Contains(col))
			colliders.Add (col);

		if (walls.Contains (col))
			currentWalls.Add (col);

		UpdateFade ();
	}

	void OnTriggerExit(Collider col)
    {
		colliders.Remove (col);
		currentWalls.Remove (col);
		UpdateFade ();
	}

	void Update ()
    {
		Vector3 pos = transform.position;

		if (InWalls()) {
			Vector3 delta = pos - lastPos;
			delta.y = 0;
			cameraRig.transform.position -= repulseFac*delta;
		}

		lastPos = pos;
	}
}
