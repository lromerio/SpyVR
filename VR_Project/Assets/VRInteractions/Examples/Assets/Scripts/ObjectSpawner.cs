using UnityEngine;
using System.Collections;

/// <summary>
/// Example script. Spawns randomly positioned objects over a set duration.
/// </summary>
public class ObjectSpawner : MonoBehaviour {

	public Transform ObjectPrefab;

	public float MaxSpawnDistance = 3;
	public float SpawnDelay = 0.10f;
	public VRLever EnergyLever;
	public VRLever PistonLever;

	public int SpawnNumber = 10;

	public void Activate()
	{
		Debug.Log ("Activingting" + EnergyLever.Value);
		if ((EnergyLever.Value * (float)SpawnNumber) > 1f) {
			
			PistonLever.Value = 0;
			PistonLever.Interactable = false;
			EnergyLever.Interactable = false;
			StartCoroutine (SpawnObjects ());	
			GetComponent<AudioSource>().Play();

		} else {
			Debug.Log ("ObjectSpawner: not enough spawns");
		}


	}

	protected bool isSpawning = false;

	public Vector3 GetRandomPointAroundPosition(Vector3 _pos, float _maxDistance)
	{
		_pos.x += Random.Range (-_maxDistance, _maxDistance);
		_pos.y += Random.Range (-_maxDistance, _maxDistance);
		_pos.z += Random.Range (-_maxDistance, _maxDistance);
		return _pos;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>The objects.</returns>
	IEnumerator SpawnObjects()
	{
		if (isSpawning == true)
			yield break;

		yield return new WaitForSeconds (1.5f);

		int spawnCount = (int)(EnergyLever.Value * SpawnNumber);

		//EnergyLever.Value = 0;
		float energyCost = EnergyLever.Value / SpawnNumber;

		while (spawnCount > 0) {

			Instantiate(ObjectPrefab, GetRandomPointAroundPosition(transform.position, MaxSpawnDistance), Quaternion.identity);
			EnergyLever.Value -= energyCost;

			spawnCount--;

			yield return new WaitForSeconds (SpawnDelay);
		}

		EnergyLever.Interactable = true;
		PistonLever.Interactable = true;
	}

	public bool test = false;
	void Update()
	{
		if (test == true) {
			test = false;
			Activate ();
		}
	}
}
