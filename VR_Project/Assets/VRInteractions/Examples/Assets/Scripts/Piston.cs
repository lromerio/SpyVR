using UnityEngine;
using System.Collections;

/// <summary>
/// Piston. Simple component that moves an object between it's start position and a target position lerped by Value
/// </summary>
public class Piston : ExposableMonobehaviour {


	public Vector3 TargetPosition;

	public float TransitionTime = 3f;

	[SerializeField]
	private float mCurrentValue = 0;
	[SerializeField]
	private float mValueTarget = 0.6f;

	private Vector3 StartPosition;

	void Awake()
	{
		StartPosition = transform.position;

	}

	void Reset()
	{
		TargetPosition = transform.position;
	}

	private IEnumerator Function;

	/// <summary>
	/// The value of the piston (or arm, door, contraption). 1 is the maximum position 0 is the minimum position (the start position)
	/// </summary>
	/// <value>The value.</value>
	[ExposeProperty]
	public float Value
	{
		get {
			return mValueTarget;
		}
		set {
			mValueTarget = Mathf.Clamp(value, 0,1); 

			if (mValueTarget < mCurrentValue) {
				if (Function != null)
					StopCoroutine (Function);
			
				Function = MoveToTarget (false); // Keep a cache of the coroutine so we can stop it later
				StartCoroutine (Function);
			} else if (mValueTarget > mCurrentValue) {
				if (Function != null)
					StopCoroutine (Function);

				Function = MoveToTarget (true);
				StartCoroutine (Function);
			}
		}
	}

	/// <summary>
	/// Moves to target.
	/// Moves the piston to the position that is the lerp of start pos and target pos by value
	/// </summary>
	/// <returns>The to target.</returns>
	/// <param name="up">If set to <c>true</c> up.</param>
	IEnumerator MoveToTarget(bool up)
	{
		Vector3 newPosition;

		if (up == true) { // If we are lerping upwards use the lower clamp
			while (mValueTarget != mCurrentValue) {
				mCurrentValue = Mathf.Clamp (mCurrentValue + (Time.fixedDeltaTime / TransitionTime), 0, mValueTarget);

				newPosition = Vector3.Lerp (StartPosition, StartPosition+TargetPosition, mCurrentValue);
				transform.position = newPosition;

				yield return new WaitForFixedUpdate ();
			}
		} else { // Clamp downwards. Code duplication because it's slightly faster.
			while (mValueTarget != mCurrentValue) {
				
				mCurrentValue = Mathf.Clamp (mCurrentValue - (Time.fixedDeltaTime / TransitionTime), mValueTarget, 1);
				newPosition = Vector3.Lerp (StartPosition , StartPosition+TargetPosition, mCurrentValue);

				transform.position = newPosition;

				yield return new WaitForFixedUpdate ();
			}
		}

		Function = null;
	}

}
