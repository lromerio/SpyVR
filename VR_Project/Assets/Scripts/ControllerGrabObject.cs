using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerGrabObject : MonoBehaviour {

	// Private attributes
    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    public GameObject inventoryController;
    private GameObject objectInHand;
    public GameObject pliersModel;
    public GameObject controllerModel;
    public Color hightlightColor;
    public float grabDistance;
    private Color black = new Color(0, 0, 0, 1);
    enum ControllerState {
        GRABNMOVE,
        PLIER
    }

    ControllerState state;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        pliersModel.SetActive(false);
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    bool interactive(GameObject obj)
    {
        return obj.CompareTag("Grabable") || obj.CompareTag("Storable") || obj.CompareTag("InventoryItem");
    }

    private void SetCollidingObject(Collider col)
    {
        

        if (col.gameObject.GetComponent<Inventory>())
        {
            inventoryController = col.gameObject;
        }
        if (interactive(col.gameObject))
        {
            hightlight(col.gameObject);
            if(col.gameObject != collidingObject) //If new object hovered
            {
                //Vibrate
                Controller.TriggerHapticPulse(3999);
            }
            collidingObject = col.gameObject;
        }
    }

    /*public void OnTriggerEnter(Collider other)
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
        if (other.gameObject.GetComponent<Inventory>())
        {
            inventoryController = null;
        }
        Renderer r = collidingObject.GetComponent<Renderer>();
        if (r) r.material.SetColor("_EmissionColor", black);
        collidingObject = null;
    }*/

    private void GrabObject()
    {
        if(collidingObject.tag == "InventoryItem")
        {
			// Extract object from inventory
            collidingObject = collidingObject.GetComponentInParent<Inventory>().TakeObject(collidingObject);
            collidingObject.transform.position = transform.position;
        }
        
        // Grab object
        if(collidingObject && (collidingObject.CompareTag("Grabable") || collidingObject.CompareTag("Storable")))
        {     
            objectInHand = collidingObject;
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
            collidingObject = null;
        }
    }


    private ConfigurableJoint AddFixedJoint()
    {
        // Add fixed joint to emulate phisics of grab
        ConfigurableJoint fx = gameObject.AddComponent<ConfigurableJoint>();
        fx.breakForce = 30000;
        fx.breakTorque = 30000;
        fx.xMotion = ConfigurableJointMotion.Limited;
        fx.yMotion = ConfigurableJointMotion.Limited;
        fx.zMotion = ConfigurableJointMotion.Limited;
        fx.angularXMotion = ConfigurableJointMotion.Limited;
        fx.angularYMotion = ConfigurableJointMotion.Limited;
        fx.angularZMotion = ConfigurableJointMotion.Limited;
        fx.linearLimitSpring = new SoftJointLimitSpring{spring=10000f,damper=1f};
        fx.linearLimit = new SoftJointLimit {limit=0.0f,bounciness=0f,contactDistance=0};
        return fx;
    }

    public void ReleaseObject()
    {
        if (GetComponent<ConfigurableJoint>())
        {
			// Remove fixed point
            GetComponent<ConfigurableJoint>().connectedBody = null;
            Destroy(GetComponent<ConfigurableJoint>());

			// Allow to throw objects
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        
        if(inventoryController && objectInHand.CompareTag("Storable"))
        {
            // Insert object in the inventory
            hightlight(objectInHand, false);
            if(inventoryController.GetComponent<Inventory>().PutObject(objectInHand))
                Destroy(objectInHand);
        }

        objectInHand = null;
    }

    void hightlight(GameObject obj, bool high = true)
    {
        Renderer r = obj.GetComponent<Renderer>();
        if (r) r.material.SetColor("_EmissionColor", high ? hightlightColor : black);
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

        //Cast a ray
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit,grabDistance) && state == ControllerState.GRABNMOVE)
        {
            if (hit.collider.gameObject != collidingObject && collidingObject)
                hightlight(collidingObject, false);

            SetCollidingObject(hit.collider);
        }
        else if(collidingObject)
        {
            hightlight(collidingObject, false);
            collidingObject = null;
            inventoryController = null;
        }

        if (state == ControllerState.PLIER)
        {
            float val = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
            pliersModel.GetComponent<Pliers>().set_closed_value(val);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu)){
            switch(state)
            {
                case ControllerState.GRABNMOVE:
                    pliersModel.SetActive(true);
                    controllerModel.SetActive(false);
                    state = ControllerState.PLIER;
                    break;
                case ControllerState.PLIER:
                    pliersModel.SetActive(false);
                    controllerModel.SetActive(true);
                    state = ControllerState.GRABNMOVE;
                    break;
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
