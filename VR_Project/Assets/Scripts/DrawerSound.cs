using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerSound : MonoBehaviour {
    public AudioClip noise;
    public AudioClip end;

    private AudioSource noise_source;
    private AudioSource end_source;

    public float noise_threshold;
    public float noise_volume;
    public float end_volume;
    public float volume_alpha;

    private float last_vol;
    private Vector3 last_pos;

    private ConfigurableJoint cj;
    private bool last_atend = true;
    public float end_threshold;
    private Vector3 rest_pos;
	// Use this for initialization
	void Start () {
        noise_source = gameObject.AddComponent<AudioSource>();
        end_source = gameObject.AddComponent<AudioSource>();
        noise_source.clip = noise;
        noise_source.playOnAwake = false;
        noise_source.loop = true;

        end_source.clip = end;
        end_source.playOnAwake = false;
        end_source.volume = end_volume;

        noise_source.volume = 0;
        last_vol = 0;
        noise_source.Play();
        last_pos = transform.position;
        cj = GetComponent<ConfigurableJoint>();
        rest_pos = last_pos;
    }
	
    bool atEnd()
    {
        float dist = Vector3.Distance(rest_pos, transform.position);
        return dist > cj.linearLimit.limit*2 - end_threshold || dist < end_threshold;
    }

	// Update is called once per frame
	void Update () {
        float velocity = (transform.position-last_pos).magnitude;
        float vol;
        if (velocity > noise_threshold)
        {
            vol = Mathf.Clamp(velocity * noise_volume, 0, 1);
        }
        else
        {
            vol = 0;
        }
        vol = Mathf.Lerp(last_vol, vol, volume_alpha);
        noise_source.volume = vol;
        last_pos = transform.position;
        last_vol = noise_source.volume;
        bool atend = atEnd();
        if(atend && !last_atend)
        {
            end_source.volume = velocity * end_volume;
            end_source.Play();
        }
        last_atend = atend;
    }
}
