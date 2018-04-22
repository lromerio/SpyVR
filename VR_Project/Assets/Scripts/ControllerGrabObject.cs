using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

	// Private attributes
    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject touchingObject;
    private GameObject objectInHand;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            touchingObject = col.gameObject;
            return;
        }
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        touchingObject = null;
        collidingObject = null;
    }

    private void GrabObject()
    {
        if(collidingObject.tag == "InventoryItem")
        {
			// Extract object from inventory
            collidingObject = collidingObject.GetComponentInParent<Inventory>().TakeObject(collidingObject);
            collidingObject.transform.position = transform.position;
        }
        
        // Grab object
        objectInHand = collidingObject;
        collidingObject = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
		// Add fixed joint to emulate phisics of grab
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
			// Remove fixed point
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

			// Allow to throw objects
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        
        if(touchingObject && touchingObject.GetComponent<Inventory>())
        {
			// Insert object in the inventory
            touchingObject.GetComponent<Inventory>().PutObject(objectInHand);
            Destroy(objectInHand);
        }
        
        objectInHand = null;
    }

    // Update is called once per frame
    void Update () {
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }
}
