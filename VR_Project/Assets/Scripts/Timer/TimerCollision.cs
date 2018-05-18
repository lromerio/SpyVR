using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimerEvent : UnityEvent{ }

public class TimerCollision : MonoBehaviour {

    public GameObject toCheckFor;
    public TimerEvent startTimer;
	private bool started;

    private void Start()
    {
        started = false;
    }

	void OnTriggerEnter(Collider collision)
    {
		if (!started && collision.gameObject == toCheckFor)
        {
            startTimer.Invoke();
            started = true;
        }
    }
}
