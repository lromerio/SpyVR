using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pliers : MonoBehaviour {

    Animator animator;
	// Use this for initialization
	void Start () {
        //animation.Play();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
       
    }

    public void set_closed_value(float val)
    {
        animator.Play("Pinch", 0,val);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
