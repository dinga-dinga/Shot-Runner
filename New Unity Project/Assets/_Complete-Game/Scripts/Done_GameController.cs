﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

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
    private Camera[] cameras;

    void Start()
    {
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        currentCameraIndex = 0;

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

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
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
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
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

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        restartText.text = "Press 'R' tp restart";
        restart = true;
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
}
