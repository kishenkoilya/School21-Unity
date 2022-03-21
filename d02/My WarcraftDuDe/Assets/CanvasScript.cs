using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject youWin;
    [SerializeField] private GameObject restarting;
    [SerializeField] private GameObject restartingSec;
    [SerializeField] private float restartingTimeLeft;
    [SerializeField] private bool restart = false;
    // Start is called before the first frame update
    void Start()
    {
        restartingTimeLeft = 5;
    }
    public void GameOver() {
        gameOver.SetActive(true);
        restarting.SetActive(true);
        restartingSec.SetActive(true);
        restart = true;
    }
    public void YouWin() {
        youWin.SetActive(true);
        restarting.SetActive(true);
        restartingSec.SetActive(true);
        restart = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (restart) {
            restartingTimeLeft -= Time.deltaTime;
            restartingSec.GetComponent<UnityEngine.UI.Text>().text = "" + Mathf.Ceil(restartingTimeLeft) + " seconds";
            if (restartingTimeLeft <= 0) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
