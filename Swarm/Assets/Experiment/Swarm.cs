using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour {
    public int droneCount = 50;
    public float spawnRadius = 100f;
    public List<GameObject> drones;

    public Vector2 swarmBounds = new Vector2(300f, 300f);

    public GameObject prefab;

    // Use this for initialization
    protected virtual void Start()
    {
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
            droneTemp = (GameObject)GameObject.Instantiate(prefab);
            Drone db = droneTemp.GetComponent<Drone>();
            db.drones = this.drones;
            db.m_swarm = this;

            // spawn inside circle
            Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnRadius;
            droneTemp.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
            droneTemp.transform.parent = transform;

            drones.Add(droneTemp);
        }
    }

}
