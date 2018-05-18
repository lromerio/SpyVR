using UnityEngine;

public class KeyHole : MonoBehaviour {

	public GameObject door;
	public GameObject load;

	private void OnTriggerEnter(Collider c)
    {
		Key k = c.GetComponent<Key>();

		if (k && k.keyName == "safe_key")
		{
			GetComponent<AudioSource>().Play();
			load.SetActive(true);
			door.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}

	void Start ()
    {
		door.GetComponent<Rigidbody> ().isKinematic = true;
		load.SetActive (false);
	}
}
