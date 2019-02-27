using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SoldierActions))]
public class Done_EvasiveManeuver : MonoBehaviour
{
    public Done_Boundary boundary;
    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    public GameObject shot;
    public Transform shotSpawn;
    public float minDist;
    public float maxDist;
    public float fireDelay;
    public float movementSpeed;

    private bool shouldFire = true;
    private bool shouldWalk = true;
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

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player.transform);

            if (Vector3.Distance(transform.position, player.transform.position) <= maxDist)
            {
                if (shouldFire)
                {
                    print("ShouldFire");
                    shouldFire = false;
                    shouldWalk = false;
                    actions.SendMessage("Aiming", SendMessageOptions.DontRequireReceiver);
                    StartCoroutine(Fire());
                    StartCoroutine(FireDelay(fireDelay));
                }
            }

            if (Vector3.Distance(transform.position, player.transform.position) >= minDist && shouldWalk)
            {
                actions.SendMessage("Walk", SendMessageOptions.DontRequireReceiver);
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
                transform.Find("Soldier").transform.position = transform.position;
            }
            else
            {
                actions.SendMessage("Aiming", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            actions.SendMessage("Walk", SendMessageOptions.DontRequireReceiver);
            GetComponent<Rigidbody>().velocity = transform.forward * movementSpeed;
            transform.Find("Soldier").transform.position = transform.position;
        }
    }

    IEnumerator Fire()
    {
        print("Fire");
        // This sould be set accordingly to the aim animation
        yield return new WaitForSeconds(0.6f);
        print("Fire time");
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
        shouldWalk = true;
    }

    IEnumerator FireDelay(float fireDelay)
    {
        print("FireDelay");
        yield return new WaitForSeconds(fireDelay);
        print("FireDelay time");
        shouldFire = true;
    }
}
