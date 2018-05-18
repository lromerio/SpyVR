using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curve;
using Tubular;


public class ControllerGrabObject : MonoBehaviour {

	// Private attributes
    private SteamVR_TrackedObject trackedObj;
	private GameObject tangibleObject;
	private GameObject hoveredObject;
    private GameObject objectInHand;
	private GameObject inventorySlot;
    public GameObject pliersModel;
    public GameObject controllerModel;
    public GameObject sparks;
	public Inventory inventory;
    public Color hightlightColor;
	public LayerMask inventoryMask;
    public float grabDistance;
    private Color black = new Color(0, 0, 0, 1);
    public enum ControllerState {
        GRABNMOVE,
        PLIER
    }

	[HideInInspector]
    public ControllerState state;

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
        if (interactive(col.gameObject))
        {
            hightlight(col.gameObject);
            if(col.gameObject != tangibleObject) //If new object hovered
            {
                //Vibrate
                Controller.TriggerHapticPulse(3999);
            }
            tangibleObject = col.gameObject;
        }
		hoveredObject = col.gameObject;
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
		if (inventorySlot) {
			Vector3 oldpos = inventorySlot.transform.position;
			tangibleObject = inventory.TakeSlot (inventorySlot);
			if (!tangibleObject)
				return;
			tangibleObject.transform.position = oldpos;
		}

		if (tangibleObject.tag == "InventoryItem") {
			// Extract object from inventory
			Vector3 oldpos = tangibleObject.transform.position;
			tangibleObject = tangibleObject.GetComponentInParent<Inventory> ().TakeObject (tangibleObject);
			tangibleObject.transform.position = oldpos;
		}
        
        // Grab object
        if(tangibleObject && (tangibleObject.CompareTag("Grabable") || tangibleObject.CompareTag("Storable")))
        {     
            objectInHand = tangibleObject;

			bool alreadyContrained = objectInHand.GetComponent<Joint> () != null;

			var joint = AddFixedJoint(alreadyContrained);
			if(!alreadyContrained) StartCoroutine (MoveObjectToGrab (joint,objectInHand));
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
            tangibleObject = null;
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
        
		if(inventorySlot && objectInHand.CompareTag("Storable"))
        {
            // Insert object in the inventory
            hightlight(objectInHand, false);
			if(inventory.PutObject(inventorySlot,objectInHand))
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
			while (t < maxt && joint) {
				float factor = t / maxt;
				joint.anchor = Vector3.Lerp (startPos, new Vector3 (0, 0, 1) * grabDistance, factor);
				t += Time.deltaTime;
				yield return 0; // leave the routine and return here in the next frame
			}
		//}
	}

    // Update is called once per frame
    void Update () {

		//Cast a general ray
		hoveredObject = null;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, grabDistance) && state == ControllerState.GRABNMOVE) {
			if (hit.collider.gameObject != tangibleObject && tangibleObject)
				hightlight (tangibleObject, false);

			moveSparkAt (hit.point, hit.normal, transform.forward);
			if(hit.collider.gameObject != objectInHand)
				vibrate (1 - hit.distance / grabDistance);
			SetCollidingObject (hit.collider);
		} else if (tangibleObject) {
			hightlight (tangibleObject, false);
			tangibleObject = null;
			sparks.SetActive (false);
		} else {
			sparks.SetActive (false);
		}

		//Cast a ray to inventory item and slot

		inventorySlot = null;
		RaycastHit hit2;
		if (Physics.Raycast (transform.position, transform.forward, out hit2, grabDistance, inventoryMask) && state == ControllerState.GRABNMOVE) {
			if (hit2.collider.gameObject.CompareTag("InventorySlot")) {
				inventorySlot = hit.collider.gameObject;
			}
		}

        if (Controller.GetHairTriggerDown())
        {
			if (tangibleObject || inventorySlot)
            {
                GrabObject();
            }

			print (tangibleObject);

			if (hoveredObject) { //Button and such
				ControllerTriggerable tr = hoveredObject.GetComponent<ControllerTriggerable>();
				if (tr) {
					tr.OnControllerTrigger ();
				}
			}
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
				pliersModel.SetActive (true);
				controllerModel.SetActive (false);
				GetComponent<Renderer> ().enabled = false;

                state = ControllerState.PLIER;
                break;
            case ControllerState.PLIER:
                pliersModel.SetActive(false);
                controllerModel.SetActive(true);
				GetComponent<Renderer> ().enabled = true;
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
