using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMover : MonoBehaviour
{
    public Vector3 speed;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + Time.time * speed;
    }
}
