using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimerEvent : UnityEvent<TimerCollision> { }

public class TimerCollision : MonoBehaviour {

    public TimerEvent startTimer;
    private bool started;

    private void Start()
    {
        started = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!started)
        {
            startTimer.Invoke(this);
            started = true;
        }
    }
}
