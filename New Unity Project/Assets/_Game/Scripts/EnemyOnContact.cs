using UnityEngine;

public class EnemyOnContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;
    public bool isShotResistant;
    public bool isDemolisher;
    public bool isHamas;
    public int scoreValuePerShot;
    public int hitsToTake;
    private GameController gameController;
    private SoldierActions actions;
    private Transform soldierCharacter;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        soldierCharacter = transform.Find("Soldier");
        actions = soldierCharacter.GetComponent<SoldierActions>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHamas)
        {
            if (other.tag != "PShot")
            {
                return;
            }

            print(other);
            Destroy(other.gameObject);
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            actions.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
            gameController.AddScore(scoreValuePerShot);
            hitsToTake--;

            if (hitsToTake == 0)
            {
                GetComponent<EvasiveManeuver>().SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
                actions.SendMessage("Death", SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject, 4);
                gameController.AddScore(scoreValue);
                gameController.GameTotalWin();
            }
        }
        else
        {
            if (other.tag == "EnemyAI" && tag == "Enemy")
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }
            else if (other.tag == "Enemy" && tag == "Enemy")
            {
                var fractScrip = other.GetComponent<SwapFractured>();
                if (fractScrip != null)
                {
                    fractScrip.SpawnFracturedObject(explosion);
                }
            }
            else if (other.tag == "PShot")
            {
                if (!isShotResistant)
                {
                    var fractScrip = GetComponent<SwapFractured>();
                    if (fractScrip != null)
                    {
                        fractScrip.SpawnFracturedObject(explosion);
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
}
