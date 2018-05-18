using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCollisionSound : MonoBehaviour {
    AudioSource source;
    public float bounceVolume;
    public float soundThreshold;
	public AudioClip sound;

	// Use this for initialization
	void Start () {
		source = gameObject.AddComponent<AudioSource> ();
		source.spatialBlend = 1f;
		source.clip = sound;
		source.playOnAwake = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        float magni = collision.relativeVelocity.magnitude;
        if (magni > soundThreshold)
        {
            source.volume = Mathf.Clamp(magni * bounceVolume, 0, 1);
            source.Play();
        }
    }
}
