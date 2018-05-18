﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    private float remainingTime;
    private bool gameOver;
    public float waitingTime;

    [HideInInspector]
    public bool alarmOn;
    [HideInInspector]
    public static bool started;


    void Start ()
    {
        waitingTime = 0.3f;
        started = false;
        gameOver = false;
        alarmOn = false;
    }

    public void StartTimer()
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

    IEnumerator Alarm()
    {
        GetComponent<AudioSource>().Play();
        int c = 10;
        while (c >= 0)
        {
            alarmOn = !alarmOn;
            yield return new WaitForSeconds(waitingTime);
            --c;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


	public void GameOver()
    {
		remainingTime = 0.0f;
		StartCoroutine(Alarm());
		gameOver = true;
	}

    void Update ()
    {
        if (started && !gameOver)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime < 0)
            {
				GameOver ();
            }
        }
    }
}
