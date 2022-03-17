using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    public List<GameObject> Characters;
    public int currentCharacter;
    public GameObject canvas;
    public GameObject textSeconds;

    [SerializeField] private bool gameOver = false;
    [SerializeField] private float timeTilRestart;
    [SerializeField] private float currentTimeTilRestart;
    public void GameOver() {
        gameOver = true;
        timeTilRestart = currentTimeTilRestart = 5;
    }
    void checkCharactersInExit() {
        bool levelClear = true;
        foreach (GameObject character in Characters) {
            if (character.activeSelf) {
                GameObject exit = GameObject.Find(character.name + "_Exit");
                if (Mathf.Abs(character.transform.position.x - exit.transform.position.x) >= 0.1f ||
                    Mathf.Abs(character.transform.position.y - exit.transform.position.y) >= 0.1f) {
                    levelClear = false;
                    break;
                }
            }
        }
        // Debug.Log(levelClear + " " + SceneManager.GetActiveScene().buildIndex + " " + (SceneManager.sceneCountInBuildSettings));
        if (levelClear)
            Debug.Log("YOU WIN!!!");
        if (levelClear && SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        if (gameOver) {
            canvas.SetActive(true);
            textSeconds.GetComponent<Text>().text = "" + (Mathf.Ceil(currentTimeTilRestart));
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
