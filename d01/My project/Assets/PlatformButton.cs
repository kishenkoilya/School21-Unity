using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public GameObject platformToRecolor;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collider) {
        platformToRecolor.GetComponent<SpriteRenderer>().color = collider.GetComponent<SpriteRenderer>().color;
        platformToRecolor.tag = collider.name;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
