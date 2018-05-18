using UnityEngine;

public class ListenerTest : MonoBehaviour {

    private Light l;

    void Awake()
    {
        l = GetComponent<Light>();
    }

    public void Trigger(VRButton button)
    {
        l.enabled = !l.enabled;
    }

    public void SetIntensity(VRLever lever, float value, float valuecache)
    {
        l.intensity = value;
    }
}
