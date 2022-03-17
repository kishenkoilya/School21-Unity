using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 shootingDirection;
    public float maxLifeTime;
    [SerializeField] private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == gameObject.name || collider.gameObject.tag == "Platform") {
            Destroy(gameObject);
        }
        else if (collider.gameObject.tag == "Player") {
            // Debug.Log(collision.gameObject.name);
            if (collider.gameObject.GetComponent<SpriteRenderer>().color == gameObject.GetComponent<SpriteRenderer>().color) {
                CharacterInteraction script = collider.gameObject.GetComponent<CharacterInteraction>();
                script.GameOver();
                Destroy(gameObject);
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += shootingDirection * Time.deltaTime;
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime) 
            Destroy(gameObject);
    }
}
