using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerTest : MonoBehaviour {
    Light l;
    void Awake()
    {
        l = GetComponent<Light>();
    }
    public void disable_me(VRButton button)
    {
        l.enabled = !l.enabled;
    }

    public void set_intesity(VRLever lever, float value, float valuecache)
    {
        l.intensity = value;
    }
}
