using UnityEngine;

public class TerrainOnContact : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PShot" || other.tag == "EShot")
        {
            Destroy(other.gameObject);
        }
    }
}