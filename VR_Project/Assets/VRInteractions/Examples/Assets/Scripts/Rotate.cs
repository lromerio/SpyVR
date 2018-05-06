using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates game object by specified value continuously
/// </summary>
public class Rotate : MonoBehaviour {

    /// <summary>
    /// rotation to apply
    /// </summary>
    [Tooltip("rotation to apply")]
    public Vector3 rotation = new Vector3(0, 0, 0.02f);
	public bool RandomizeRotationRange = false;
	void OnEnable()
	{
		if (RandomizeRotationRange) {
			Vector3 newRot = new Vector3 (RandomRamngeBinamial(rotation.x), RandomRamngeBinamial(rotation.y), RandomRamngeBinamial(rotation.z));
			rotation = newRot;
		}
	}

	public float RandomRamngeBinamial(float _range)
	{
		float newValue = RandomBinamial () * _range;
		return Random.Range (-newValue, newValue);
	}
	public float RandomBinamial()
	{
		return UnityEngine.Random.Range(-1.0f, 1.0f) - UnityEngine.Random.Range(0, 1);
	}


	void Update () {    
            gameObject.transform.Rotate(rotation);  
	}
}
