using UnityEngine;

public class DrawerSound : MonoBehaviour {

    public AudioClip noise;
    public AudioClip end;
    public float noiseThreshold;
    public float noiseVolume;
    public float endVolume;
    public float volumeAlpha;
    public float endThreshold;

    private AudioSource noiseSource;
    private AudioSource endSource;
    private float lastVol;
    private Vector3 lastPos;
    private ConfigurableJoint cj;
    private bool lastAtend;
    private Vector3 restPos;

	void Start ()
    {
        lastAtend = true;

        noiseSource = gameObject.AddComponent<AudioSource>();
        endSource = gameObject.AddComponent<AudioSource>();
        noiseSource.clip = noise;
        noiseSource.playOnAwake = false;
        noiseSource.loop = true;

        endSource.clip = end;
        endSource.playOnAwake = false;
        endSource.volume = endVolume;

        noiseSource.volume = 0;
        lastVol = 0;
        noiseSource.Play();
        lastPos = transform.position;
        cj = GetComponent<ConfigurableJoint>();
        restPos = lastPos;
    }
	
    bool AtEnd()
    {
        float dist = Vector3.Distance(restPos, transform.position);
        return dist > cj.linearLimit.limit*2 - endThreshold || dist < endThreshold;
    }

	void Update()
    {
        float velocity = (transform.position-lastPos).magnitude;
        float vol;

        if (velocity > noiseThreshold)
        {
            vol = Mathf.Clamp(velocity * noiseVolume, 0, 1);
        }
        else
        {
            vol = 0;
        }

        vol = Mathf.Lerp(lastVol, vol, volumeAlpha);
        noiseSource.volume = vol;
        lastPos = transform.position;
        lastVol = noiseSource.volume;
        bool atend = AtEnd();
        if(atend && !lastAtend)
        {
            endSource.volume = velocity * endVolume;
            endSource.Play();
        }
        lastAtend = atend;
    }
}
