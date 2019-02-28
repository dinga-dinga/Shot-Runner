using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class SafeSpot
{
    public float value = 0;

    public SafeSpot(float value)
    {
        this.value = value;
    }
}

[RequireComponent(typeof(SoldierActions))]
public class EvasiveManeuver : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float minDist;
    public float maxDist;
    public float fireDelay;
    public float movementSpeed;
    public float backRayDistance = 1.0f;
    public SafeSpot[] safeSpots;

    private bool shouldFire = true;
    private bool shouldWalk = true;
    private bool walkToCenter = false;
    private float targetManeuver;
    private GameObject player;
    private GameController gameController;

    private SoldierActions actions;

    void Start()
    {
        actions = transform.Find("Soldier").GetComponent<SoldierActions>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        // Apply spacial ability to soldiers
        float middleOfLaneWidth = (gameController.gameWidth / gameController.numberOfLanes) / 2;
        float laneOffsetStart = -(gameController.gameWidth / 2);

        safeSpots = new SafeSpot[gameController.numberOfLanes - 1];
        for (int j = 0; j < gameController.numberOfLanes - 1; j++)
        {
            safeSpots[j] = new SafeSpot(laneOffsetStart + (j + 1) * 2 * middleOfLaneWidth);
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

        Quaternion leftAngle = Quaternion.AngleAxis(-10, new Vector3(0, 1, 0));
        Quaternion rightAngle = Quaternion.AngleAxis(10, new Vector3(0, 1, 0));

        Vector3 behindDirection = new Vector3(0, 0, 1);
        Vector3 leftDirection = leftAngle * behindDirection;
        Vector3 rightDirection = rightAngle * behindDirection;

        // DrawRay as required
        Debug.DrawRay(startPosition, behindDirection * backRayDistance, Color.red);
        Debug.DrawRay(startPosition, leftDirection * backRayDistance, Color.red);
        Debug.DrawRay(startPosition, rightDirection * backRayDistance, Color.red);

        bool enemyBehind = (Physics.Raycast(startPosition, behindDirection * backRayDistance, out hit) && hit.transform.tag == "Enemy");
        bool enemyBehindLeft = (Physics.Raycast(startPosition, leftDirection * backRayDistance, out hit) && hit.transform.tag == "Enemy");
        bool enemyBehindRight = (Physics.Raycast(startPosition, rightDirection * backRayDistance, out hit) && hit.transform.tag == "Enemy");

        return enemyBehind || enemyBehindLeft || enemyBehindRight;
        
    }

    void FixedUpdate()
    {
        if (IsVehicleBehind())
        {
            walkToCenter = true;
        }

        if (walkToCenter)
        {
            float lastValue = 999;
            SafeSpot safeSpot = new SafeSpot(0);
            foreach (var spot in safeSpots)
            {
                float newValue = Math.Abs(transform.position.x - spot.value);
                if (newValue < lastValue)
                {
                    lastValue = newValue;
                    safeSpot = spot;
                }
            }

            if (Math.Abs(transform.position.x - safeSpot.value) < 0.5)
            {
                walkToCenter = false;
            }
            else
            {
                Vector3 center = new Vector3(safeSpot.value, transform.position.y, transform.position.z);
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
