using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public float remainingTime;
    public static bool started;
    private bool gameOver;
    public float waitingTime = 0.3f;
    public bool alarmOn;

    // Use this for initialization
    void Start () {
        remainingTime = 10.0f;
        started = true;
        gameOver = false;
        alarmOn = false;
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

    // Update is called once per frame
    void Update () {
        if (started && !gameOver)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime < 0)
            {
                remainingTime = 0.0f;
                StartCoroutine(Alarm());
                gameOver = true;
            }
        }
    }
}
