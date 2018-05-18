using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public float remainingTime;
    private bool gameOver;
    public float waitingTime;

    [HideInInspector]
    public bool alarmOn;
    [HideInInspector]
    public bool started;


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

	public void StopTimer()
	{
		started = false;
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
		if (!gameOver) {
			remainingTime = 0.0f;
			StartCoroutine (Alarm ());
			gameOver = true;
		}
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
