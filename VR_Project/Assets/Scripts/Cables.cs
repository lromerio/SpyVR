using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private List<GameObject> cable_instances = new List<GameObject>();

    //public List<Material> colors;
    public List<NamedMaterial> colors;
    private Dictionary<string, Material> colors_by_name = new Dictionary<string, Material>();

    void Awake ()
    {
        foreach(NamedMaterial color in colors)
        {
            colors_by_name.Add(color.name, color.material);
        }
    }

    public void cut(Cable cable)
    {
        Material m = Instantiate(cable.GetComponent<Renderer>().material) as Material;
        GameObject cut_inst = Instantiate(cut_cables[Random.Range(0, cut_cables.Count)],transform);
        cut_inst.GetComponent<Renderer>().material = m;
        cut_inst.transform.localPosition = cable.transform.localPosition;
        DestroyObject(cable.gameObject);
        //TODO notify cable cut to make puzzle progress
    }

	// Use this for initialization
	void Start () {
		//Generate random cables
        foreach(int i in System.Linq.Enumerable.Range(0,cable_count))
        {
            Material m = colors[Random.Range(0, colors.Count)].material;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
