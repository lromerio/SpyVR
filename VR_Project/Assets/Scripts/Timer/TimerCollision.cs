using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimerEvent : UnityEvent{ }

public class TimerCollision : MonoBehaviour {

    public TimerEvent startTimer;
	public GameObject to_check_for;
    private bool started;

    private void Start()
    {
        started = false;
    }

	void OnTriggerEnter(Collider collision)
    {
		if (!started && collision.gameObject == to_check_for)
        {
            startTimer.Invoke();
            started = true;
        }
    }
}
