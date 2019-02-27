using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SoldierActions))]
public class Done_EvasiveManeuver : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float minDist;
    public float maxDist;
    public float fireDelay;
    public float movementSpeed;
    public float roadCenter = 0;

    private bool shouldFire = true;
    private bool shouldWalk = true;
    private bool walkToCenter = false;
    private float targetManeuver;
    private GameObject player;

    private SoldierActions actions;

    void Start()
    {
        actions = transform.Find("Soldier").GetComponent<SoldierActions>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void MoveForwards()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
        Transform soldier = transform.Find("Soldier");
        if (soldier != null)
        {
            soldier.transform.position = transform.position;
        }
    }

    private bool IsVehicleBehind()
    {
        RaycastHit hit;
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Vector3 behindDirection = new Vector3(0, 0, 1);
        return Physics.Raycast(startPosition, behindDirection, out hit);
    }

    void FixedUpdate()
    {
        if (IsVehicleBehind())
        {
            walkToCenter = true;
        }

        if (walkToCenter)
        {
            if ((int)transform.position.x == (int)roadCenter)
            {
                Debug.Log("=false!!");
                walkToCenter = false;
            }
            else
            {
                Vector3 center = new Vector3(roadCenter, transform.position.y, transform.position.z);
                transform.LookAt(center);
                MoveForwards();
                return;
            }
        }

        if (player == null)
        {
            if (actions != null)
            {
                actions.SendMessage("Walk", SendMessageOptions.DontRequireReceiver);
            }
            MoveForwards();
            return;
        }

        transform.LookAt(player.transform);

        if (Vector3.Distance(transform.position, player.transform.position) <= maxDist)
        {
            if (shouldFire)
            {
                shouldFire = false;
                shouldWalk = false;
                if (actions != null)
                {
                    actions.SendMessage("Aiming", SendMessageOptions.DontRequireReceiver);
                }
                StartCoroutine(Fire());
                StartCoroutine(FireDelay(fireDelay));
            }
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= minDist && shouldWalk)
        {
            if (actions != null)
            {
                actions.SendMessage("Walk", SendMessageOptions.DontRequireReceiver);
            }
            MoveForwards();
        }
        else if (actions != null)
        {
            actions.SendMessage("Aiming", SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator Fire()
    {
        // This sould be set accordingly to the aim animation
        yield return new WaitForSeconds(0.6f);
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
        shouldWalk = true;
    }

    IEnumerator FireDelay(float fireDelay)
    {
        yield return new WaitForSeconds(fireDelay);
        shouldFire = true;
    }
}
