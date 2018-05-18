using UnityEngine;

public class AlarmLight : MonoBehaviour {

    private Light l;
    
	void Start()
    {
        l = GetComponent<Light>();
        l.enabled = false;
	}

    private void Update()
    {
        l.enabled = GameObject.Find("GlobalTimer").GetComponent<Timer>().alarmOn;
    }
}
