using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTimer : MonoBehaviour {

    private TextMesh countdownText;

	// Use this for initialization
	void Start () {
        countdownText = gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        Timer timer = GameObject.Find("GobalTimer").GetComponent<Timer>();
        countdownText.text = timer.GetTimeLeft();
	}
}
