using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInputTest : MonoBehaviour {

	
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	void Update () {
		
		// Test user inputs
        if (Controller.GetAxis() != Vector2.zero)
        {
            Debug.Log(gameObject.name + Controller.GetAxis());
        }
        if (Controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }
        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
    }
}
