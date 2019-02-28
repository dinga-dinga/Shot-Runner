using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoldierActions))]
public class PlayerOnContact : MonoBehaviour
{
    public GameObject explosion;
	public GameObject playerExplosion;
	private GameController gameController;
    private SoldierActions actions;
    private Transform soldierCharacter;
    private bool alreadyDead = false;

    void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

        soldierCharacter = transform.Find("SoldierIsrael");
        actions = soldierCharacter.GetComponent<SoldierActions>();
    }

	void OnTriggerEnter (Collider other)
	{
        if (other.tag == "Boundary" || other.tag == "Terrain" || other.tag == "ExplosionParts" || alreadyDead)
		{
            return;
        }

	    if (other.tag == "FinishLine")
	    {
            gameController.GameWon();
	        Destroy(gameObject);
            return;
        }

        var fractScrip = other.GetComponent<SwapFractured>();
        if (fractScrip != null)
        {
            fractScrip.SpawnFracturedObject();
        }
        else if (explosion != null)
        {
        	Instantiate(explosion, other.transform.position, other.transform.rotation);
        }

        // TODO: DO WE WANT IT?
        //if (other.tag == "EShot")
        //{
        //    alreadyDead = true;
        //    actions.SendMessage("Death", SendMessageOptions.DontRequireReceiver);
        //    Destroy(gameObject, 4);
        //}
        //else
        //{
            Instantiate(playerExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        //}
		
		gameController.GameOver();
	}
}
