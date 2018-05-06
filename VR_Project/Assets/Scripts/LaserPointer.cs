using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

	// Public attributes
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    public GameObject laserPrefab;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    
    // Private attributes
    private SteamVR_TrackedObject trackedObj;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    private bool shouldTeleport;


    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	// Turn on teleport laser and transform it according to current position
    private void ShowLaser(RaycastHit hit)
    {
	    laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);
    }

	// Teleport ad turn off laser
    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }

    void Start () {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }
    
    void Update () {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
			
			// Update laser position
            RaycastHit hit;
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);

                reticle.SetActive(true);
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                shouldTeleport = true;
            }
        }
        else
        {
			// Turn off laser
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip) && shouldTeleport)
        {
			// Teleport
            Teleport();
        }
    }
}
