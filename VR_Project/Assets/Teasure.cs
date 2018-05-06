using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teasure : MonoBehaviour {
	private MeshCollider mc;
	public Transform key;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		print (gameObject.name);
		if (mc && mc.isTrigger && gameObject.name == "KeyGold") {
			GetComponent<Animator> ().Play("box_open");
		}
	}
}
