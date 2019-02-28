﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnContact : MonoBehaviour
{
    public GameObject explosion;
	public GameObject playerExplosion;
	private GameController gameController;

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
	}

	void OnTriggerEnter (Collider other)
	{
        if (other.tag == "Boundary" || other.tag == "Terrain" || other.tag == "ExplosionParts")
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

		Instantiate(playerExplosion, transform.position, transform.rotation);
		gameController.GameOver();
		Destroy (gameObject);
	}
}
