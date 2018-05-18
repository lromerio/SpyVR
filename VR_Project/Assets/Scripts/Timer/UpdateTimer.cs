using UnityEngine;

public class UpdateTimer : MonoBehaviour {

    private TextMesh countdownText;

	void Start ()
    {
        countdownText = gameObject.GetComponent<TextMesh>();
	}
	
	void Update ()
    {
        Timer timer = GameObject.Find("GlobalTimer").GetComponent<Timer>();
		countdownText.text = timer.GetTimeLeft();
	}
}
