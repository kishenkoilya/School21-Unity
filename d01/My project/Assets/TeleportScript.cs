using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public GameObject portalExit;
    public bool justTeleported = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!justTeleported) {
            collider.transform.position = portalExit.transform.position;
        }
        portalExit.GetComponent<TeleportScript>().justTeleported = true;
    }
    void OnTriggerExit2D(Collider2D collider) {
        justTeleported = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
