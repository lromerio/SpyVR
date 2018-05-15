using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour {

	public GameObject door;
	public GameObject load;

	private void OnTriggerEnter(Collider c) {
		Key k = c.GetComponent<Key>();
		if (k && k.name == "safe_key")
		{
			//TODO play key unlock sound
			GetComponent<AudioSource>().Play();
			load.SetActive(true);
			door.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}

	// Use this for initialization
	void Start () {
		door.GetComponent<Rigidbody> ().isKinematic = true;
		load.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
