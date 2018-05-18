using System.Collections.Generic;
using UnityEngine;

public class ControllersManager : MonoBehaviour {

	// Public attributes
    public static ControllersManager Instance;
    public ControllerGrabObject leftController;
    public ControllerGrabObject rightController;

	// Private attributes
    private List<GameObject> inventory;

    private void Awake()
    {
        Instance = this;
    }
}
