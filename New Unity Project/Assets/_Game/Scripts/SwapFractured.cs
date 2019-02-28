using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapFractured : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject fracturedObject;

    public void SpawnFracturedObject(GameObject explosion = null)
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        Destroy(originalObject);
        GameObject fractObj = Instantiate(fracturedObject, originalObject.transform.position, originalObject.transform.rotation) as GameObject;
        fractObj.GetComponent<ExplodeObject>().Explode();
    }
}
