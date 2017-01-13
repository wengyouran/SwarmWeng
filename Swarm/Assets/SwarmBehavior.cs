using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmBehavior : MonoBehaviour {

	/// the number of drones 
	public int droneCount = 50;
	public float spawnRadius = 100f;
	public List<GameObject> drones;

	public Vector2 swarmBounds = new Vector2(300f, 300f);

	public GameObject prefab;

	protected virtual void Start () {
		if (prefab == null)
		{
			// end early
			Debug.Log("Please assign a drone prefab.");
			return;
		}

		// instantiate the drones
		GameObject droneTemp;
		drones = new List<GameObject>();
		for (int i = 0; i < droneCount; i++)
		{
			droneTemp = (GameObject) GameObject.Instantiate(prefab);
			DroneBehavior db = droneTemp.GetComponent<DroneBehavior>();
			db.drones = this.drones;
			db.swarm = this;

			// spawn inside circle
			Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnRadius;
			droneTemp.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
			droneTemp.transform.parent = transform;
			
			drones.Add(droneTemp);
		}
	}
	

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(swarmBounds.x, 0f, swarmBounds.y));
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}
}
