using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInteraction : MonoBehaviour
{
    public GameObject currentCharacter;
    public float speed, jumpHeight;
    [SerializeField] private float yCoordinate;
    [SerializeField] private int timesYCoordinateNoChange;
    private float jumpTime;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void GameOver() {
        gameOver = true;
        GameObject.Find("Main Camera").GetComponent<CameraScript>().GameOver();
    }
    void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log("collision: " + collision.GetContact(0).point.y + " floor: " + (collision.transform.position.y + collision.transform.localScale.y / 2) + 
        // " " + collision.collider.name + " " + gameObject.name);
        
        if (collision.gameObject.tag != gameObject.name && collision.gameObject.tag != "Platform" && collision.gameObject.tag != "Player") {
            // Debug.Log("not listed: " + collision.gameObject.name + "  " + collision.gameObject.tag);
            // Debug.Log((collision.gameObject.tag != gameObject.name) + " " + (collision.gameObject.tag != "Platform") + " " + (collision.gameObject.tag != "Player"));
            // gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
        }
        // else if (Mathf.Abs(collision.GetContact(0).point.y - (collision.transform.position.y + collision.transform.localScale.y / 2)) <= 0.1) {
        //     isGrounded = true;
        // }
        // else if (collision.gameObject.tag == gameObject.name || collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Player") {
        //     // Debug.Log("listed: " + collision.gameObject.name + "  " + collision.gameObject.tag);
        //     // Debug.Log((collision.gameObject.tag == gameObject.name) + " " + (collision.gameObject.tag == "Platform") + " " + (collision.gameObject.tag == "Player"));
        //     // gameObject.GetComponent<BoxCollider2D>().enabled = true;
        // }

        
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag != gameObject.name && collision.gameObject.tag != "Platform" && collision.gameObject.tag != "Player") {
            // Debug.Log("not listed: " + collision.gameObject.name + "  " + collision.gameObject.tag);
            // Debug.Log((collision.gameObject.tag != gameObject.name) + " " + (collision.gameObject.tag != "Platform") + " " + (collision.gameObject.tag != "Player"));
            // gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
        }
    }
    void MoveLeft() {
        transform.position += new Vector3 (-speed * Time.deltaTime, 0, 0);
    }
    void MoveRight() {
        transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
    }
    void Jump() {
        if (isGrounded) {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpHeight);
            isGrounded = false;
        }
    }
    bool CheckIfGrounded() {
        for (int i = -5; i <= 5; i++) {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (gameObject.transform.localScale.x / 10 - 0.02f) * i, 
                            gameObject.transform.position.y - gameObject.transform.localScale.y / 2 - 0.01f),
                            Vector2.down);
            if (hit.collider != null) {
                Debug.Log(hit.point.y + " " + (gameObject.transform.position.y - gameObject.transform.localScale.y / 2) + " " + 
                (hit.point.y - (gameObject.transform.position.y - gameObject.transform.localScale.y / 2)));
                if (Mathf.Abs(hit.point.y - (gameObject.transform.position.y - gameObject.transform.localScale.y / 2)) <= 0.018){
                    return true;
                }
            }
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameOver) {
            if (currentCharacter == gameObject) {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
                    Jump();
                }
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    MoveLeft();
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    MoveRight();
                }
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.R)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            isGrounded = CheckIfGrounded();
            // if (yCoordinate == transform.position.y) {
            //     Debug.Log("true: " + yCoordinate + "  " + transform.position.y + "  " + (yCoordinate == transform.position.y));
            //     timesYCoordinateNoChange++;
            // } else {
            //     Debug.Log("false: " + yCoordinate + "  " + transform.position.y + "  " + (yCoordinate == transform.position.y));
            //     timesYCoordinateNoChange = 0;
            //     yCoordinate = transform.position.y;
            // }
                
            // if (timesYCoordinateNoChange >= 100) {
            //     isGrounded = true;
            // }
        }
        else {
            if (transform.localScale.x > 0)
                transform.localScale -= transform.localScale * Time.deltaTime;
        }
    }
}
