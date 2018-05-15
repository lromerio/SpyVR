using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlarmLight : MonoBehaviour {

    Light l;
    
	void Start() {
        l = GetComponent<Light>();
        l.enabled = false;
	}

    private void Update()
    {
        l.enabled = GameObject.Find("GlobalTimer").GetComponent<Timer>().alarmOn;
    }
}
