using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {


	// Public attributes
    [HideInInspector]
    public bool shown = false;
    public float radius;
    public float max_angle;
    public float obj_scale;
	public uint object_count;

	public GameObject slot_prefab;

	public class InventorySlot {
		public GameObject slot;
		public GameObject content;
		public GameObject shown;
	}

	public InventorySlot[] inventory;
    
	// Private attributes
	private Dictionary<GameObject, InventorySlot> objToSlot;
	private Dictionary<GameObject, InventorySlot> shownToSlot;

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
		inventory = new InventorySlot[object_count];

		objToSlot = new Dictionary<GameObject, InventorySlot> ();

		for (int i = 0; i < object_count; i++) {
			GameObject slot =  Instantiate (slot_prefab, this.transform);
			inventory [i] = new InventorySlot ();
			inventory[i].slot = slot;
			slot.transform.position = transform.position;
			slot.transform.localScale *= obj_scale/1.5f;
			float instAngle = Mathf.Lerp(-max_angle, max_angle, i / (float)(inventory.Length)) + Mathf.PI/2;
			var offset = new Vector3(Mathf.Cos(instAngle), 0, Mathf.Sin(instAngle))*radius;
			slot.transform.Translate(transform.TransformVector(offset),Space.World);
			objToSlot.Add(slot, inventory [i]);
			slot.SetActive (false);
			slot.layer = LayerMask.NameToLayer("Inventory Items");
		}
    }

    void ShowInventory()
    {
        ControllerGrabObject cont = GetComponent<ControllerGrabObject>();
        cont.ReleaseObject();

		shownToSlot = new Dictionary<GameObject, InventorySlot>();
		foreach(InventorySlot slot in inventory)
        {
			slot.slot.SetActive (true);
			if (slot.content == null)
				continue;
			GameObject prefab = slot.content;
			GameObject inst = Instantiate(prefab);
			inst.transform.parent = this.transform;
            inst.SetActive(true);

            inst.GetComponent<Rigidbody>().isKinematic = true;
            inst.GetComponent<Rigidbody>().useGravity = false;
            inst.layer = LayerMask.NameToLayer("Inventory Items");
            //inst.GetComponent<Rigidbody>(). = false;


            //Experimental
            /*Material m = inst.GetComponent<Renderer>().material;
            m.SetOverrideTag("RenderType", "Opaque");
            m.SetFloat("_Mode", 3);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_APLHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
            Color c = m.color;
            c.a = 0.1f;
            inst.GetComponent<Renderer>().material.color = c;*/

            inst.transform.position = slot.slot.transform.position;
            inst.transform.localScale *= obj_scale / inst.GetComponent<Renderer>().bounds.size.magnitude;
            inst.tag = "InventoryItem";
			slot.shown = inst;
			shownToSlot.Add (inst, slot);
        }
        shown = true;
    }

    public GameObject TakeObject(GameObject item)
    {
		var slot = shownToSlot[item];
		GameObject obj = Instantiate(slot.content) as GameObject;
		slot.shown = null;
		slot.content = null;
        Destroy(item);
        obj.SetActive(true);
        return obj;
    }

	public GameObject TakeSlot(GameObject s)
	{
		var obj = objToSlot [s].shown;
		if (!obj)
			return null;
		return TakeObject (obj);
	}

    public bool PutObject(GameObject slot_inst, GameObject obj)
    {
		InventorySlot slot = objToSlot [slot_inst];
		if (!shown || slot.content != null) return false;
        HideInventory();
        slot.content = Instantiate(obj) as GameObject;
		slot.content.SetActive(false);
        ShowInventory();
        return true;
    }

    void HideInventory()
    {

		foreach(InventorySlot slot in inventory)
        {
			slot.slot.SetActive (false);
			if (slot.shown) {
				Destroy (slot.shown);
				slot.shown = null;
			}
        }
        shown = false;
    }

    void Start () {
		// TODO initiallization goes here
	}
	
	void Update () {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            ShowInventory();
        }

        if(Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            HideInventory();
        }
    }
}
