using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for interactables. Tracks whether the object is interactable or not and contains facilities for managing colliders.
/// Collider management is usefull for if you want to temporarily stop the controllers or the character from colliding with the interactable object
/// </summary>
public class VRInteractable : ExposableMonobehaviour {

	public bool Interactable = true;

	/// <summary>
	/// Requires a rigidbody
	/// </summary>
	private Rigidbody rb3d;


	/// <summary>
	/// Cached colliders for disabling collisions
	/// </summary>
	private List<Collider> mColliders3D;

	void Awake()
	{
		UpdateColliders3D ();
		mCache = Interactable;
	}

	/// <summary>
	/// Updates the cache of which colliders are in this objects hierachy.
	/// </summary>
	public void UpdateColliders3D()
	{
		
		rb3d = gameObject.GetComponent<Rigidbody> ();

		mColliders3D = new List<Collider>();

		if (rb3d != null) // I need a note on why this is done. Is rb3d.gameobject not the same as gameObject in this case?
		{
			mColliders3D.AddRange(rb3d.gameObject.GetComponentsInChildren<Collider>());
			mColliders3D.Add(rb3d.gameObject.GetComponent<Collider>());
		}
		else
		{
			mColliders3D.AddRange(GetComponentsInChildren<Collider>());
			mColliders3D.Add(GetComponent<Collider>());
		}
				
	}

	/// <summary>
	/// Ignores the colliders of the given rigidbody
	/// </summary>
	/// <param name="_rigidbody">Rigidbody.</param>
	public void IgnoreColliders(Rigidbody _rigidbody)
	{
		Collider[] colliders = _rigidbody.GetComponentsInChildren<Collider>();
		IgnoreColliders3D (colliders, mColliders3D.ToArray());
	}

	/// <summary>
	/// Ignores the colliders of the given transform
	/// </summary>
	/// <param name="_object">Object.</param>
	public void IgnoreColliders(Transform _object)
	{
		Collider[] colliders = _object.GetComponentsInChildren<Collider>();
		IgnoreColliders3D (colliders, mColliders3D.ToArray());
	}

	/// <summary>
	/// Removes the physics ignore for the given rigidbody
	/// </summary>
	/// <param name="_rigidbody">Rigidbody.</param>
	public void RemoveIgnoreColliders(Rigidbody _rigidbody)
	{
		Collider[] colliders = _rigidbody.GetComponentsInChildren<Collider>();
		IgnoreColliders3D (colliders, mColliders3D.ToArray(), false);
	}

	/// <summary>
	/// Removes the physics ignore for the given transform
	/// </summary>
	/// <param name="_object">Object.</param>
	public void RemoveIgnoreColliders(Transform _object)
	{
		Collider[] colliders = _object.GetComponentsInChildren<Collider>();
		IgnoreColliders3D (colliders, mColliders3D.ToArray(), false);
	}

	/// <summary>
	/// Allows prevention of collision between this object and your controllers colliders
	/// </summary>
	/// <param name="_bullet">Bullet to ignore collision with</param>
	/// <param name="_colliders">Colliders of object we want to ignore collisions with</param>
	/// <summary>
	public static void IgnoreColliders3D(Collider[] _colliders, Collider[] _otherColliders, bool _ignore = true)
	{
		foreach (Collider col in _colliders)
		{
			foreach (Collider otherCol in _otherColliders)
				if (otherCol == null || col == null) {
					//Debug.Log ("selfcol " + otherCol.ToString () + "  bulletcol " + col.ToString ());
				} else {
					//Debug.Log ("selfcol " + otherCol.ToString () + "  bulletcol " + col.ToString ());
					Physics.IgnoreCollision (col, otherCol, _ignore);
				}
			
		}
	}
		
// This will be used in the future for enable
//	private bool mInteractable = true;
//
//	[ExposeProperty]
//	public bool Interactable
//	{
//		get 
//		{ 
//			return mInteractable; 
//		} 
//		set {
//			mInteractable = value;
//		}
//	}

	/// <summary>
	/// Ignores colliders for all of the active steam contorollers. // This will need to be revisited if local multiplayer becomes a thing
	/// </summary>
	void IgnoreAllControllerColliders()
	{
		foreach (SteamVR_TrackedObject controller in VRGripper.GetControllers()) {
			IgnoreColliders (controller.transform);
		}
	}

	/// <summary>
	/// Removes the physics ignore for the steam controllers
	/// </summary>
	void RemoveIgnoreAllControllerColliders()
	{
		foreach (SteamVR_TrackedObject controller in VRGripper.GetControllers()) {
			RemoveIgnoreColliders (controller.transform);
		}
	}

	/// <summary>
	/// Trachs changes to interactable state
	/// </summary>
	bool mCache;

	// NOTE: This will be moved to property get/set methods.
	public void Update()
	{
		
		if (mCache != Interactable) {
			if (Interactable == false)
				IgnoreAllControllerColliders ();
			else
				RemoveIgnoreAllControllerColliders ();
				
			mCache = Interactable;
		}
	}
}

