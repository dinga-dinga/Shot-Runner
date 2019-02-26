using UnityEngine;
using System.Collections;

public class Done_EvasiveManeuver : MonoBehaviour
{
	public Done_Boundary boundary;
	public float dodge;
	public float smoothing;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;

    public float minDist;
    public float maxDist;
    public float movementSpeed;

    private float targetManeuver;
    private GameObject player;

	void Start ()
	{
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

            if (Vector3.Distance(transform.position, player.transform.position) >= minDist)
            {
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = transform.forward * movementSpeed;
        }
    }
	
	IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}
	
	void FixedUpdate ()
	{
        //if (player != null)
        //{
        //    if (Vector3.Distance(transform.position, player.transform.position) >= minDist)
        //    {
        //        transform.position += transform.forward * movementSpeed * Time.deltaTime;
        //    }
        //}

        //GetComponent<Rigidbody>().velocity = new Vector3 (0.0f, 0.0f, movementSpeed);
	}
}
