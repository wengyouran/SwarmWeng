using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {
    public List<GameObject> drones;
    public Swarm m_swarm;

    public float desiredSeparation = 6f;
    public float neighborRadius = 50f;

    public float speed = 10f;
    public float maxSpeed = 20f;
    public float maxSteer = .05f;

    public Vector3 _separation;
    public Vector3 _alignment;
    public Vector3 _cohesion;
    public Vector3 _bounds;

    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float boundsWeight = 1f;

    void FixedUpdate () {
        Flock();
    }
    public virtual void Flock()
    {

        Vector3 newVelocity = Vector3.zero;

        CalculateVelocities();

        //transform.forward = _alignment;

        newVelocity += _separation * separationWeight;
        newVelocity += _alignment * alignmentWeight;
        newVelocity += _cohesion * cohesionWeight;
        newVelocity += _bounds * boundsWeight;
        newVelocity = newVelocity * speed;
        newVelocity = GetComponent<Rigidbody>().velocity + newVelocity;
        newVelocity.y = 0f;

        GetComponent<Rigidbody>().velocity = Limit(newVelocity, maxSpeed);
    }

    protected virtual void CalculateVelocities()
    {
        Vector3 separationSum = Vector3.zero;
        Vector3 alignmentSum = Vector3.zero;
        Vector3 cohesionSum = Vector3.zero;
        Vector3 boundsSum = Vector3.zero;

        int separationCount = 0;
        int alignmentCount = 0;
        int cohesionCount = 0;
        int boundsCount = 0;

        for (int i = 0; i < this.drones.Count; i++)
        {
            if (drones[i] == null) continue;

            float distance = Vector3.Distance(transform.position, drones[i].transform.position);

            // separation
            // calculate separation influence velocity for this drone, based on its preference to keep distance between itself and neighboring drones
            if (distance > 0 && distance < desiredSeparation)
            {
                // calculate vector headed away from myself
                Vector3 direction = transform.position - drones[i].transform.position;
                direction.Normalize();
                direction = direction / distance; // weight by distance
                separationSum += direction;
                separationCount++;
            }
            if (distance > 0 && distance < neighborRadius)
            {
                alignmentSum += drones[i].GetComponent<Rigidbody>().velocity;
                alignmentCount++;

                cohesionSum += drones[i].transform.position;
                cohesionCount++;
            }
            Bounds bounds = new Bounds(m_swarm.transform.position, new Vector3(m_swarm.swarmBounds.x, 10000f, m_swarm.swarmBounds.y));
            if (distance > 0 && distance < neighborRadius && !bounds.Contains(drones[i].transform.position))
            {
                Vector3 diff = transform.position - m_swarm.transform.position;
                if (diff.magnitude > 0)
                {
                    boundsSum += m_swarm.transform.position;
                    boundsCount++;
                }
            }
        }
            _separation = separationCount > 0 ? separationSum / separationCount : separationSum;
            _alignment = alignmentCount > 0 ? Limit(alignmentSum / alignmentCount, maxSteer) : alignmentSum;
            _cohesion = cohesionCount > 0 ? Steer(cohesionSum / cohesionCount, false) : cohesionSum;
            _bounds = boundsCount > 0 ? Steer(boundsSum / boundsCount, false) : boundsSum;

        }
    protected virtual Vector3 Steer(Vector3 target, bool slowDown)
    {
        // the steering vector
        Vector3 steer = Vector3.zero;
        Vector3 targetDirection = target - transform.position;
        float targetDistance = targetDirection.magnitude;

        transform.LookAt(target);

        if (targetDistance > 0)
        {
            // move towards the target
            targetDirection.Normalize();

            //two options for speed
            if (slowDown && targetDistance < 100f * speed)
            {
                targetDirection *= (maxSpeed * targetDistance / (100f * speed));
                targetDirection *= speed;
            }
            else
            {
                targetDirection *= maxSpeed;
            }

            // set steering vector
            steer = targetDirection - GetComponent<Rigidbody>().velocity;
            steer = Limit(steer, maxSteer);
        }

        return steer;
    }

    protected virtual Vector3 Limit(Vector3 v, float max)
    {
        if (v.magnitude > max)
        {
            return v.normalized * max;
        }
        else
        {
            return v;
        }
    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, neighborRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _alignment.normalized * neighborRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + _separation.normalized * neighborRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _cohesion.normalized * neighborRadius);
    }

}
