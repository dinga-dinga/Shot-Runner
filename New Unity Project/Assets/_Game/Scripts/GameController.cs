using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class Hazard
{
    public GameObject obj;
    public int chance;
}

public class GameController : MonoBehaviour
{
    public Hazard[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float gameOverWait;

    public Camera topCamera;
    public Camera[] otherCameras;

    public float gameWidth;
    public int numberOfLanes;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private bool restart;
    private int score;
    private int currentCameraIndex;
    private int chancesSum;
    private Camera[] cameras;
    private bool paused;
    private bool allowedToContinue;
    private string textBeforePause;

    void Start()
    {
        restart = false;
        paused = false;
        allowedToContinue = true;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        currentCameraIndex = 0;

        chancesSum = 0;
        foreach (var hazard in hazards)
        {
            chancesSum += hazard.chance;
        }

        Time.timeScale = 1;

        cameras = new Camera[otherCameras.Length + 1];
        cameras[0] = topCamera;
         
         //Turn all cameras off, except the first default one
         for (int i=1; i<cameras.Length; i++)
         {
             cameras[i] = otherCameras[i - 1];
             cameras[i].gameObject.SetActive(false);
         }
         
         //If any cameras were added to the controller, enable the first one
         if (cameras.Length>0)
         {
             cameras[0].gameObject.SetActive(true);
             Debug.Log ("Camera with name: " + cameras [0].GetComponent<Camera>().name + ", is now enabled");
         }

        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
         //If the c button is pressed, switch to the next camera
         //Set the camera at the current index to inactive, and set the next one in the array to active
         //When we reach the end of the camera array, move back to the beginning or the array.
         if (Input.GetKeyDown(KeyCode.C))
         {
             currentCameraIndex ++;
             Debug.Log ("C button has been pressed. Switching to the next camera");
             if (currentCameraIndex < cameras.Length)
             {
                 cameras[currentCameraIndex-1].gameObject.SetActive(false);
                 cameras[currentCameraIndex].gameObject.SetActive(true);
                 Debug.Log ("Camera with name: " + cameras [currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
             }
             else
             {
                 cameras[currentCameraIndex-1].gameObject.SetActive(false);
                 currentCameraIndex = 0;
                 cameras[currentCameraIndex].gameObject.SetActive(true);
                 Debug.Log ("Camera with name: " + cameras [currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
             }
         }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void Pause(bool gameOver = false)
    {
        if (!paused)
        {
            if (!gameOver)
            {
                textBeforePause = gameOverText.text;
                gameOverText.text = "Game Paused";
            }

            Time.timeScale = 0;
            paused = true;
        }
        else if (allowedToContinue)
        {
            gameOverText.text = textBeforePause;
            Time.timeScale = 1;
            paused = false;
        }
    }

    private GameObject GetRandomHazard()
    {
        int remaining = Random.Range(0, chancesSum);

        foreach (var hazard in hazards)
        {
            remaining -= hazard.chance;

            if (remaining < 0)
            {
                return hazard.obj;
            }
        }

        return null;
    }

    IEnumerator SpawnWaves()
    {
        float middleOfLaneWidth = (gameWidth / numberOfLanes) / 2;
        float laneOffsetStart = -(gameWidth / 2);

        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = GetRandomHazard();
                int spawnLane = Random.Range(0, numberOfLanes);
                Vector3 spawnPosition = new Vector3(
                    laneOffsetStart + middleOfLaneWidth + (spawnLane * 2 * middleOfLaneWidth),
                    spawnValues.y,
                    spawnValues.z);
                Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);

                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator GameOverEnumerator()
    {
        yield return new WaitForSeconds(gameOverWait);
        allowedToContinue = false;
        Pause(true);
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        restartText.text = "Press 'R' tp restart";
        restart = true;

        StartCoroutine(GameOverEnumerator());
    }

    public Camera SelectedCamera()
    {
        return cameras[currentCameraIndex];
    }

    public Camera TopCamera()
    {
        return topCamera;
    }

    public void GameWon()
    {
        gameOverText.text = "You Won!";
        restartText.text = "Press 'R' tp restart\n" +
                           "Press 'N' to start the next level";
        restart = true;
    }

    public void GameTotalWin()
    {
        gameOverText.text = "You Win!";
        restartText.text = "Press 'R' tp restart";
        restart = true;

        StartCoroutine(GameOverEnumerator());
    }
}
