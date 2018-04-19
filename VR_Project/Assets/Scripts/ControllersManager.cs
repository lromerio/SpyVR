using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersManager : MonoBehaviour {

    public static ControllersManager Instance;

    public ControllerGrabObject leftController;
    public ControllerGrabObject rightController;

    private List<GameObject> inventory;

    private void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
