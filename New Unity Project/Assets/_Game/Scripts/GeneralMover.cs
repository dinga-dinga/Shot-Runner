using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMover : MonoBehaviour
{
    public Vector3 speed;

    private Vector3 startPosition;
    private float startTime;

    void Start()
    {
        startPosition = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        float timePassed = Time.time - startTime;
        transform.position = startPosition + timePassed * speed;
    }
}
