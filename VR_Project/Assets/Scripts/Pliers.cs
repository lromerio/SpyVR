using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pliers : MonoBehaviour {

    Animator animator;
	// Use this for initialization
    public bool cutting
    {
        get { return old_val < new_val && new_val > 0.9; }
    }

    float old_val = 0f;
    float new_val = 0f;
	void Start () {
        //animation.Play();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
       
    }



    public void set_closed_value(float val)
    {
        old_val = new_val;
        if (animator.runtimeAnimatorController != null)
            animator.Play("Pinch", 0, val);
        new_val = val;
    }
	
	// Update is called once per frame
	void Update () {
	}
}
