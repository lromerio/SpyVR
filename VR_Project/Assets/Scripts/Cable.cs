using UnityEngine;

public class Cable : MonoBehaviour {

    Pliers pliers = null;
    public Cables cables;

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

    void Update ()
    {
		if(pliers && pliers.Cutting)
            cables.Cut(this);
    }
}
