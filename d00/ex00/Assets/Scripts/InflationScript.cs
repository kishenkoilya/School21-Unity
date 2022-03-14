using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InflationScript : MonoBehaviour
{    
    private float balVolume;
    private float maxBalVolume;
    public float maxInflationRatio;
    private DateTime lifeTime;
    private bool gameOver;
    public GameObject Fatigue;
    // Start is called before the first frame update
    void Start()
    {
        maxBalVolume = transform.localScale.x * transform.localScale.y / 4 * Mathf.PI * maxInflationRatio;
        balVolume = transform.localScale.x * transform.localScale.y / 4 * Mathf.PI;
        lifeTime = DateTime.Now;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) {
            if (Input.GetKeyDown(KeyCode.Space) && Fatigue.transform.GetChild(0).gameObject.transform.localScale.x < 0.91) {
                transform.localScale += new Vector3(0.1f, 0.1f, 0); 
                Fatigue.transform.GetChild(0).gameObject.transform.localScale += new Vector3(0.1f, 0, 0);
            }   
            else if (transform.localScale.x > 0.2f && transform.localScale.y > 0.2f) {
                transform.localScale += new Vector3(-0.1f * Time.deltaTime, -0.1f * Time.deltaTime, 0);
                if (Fatigue.transform.GetChild(0).gameObject.transform.localScale.x > 0) {
                    Fatigue.transform.GetChild(0).gameObject.transform.localScale += new Vector3(-0.15f * Time.deltaTime, 0, 0);
                }
            }
            balVolume = transform.localScale.x * transform.localScale.y / 4 * Mathf.PI;
            if (balVolume >= maxBalVolume) {
                Destroy(gameObject);
                Debug.Log("Balloon life time: " + ((DateTime.Now - lifeTime).Minutes * 60 + (DateTime.Now - lifeTime).Seconds) + "s");
                gameOver = true;
            }
            else if (transform.localScale.x <= 0.2f && transform.localScale.y <= 0.2f) {
                Debug.Log("Balloon life time: " + (DateTime.Now - lifeTime).Seconds + "s");
                gameOver = true;
            }
        }
    }
}
