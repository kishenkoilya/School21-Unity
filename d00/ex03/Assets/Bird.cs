using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float currentFallSpeed = 0.1f;
    public float upliftSpeed, gConstant;
    public List<GameObject> collisionObjects;
    public GameObject floor;
    public GameObject Pipes;
    public bool gameOver = false;

    public void Fall() {
        transform.position -= new Vector3(0, currentFallSpeed * Time.deltaTime, 0);
        currentFallSpeed += gConstant * Time.deltaTime;
    }

    public bool CheckCollision(GameObject a, GameObject b) { //foooo
        bool CollisionX =   a.transform.position.x + a.transform.localScale.x / 2 >= b.transform.position.x - b.transform.localScale.x / 2 &&
                            b.transform.position.x + b.transform.localScale.x / 2 >= a.transform.position.x - a.transform.localScale.x / 2;
        bool CollisionY =   a.transform.position.y + a.transform.localScale.y / 2 >= b.transform.position.y - b.transform.localScale.y / 2 &&
                            b.transform.position.y + b.transform.localScale.y / 2 >= a.transform.position.y - a.transform.localScale.y / 2 ;
        return CollisionX && CollisionY;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) {//baaar
            Fall();
            foreach (GameObject obj in collisionObjects) {
                if (CheckCollision(gameObject, obj)) {
                    Pipes.GetComponent<Pipe>().gameOver = true;
                    gameOver = true;
                    transform.rotation *= Quaternion.AngleAxis(-90 * Time.deltaTime, Vector3.forward);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                currentFallSpeed = upliftSpeed;
            }

        }
        else {
            if (transform.position.y > floor.transform.position.y + floor.transform.localScale.y / 2) {
                Fall();
            }
            else {
                currentFallSpeed = 0;
                transform.position = new Vector3(0, floor.transform.position.y + floor.transform.localScale.y / 2, 0);
            }
            if (transform.rotation.eulerAngles.z >= 270) {
                transform.rotation *= Quaternion.AngleAxis(-90 * Time.deltaTime, Vector3.forward);
            }

        }
    }
}
