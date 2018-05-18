using UnityEngine;
using System;

public class UpdateTimer : MonoBehaviour {

    private TextMesh countdownText;
	private float lastMod;
	private Timer timer;
	private AudioSource bip;

	void Start ()
    {
		bip = gameObject.GetComponent<AudioSource>();
        countdownText = gameObject.GetComponent<TextMesh>();
		timer = GameObject.Find("GlobalTimer").GetComponent<Timer>();
		lastMod = 0;

		countdownText.text = FormatTime (timer.remainingTime);
	}

	private string FormatTime(float remainingTime)
	{
		TimeSpan t = TimeSpan.FromSeconds(remainingTime);
		string s = string.Format("{0:D2}:{1:D2}:{2:D3}",
			t.Minutes,
			t.Seconds,
			t.Milliseconds);
		
		return s;
	}

	private float BipRate(float t)
	{
		if (t <= 5)
			return 0.5f;
		else if (t <= 10)
			return 1f;
		else if  (t <= 30)
			return 2.0f; 
		else if (t <= 60)
			return 5.0f;
		else if(t <= 90)
			return 10.0f;

		return 30.0f;
	}
	
	void Update ()
    {
		if (timer.started)
		{
			float remainingTime = timer.remainingTime;
			float mod = Mathf.Repeat (remainingTime, BipRate(remainingTime));

			if (mod > lastMod)
				bip.Play ();

			lastMod = mod;

			countdownText.text = FormatTime (remainingTime);
		}
	}
}
