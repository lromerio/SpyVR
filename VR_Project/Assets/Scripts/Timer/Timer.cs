using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float remainingTime;
    public static bool started;

	// Use this for initialization
	void Start () {
        remainingTime = 600.0f;
        started = false;
    }

    public static void StartTimer()
    {
        started = true;
    }

    public string GetTimeLeft()
    {

        TimeSpan t = TimeSpan.FromSeconds(remainingTime);

        string s = string.Format("{0:D2}:{1:D2}:{2:D3}",
                    t.Minutes,
                    t.Seconds,
                    t.Milliseconds);

        return s;
}
	
	// Update is called once per frame
	void Update () {
        if (started)
            remainingTime -= Time.deltaTime;
    }
}
