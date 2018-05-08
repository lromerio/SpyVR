using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teasure : MonoBehaviour {
	public Transform key;

	private void OnTriggerEnter(Collider c) {
		if (c.gameObject.name == "KeyGold")
		{
			Debug.Log("collision");
			GetComponent<Animation> ().Play("box_open");
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
