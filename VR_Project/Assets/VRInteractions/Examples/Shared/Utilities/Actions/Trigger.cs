// example consumer
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
	
}


public class Trigger : MonoBehaviour {
	// must derive a new class because templates and inheritance don't seem to trigger the custom drawer
	[System.Serializable]
	public class UnityAction : Action<Collider> { }

	[Action(typeof(void), typeof(Collider))]
	public UnityAction enters, stays, exits;

	public UnityEvent<int> MyEvent;


	public MyIntEvent m_MyEvent;




	public void OnTriggerEnter(Collider collider) {
		Debug.Log ("here");

		enters.action(collider);

	}

	public void OnTriggerStays(Collider collider) {
		stays.action(collider);
	}

	public void OnTriggerExit(Collider collider) {
		exits.action(collider);
	}

	public void Awake() {
		enters.Awake();
		stays.Awake();
		exits.Awake();
	}
}