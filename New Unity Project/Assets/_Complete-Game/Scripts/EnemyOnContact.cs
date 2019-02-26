using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;
    public bool isShotIndestructible;
    public bool isDemolisher;
    private Done_GameController gameController;

    void Start()
    {
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var fractScrip = other.GetComponent<SwapFractured>();
            if (fractScrip != null)
            {
                fractScrip.SpawnFracturedObject();
            }
            else if (explosion != null)
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }
        }
        else if (other.tag == "PShot")
        {
            if (!isShotIndestructible)
            {
                var fractScrip = GetComponent<SwapFractured>();
                if (fractScrip != null)
                {
                    fractScrip.SpawnFracturedObject();
                }
                else if (explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(gameObject);
                }

                gameController.AddScore(scoreValue);
            }

            Destroy(other.gameObject);
        }
    }
}
