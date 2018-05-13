using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    public float waitingTime = 0.3f;

	void Start() {
		GetComponent<Light> ().enabled = false;
	}

	public void StartAlarm(Cables _) {
		StartCoroutine (StartAlarm ());
	}

	IEnumerator StartAlarm()
    {
		GetComponent<AudioSource> ().Play ();
        while (true)
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled; //toggle on/off the enabled property
            yield return new WaitForSeconds(waitingTime);
        }
    }
}
