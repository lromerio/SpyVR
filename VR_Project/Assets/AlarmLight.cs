using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    public float waitingTime = 0.3f;

    IEnumerator Start()
    {
        while (true)
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled; //toggle on/off the enabled property
            yield return new WaitForSeconds(waitingTime);
        }
    }
}
