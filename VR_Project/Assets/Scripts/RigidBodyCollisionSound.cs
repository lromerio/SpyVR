using UnityEngine;

public class RigidBodyCollisionSound : MonoBehaviour {

    private AudioSource source;
    public float bounceVolume;
    public float soundThreshold;

    void Start ()
    {
        source = GetComponent<AudioSource>();
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
