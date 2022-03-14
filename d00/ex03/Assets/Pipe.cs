using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public List<GameObject> pipes;
    public float speed = 1;
    public bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) {
            foreach (GameObject pipe in pipes) {
                pipe.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                if (pipe.transform.position.x <= -25)
                pipe.transform.position += new Vector3(50, 0, 0);
                speed += 0.5f * Time.deltaTime;
            }
        }
    }
}
