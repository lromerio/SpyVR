using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curve;
using Tubular;


public class ControllerGrabObject : MonoBehaviour {

	// Private attributes
    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    public GameObject inventoryController;
    private GameObject objectInHand;
    public GameObject pliersModel;
    public GameObject controllerModel;
    public GameObject sparks;
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
        GenerateTube();
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

	private void vibrate(float intensity) {
		Controller.TriggerHapticPulse((ushort)(2000f*intensity));
	}

    private void GenerateTube()
    {
        var controls = new List<Vector3>() {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, grabDistance)
        };
        var curve = new CatmullRomCurve(controls);

        // Build tubular mesh with Curve
        int tubularSegments = 40;
        float radius = 0.001f;
        int radialSegments = 20;
        bool closed = false; // closed curve or not
        var mesh = Tubular.Tubular.Build(curve, tubularSegments, radius, radialSegments, closed);

        // visualize mesh
        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }

    private void GrabObject()
    {
        if(collidingObject.tag == "InventoryItem")
        {
			// Extract object from inventory
			Vector3 oldpos = collidingObject.transform.position;
            collidingObject = collidingObject.GetComponentInParent<Inventory>().TakeObject(collidingObject);
			collidingObject.transform.position = oldpos;
        }
        
        // Grab object
        if(collidingObject && (collidingObject.CompareTag("Grabable") || collidingObject.CompareTag("Storable")))
        {     
            objectInHand = collidingObject;

			bool alreadyContrained = objectInHand.GetComponent<Joint> () != null;

			var joint = AddFixedJoint(alreadyContrained);
			if(!alreadyContrained) StartCoroutine (MoveObjectToGrab (joint,objectInHand));
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
            collidingObject = null;
        }
    }


	private ConfigurableJoint AddFixedJoint(bool dontMessAnchor)
    {
        // Add fixed joint to emulate phisics of grab
        ConfigurableJoint fx = gameObject.AddComponent<ConfigurableJoint>();
        fx.breakForce = 30000;
        fx.breakTorque = 30000;
		if (!dontMessAnchor) {
			fx.autoConfigureConnectedAnchor = false;
			fx.connectedAnchor = new Vector3 (0, 0, 0);
		}
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

	void moveSparkAt(Vector3 position, Vector3 normal, Vector3 ray)
    {
		sparks.transform.position = position+0.0001f*normal;
		Vector3 dir = Vector3.Reflect (ray, normal);
		sparks.transform.forward = dir;
		sparks.SetActive (true);
    }

	IEnumerator<int> MoveObjectToGrab(ConfigurableJoint joint, GameObject obj){
		float t = 0f;
		float maxt = 0.3f;
		Vector3 startPos = transform.InverseTransformPoint(obj.transform.position);
		/*PreferedRotation rot = obj.GetComponent<PreferedRotation> ();
		if (rot) {
			Vector3 startRot = obj.transform.eulerAngles;
			while (t < maxt) {
				float factor = t / maxt;
				joint.targetRotation = Quaternion.Euler(Vector3.Lerp (startRot, rot.preferedRotation, factor));
				joint.anchor = Vector3.Lerp (startPos, new Vector3 (0, 0, 1) * grabDistance, factor);
				t += Time.deltaTime;
				yield return 0; // leave the routine and return here in the next frame
			}
		} else {*/
			while (t < maxt) {
				float factor = t / maxt;
				joint.anchor = Vector3.Lerp (startPos, new Vector3 (0, 0, 1) * grabDistance, factor);
				t += Time.deltaTime;
				yield return 0; // leave the routine and return here in the next frame
			}
		//}
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
		if (Physics.Raycast (transform.position, transform.forward, out hit, grabDistance) && state == ControllerState.GRABNMOVE) {
			if (hit.collider.gameObject != collidingObject && collidingObject)
				hightlight (collidingObject, false);

			moveSparkAt (hit.point, hit.normal, transform.forward);
			if(hit.collider.gameObject != objectInHand)
				vibrate (1 - hit.distance / grabDistance);
			SetCollidingObject (hit.collider);
		} else if (collidingObject) {
			hightlight (collidingObject, false);
			collidingObject = null;
			inventoryController = null;
			sparks.SetActive (false);
		} else {
			sparks.SetActive (false);
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
