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
            inst.SetActive(true);
            shownToInv.Add(inst, prefab);

            shownInstances.Add(inst);

            inst.GetComponent<Rigidbody>().isKinematic = true;
            inst.GetComponent<Rigidbody>().useGravity = false;
            inst.layer = LayerMask.NameToLayer("Inventory Items");
            //inst.GetComponent<Rigidbody>(). = false;


            //Experimental
            Material m = inst.GetComponent<Renderer>().material;
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
            inst.GetComponent<Renderer>().material.color = c;

            inst.transform.position = transform.position;
            inst.transform.localScale *= obj_scale / inst.GetComponent<Renderer>().bounds.size.magnitude;
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
        print(invItem);
        print("Removed");
        GameObject obj = Instantiate(invItem) as GameObject;
        inventory.Remove(invItem);
        Destroy(item);
        obj.SetActive(true);
        return obj;
    }

    public bool PutObject(GameObject obj)
    {
        if (!shown) return false;
        HideInventory();
        GameObject foo = Instantiate(obj) as GameObject;
        inventory.Add(foo);
        foo.SetActive(false);
        ShowInventory();
        return true;
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
