using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObject : MonoBehaviour
{
    public float minForce;
    public float maxForce;
    public float minDespawnTime;
    public float maxDespawnTime;
    public float radius;
    
    public void Explode()
    {
        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }
        }

        Destroy(gameObject, Random.Range(minDespawnTime, maxDespawnTime));
    }
}
