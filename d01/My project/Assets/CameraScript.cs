using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    public List<GameObject> Characters;
    public int currentCharacter;
    public List<GameObject> canvasObjects;

    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool levelClear = false;
    [SerializeField] private float timeTilRestart;
    [SerializeField] private float currentTimeTilRestart;
    [SerializeField] private float timeTilNextLevel;
    [SerializeField] private float currentTimeTilNextLevel;
    public void GameOver() {
        if (levelClear) 
            return;
        gameOver = true;
        timeTilRestart = currentTimeTilRestart = 5;
        canvasObjects[0].SetActive(true);
        canvasObjects[2].SetActive(true);
        canvasObjects[2].GetComponent<UnityEngine.UI.Text>().text = "Restarting in:";
        canvasObjects[3].SetActive(true);
        canvasObjects[4].SetActive(false);
    }
    void checkCharactersInExit() {
        bool levelClearLocal = true;
        foreach (GameObject character in Characters) {
            if (character.activeSelf) {
                GameObject exit = GameObject.Find(character.name + "_Exit");
                if (Mathf.Abs(character.transform.position.x - exit.transform.position.x) >= 0.1f ||
                    Mathf.Abs(character.transform.position.y - exit.transform.position.y) >= 0.1f) {
                    levelClearLocal = false;
                    break;
                }
            }
        }
        // Debug.Log(levelClear + " " + SceneManager.GetActiveScene().buildIndex + " " + (SceneManager.sceneCountInBuildSettings));
        if (levelClearLocal) {
            levelClear = true;
            timeTilNextLevel = currentTimeTilNextLevel = 3;
            canvasObjects[1].SetActive(true);
            canvasObjects[2].SetActive(true);
            canvasObjects[2].GetComponent<UnityEngine.UI.Text>().text = "Next level in:";
            canvasObjects[3].SetActive(true);
            canvasObjects[4].SetActive(false);        
        }
    }
    void CameraOnCharacter(int index) {
        //if active
        currentCharacter = index;
        transform.position = new Vector3 (Characters[index].transform.position.x, Characters[index].transform.position.y, -10);
    }

    void SelectCharacter(int index) {
        foreach (GameObject character in Characters) {
            character.GetComponent<CharacterInteraction>().currentCharacter = Characters[index];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach(GameObject ga in GameObject.FindGameObjectsWithTag("Player")) {
            Characters[i] = ga;
            i++;
        }
        SelectCharacter(0);
        CameraOnCharacter(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelClear) {
            canvasObjects[3].GetComponent<Text>().text = "" + (Mathf.Ceil(currentTimeTilNextLevel));
            currentTimeTilNextLevel -= Time.deltaTime;
            if (currentTimeTilNextLevel <= 0) {
                if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
        else if (gameOver) {
            canvasObjects[3].GetComponent<Text>().text = "" + (Mathf.Ceil(currentTimeTilRestart));
            currentTimeTilRestart -= Time.deltaTime;
            if (currentTimeTilRestart <= 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        else {
            checkCharactersInExit();
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SelectCharacter(0);
                CameraOnCharacter(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SelectCharacter(1);
                CameraOnCharacter(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SelectCharacter(2);
                CameraOnCharacter(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                SelectCharacter(3);
                CameraOnCharacter(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                SelectCharacter(4);
                CameraOnCharacter(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                SelectCharacter(5);
                CameraOnCharacter(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7)) {
                SelectCharacter(6);
                CameraOnCharacter(6);
            }
            else
                CameraOnCharacter(currentCharacter);
        }
    }
}
