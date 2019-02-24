using UnityEngine;
using System.Collections;

public class Done_EvasiveManeuver : MonoBehaviour
{
	public Done_Boundary boundary;
	public float tilt;
	public float dodge;
	public float smoothing;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;

    public float minDist;
    public float maxDist;

	private float currentSpeed;
    private float targetManeuver;
    private GameObject player;

	void Start ()
	{
        if (player == null)
            player = GameObject.FindWithTag("Player");

		currentSpeed = GetComponent<Rigidbody>().velocity.z;
		StartCoroutine(Evade());
	}

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player.transform);
            transform.Rotate(0, 180, 0);

            //    if(Vector3.Distance(transform.position, Player.position) <= MaxDist)
            //    {
            //         //Here Call any function U want Like Shoot at here or something
            //    } 
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
		GetComponent<Rigidbody>().velocity = new Vector3 (0.0f, 0.0f, currentSpeed);

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= minDist)
            {
                transform.position += transform.forward * currentSpeed * Time.deltaTime;
            }
        }
		
        // Todo: This should take in count z also as it's not looking in one way.
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
}
