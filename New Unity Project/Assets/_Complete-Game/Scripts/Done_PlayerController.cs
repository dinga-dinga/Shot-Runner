using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

[RequireComponent(typeof(SoldierActions))]
public class Done_PlayerController : MonoBehaviour
{
    public float speed;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

    private bool notShooting = true;
    private float nextFire;
    private Done_GameController gameController;
    private SoldierActions actions;
    private Transform soldierCharacter;

    void Start()
    {
        soldierCharacter = transform.Find("SoldierIsrael");
        actions = soldierCharacter.GetComponent<SoldierActions>();
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void Update ()
    {
        Camera topCamera = gameController.TopCamera();
        Vector3 mousePos = topCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 0.0f;
        GetComponent<Rigidbody>().transform.LookAt(mousePos);

        if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
            nextFire = Time.time + fireRate;
            StartCoroutine(Fire());
        }
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
        Rigidbody rigCom = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        // TODO: Fine tune movement - its slow and stuttery
        rigCom.velocity = movement * speed;

        soldierCharacter.transform.position = rigCom.position;
        rigCom.position = new Vector3
        (
            Mathf.Clamp(rigCom.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rigCom.position.z, boundary.zMin, boundary.zMax)
        );

        if (actions != null && notShooting)
        {
            actions.SendMessage("Walk", SendMessageOptions.DontRequireReceiver);
        }
        soldierCharacter.transform.position = rigCom.position;
    }

    IEnumerator Fire()
    {
        notShooting = false;
        if (actions != null)
        {
            actions.SendMessage("Aiming", SendMessageOptions.DontRequireReceiver);
        }
        yield return new WaitForSeconds(0.6f);
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
        notShooting = true;
    }
}
