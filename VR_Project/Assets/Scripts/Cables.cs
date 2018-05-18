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

	public struct CableInstance {
		public CableInstance(GameObject o, string c){
			inst = o; color = c;
		}
		public GameObject inst;
		public string color;
	}

    public int cableCount;
    public List<GameObject> cablesPrefabs;
    public List<GameObject> cutCables;
	public CablesEvent cablesOnFail;
	public CablesEvent cablesOnSucess;
	public bool failed = false;
    public List<NamedMaterial> colors;

    private List<CableInstance> cable_instances = new List<CableInstance>();
	private HashSet<string> present_colors;
	private string to_cut_color;

    public void Cut(Cable cable)
    {
        Material m = Instantiate(cable.GetComponent<Renderer>().material) as Material;
        GameObject cut_inst = Instantiate(cutCables[Random.Range(0, cutCables.Count)],transform);
        cut_inst.GetComponent<Renderer>().material = m;
        cut_inst.transform.localPosition = cable.transform.localPosition;

		string cut_color = CableColor (cable.gameObject);
		print ("cut : " + cut_color);

		if(cut_color != to_cut_color)
        {
			//Failed! Trigger alarm!
			failed = true;
			cablesOnFail.Invoke(this);
		}
        else
        {
			cable_instances.Remove (cable_instances.Find (p => p.inst == cable.gameObject));
			UpdatePresentColors ();

			if (!present_colors.Contains (to_cut_color) && !failed)
            {
				//All cable Cut! Sucess
				GetComponent<AudioSource>().Play();
				cablesOnSucess.Invoke(this);
			}
		}

        DestroyObject(cable.gameObject);
    }

	string CableColor(GameObject cable)
    {
		return cable_instances.Find (p => p.inst == cable).color;
	}

	public string ChooseToCutColor()
    {
		string[] array = new string[present_colors.Count];
		present_colors.CopyTo (array);
		to_cut_color = array[Random.Range (0, present_colors.Count)];
		return to_cut_color;
	}

	void UpdatePresentColors()
    {
		present_colors = new HashSet<string> ();
		foreach (CableInstance inst in cable_instances) {
			present_colors.Add(inst.color);
		}
	}

	void Start ()
    {
		//Generate random cables
        foreach(int i in System.Linq.Enumerable.Range(0,cableCount))
        {
			NamedMaterial nm = colors [Random.Range (0, colors.Count)];
            Material m = nm.material;
            GameObject prefab = cablesPrefabs[Random.Range(0, cablesPrefabs.Count)];
            GameObject inst = Instantiate(prefab, transform);
            inst.GetComponent<Renderer>().material = m;
            BoxCollider bc = inst.AddComponent<BoxCollider>();
            inst.AddComponent<Cable>().cables = this;
            Bounds meshBounds = inst.GetComponent<MeshRenderer>().bounds;
            //bc.center = meshBounds.center;
            //bc.size = meshBounds.size;
            bc.isTrigger = true;
			cable_instances.Add(new CableInstance(inst,nm.name));
            float w = 0.2f;
            inst.transform.localPosition = Vector3.Lerp(new Vector3(-w, 0f, 0f), new Vector3(w,0f,0f),(float)i/(float)cableCount);
        }

		UpdatePresentColors ();
	}
}
