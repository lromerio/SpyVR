using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CablesEvent : UnityEvent<Cables>{}


public class Cables : MonoBehaviour {
    [System.Serializable]
    public struct NamedMaterial
    {
        public string name;
        public Material material;
    }

    public int cable_count;
    public List<GameObject> cables_prefabs;
    public List<GameObject> cut_cables;
	public CablesEvent cables_on_fail;
	public CablesEvent cables_on_sucess;

    private List<GameObject> cable_instances = new List<GameObject>();

    //public List<Material> colors;
    public List<NamedMaterial> colors;
    private Dictionary<string, Material> colors_by_name = new Dictionary<string, Material>();
	private Dictionary<Material, string> name_by_color = new Dictionary<Material, string> ();
	private HashSet<string> present_colors;
	private string to_cut_color;

    void Awake ()
    {
        foreach(NamedMaterial color in colors)
        {
            colors_by_name.Add(color.name, color.material);
			name_by_color.Add (color.material, color.name);
        }
    }

    public void Cut(Cable cable)
    {
        Material m = Instantiate(cable.GetComponent<Renderer>().material) as Material;
        GameObject cut_inst = Instantiate(cut_cables[Random.Range(0, cut_cables.Count)],transform);
        cut_inst.GetComponent<Renderer>().material = m;
        cut_inst.transform.localPosition = cable.transform.localPosition;
		if (CableColor (cable.gameObject) != to_cut_color) {
			//Failed! Trigger alarm!
			cables_on_fail.Invoke(this);
		} else {
			UpdatePresentColors ();
			if (!present_colors.Contains (to_cut_color)) {
				//All cable Cut! Sucess
				cables_on_sucess.Invoke(this);
			}
		}
        DestroyObject(cable.gameObject);
        //TODO notify cable cut to make puzzle progress
    }

	string CableColor(GameObject cable) {
		return name_by_color [cable.GetComponent<Renderer> ().material];
	}

	public string ChooseToCutColor() {
		string[] array = new string[present_colors.Count];
		to_cut_color = array[Random.Range (0, present_colors.Count)];
		return to_cut_color;
	}

	void UpdatePresentColors() {
		present_colors = new HashSet<string> ();
		foreach (GameObject inst in cable_instances) {
			present_colors.Add (CableColor(inst));
		}
	}

	// Use this for initialization
	void Start () {
		//Generate random cables
        foreach(int i in System.Linq.Enumerable.Range(0,cable_count))
        {
			NamedMaterial nm = colors [Random.Range (0, colors.Count)];
            Material m = nm.material;
            GameObject prefab = cables_prefabs[Random.Range(0, cables_prefabs.Count)];
            GameObject inst = Instantiate(prefab, transform);
            inst.GetComponent<Renderer>().material = m;
            BoxCollider bc = inst.AddComponent<BoxCollider>();
            inst.AddComponent<Cable>().cables = this;
            Bounds meshBounds = inst.GetComponent<MeshRenderer>().bounds;
            //bc.center = meshBounds.center;
            //bc.size = meshBounds.size;
            bc.isTrigger = true;
            cable_instances.Add(inst);
            float w = 0.2f;
            inst.transform.localPosition = Vector3.Lerp(new Vector3(-w, 0f, 0f), new Vector3(w,0f,0f),(float)i/(float)cable_count);
        }
		UpdatePresentColors ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
