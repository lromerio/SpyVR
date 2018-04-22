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
	public List<GameObject> inventory;
    
	// Private attributes
    private List<GameObject> shownInstances = new List<GameObject>();
    private Dictionary<GameObject, GameObject> shownToInv;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void ShowInventory()
    {
        ControllerGrabObject cont = GetComponent<ControllerGrabObject>();
        cont.ReleaseObject();

        shownInstances = new List<GameObject>();
        shownToInv = new Dictionary<GameObject, GameObject>();

        float angle = max_angle;
        for(int i = 0; i < inventory.Count; i++)
        {
            GameObject prefab = inventory[i];
            GameObject inst = Instantiate(prefab,this.transform);
            shownToInv.Add(inst, prefab);

            shownInstances.Add(inst);

            inst.GetComponent<Rigidbody>().isKinematic = true;
            inst.GetComponent<Rigidbody>().useGravity = false;

            inst.transform.position = transform.position;
            inst.transform.localScale *= obj_scale;
            inst.tag = "InventoryItem";

            float instAngle = Mathf.Lerp(-angle, angle, i / (float)(inventory.Count)) + Mathf.PI/2;
            var offset = new Vector3(Mathf.Cos(instAngle), 0, Mathf.Sin(instAngle))*radius;
            
            inst.transform.Translate(transform.TransformVector(offset),Space.World);
        }
        shown = true;
    }

    public GameObject TakeObject(GameObject item)
    {
        shownInstances.Remove(item);
        var invItem = shownToInv[item];
        var obj = Instantiate(invItem);
        inventory.Remove(invItem);
        Destroy(item);
        return obj;
    }

    public void PutObject(GameObject obj)
    {
        HideInventory();
        inventory.Add(Instantiate(obj));
        ShowInventory();
    }

    void HideInventory()
    {

        foreach(var go in shownInstances)
        {
            Destroy(go);
        }
        shownInstances.Clear();
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

        if(Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shownInstances.Count > 0)
        {
            HideInventory();
        }
    }
}
