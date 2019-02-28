using UnityEngine;


[RequireComponent(typeof(SoldierActions))]
public class HamasOnContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;
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
}
