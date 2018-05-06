using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour {

    Pliers pliers = null;
    public Cables cables;

	// Use this for initialization
	void Start () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        Pliers p = other.gameObject.GetComponent<Pliers>();
        if (p) pliers = p;
    }

    public void OnTriggerStay(Collider other)
    {
        Pliers p = other.gameObject.GetComponent<Pliers>();
        if (p) pliers = p;
    }

    public void OnTriggerExit(Collider other)
    {
        Pliers p = other.gameObject.GetComponent<Pliers>();
        if (p) pliers = null;
    }

    // Update is called once per frame
    void Update () {
        if (pliers)
            print("La merde");
		if(pliers && pliers.cutting)
        {
            cables.cut(this);
        }
	}
}
