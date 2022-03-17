using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public GameObject doorToOpen;
    public float pressingTime;
    public Vector3 openingDirection;
    public bool doorCloses;
    public List<GameObject> doorsToChooseFrom;
    [SerializeField] private bool isPressed = false;
    [SerializeField] private bool isOpened = false;
    [SerializeField] private int isOpening = 0; //0 - nor opening nor closing, 1 - opening, -1 - closing
    [SerializeField] private float initialButtonScaleY;
    [SerializeField] private Vector3 initialDoorScale;
    [SerializeField] private Vector3 initialDoorPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialButtonScaleY = transform.localScale.y;
        if (doorToOpen) {
            initialDoorScale = doorToOpen.transform.localScale;
            initialDoorPosition = doorToOpen.transform.position;
        }
    }
    void OnTriggerEnter2D(Collider2D collider) {
        // Debug.Log("OnTriggerEnter2D");
        if (doorToOpen == null) {
            // Debug.Log("choose door");
            gameObject.GetComponent<SpriteRenderer>().color = collider.GetComponent<SpriteRenderer>().color;
            foreach (GameObject door in doorsToChooseFrom) {
                // Debug.Log("door " + door.name + " button color: " + gameObject.GetComponent<SpriteRenderer>().color + 
                // " door color: " + door.GetComponent<SpriteRenderer>().color);
                if (gameObject.GetComponent<SpriteRenderer>().color == door.GetComponent<SpriteRenderer>().color) {
                    // Debug.Log("door legit");
                    doorToOpen = door;
                    initialDoorScale = doorToOpen.transform.localScale;
                    initialDoorPosition = doorToOpen.transform.position;
                    break;
                }
            }
        }
        isPressed = true;
        isOpening = 1;
    }
    void OnTriggerExit2D(Collider2D collider) {
        isPressed = false;
        if (doorCloses) {
            isOpening = -1;
        }
    }

    void ButtonShrinkAndExpansion() {
        if (isPressed && transform.localScale.y > 0.01f) {
            transform.localScale -= new Vector3(0, initialButtonScaleY * (Time.deltaTime / pressingTime), 0);
        }
        else if (isPressed && transform.localScale.y < 0.01f) {
            transform.localScale = new Vector3(transform.localScale.x, 0.01f, transform.localScale.z);
        }

        if (!isPressed && transform.localScale.y < initialButtonScaleY) {
            transform.localScale += new Vector3(0, initialButtonScaleY * (Time.deltaTime / pressingTime), 0);
        }
        else if (!isPressed && transform.localScale.y > initialButtonScaleY) {
            transform.localScale = new Vector3(transform.localScale.x, initialButtonScaleY, transform.localScale.z);
        }
    }
    void OpenDoor() {
        // Debug.Log("opening");
        doorToOpen.transform.localScale -= openingDirection * (Time.deltaTime / pressingTime);
        doorToOpen.transform.position += (openingDirection / 2) * (Time.deltaTime / pressingTime);
        Vector3 resultingDoorScale = initialDoorScale - openingDirection;
        if (doorToOpen.transform.localScale.x <= resultingDoorScale.x &&
            doorToOpen.transform.localScale.y <= resultingDoorScale.y &&
            doorToOpen.transform.localScale.z <= resultingDoorScale.z) {
            doorToOpen.transform.localScale = initialDoorScale - openingDirection;
            isOpening = 0;
            isOpened = true;
        }
    }

    void CloseDoor() {
        doorToOpen.transform.localScale += openingDirection * (Time.deltaTime / pressingTime);
        doorToOpen.transform.position -= (openingDirection / 2) * (Time.deltaTime / pressingTime);
        if (doorToOpen.transform.localScale.x >= initialDoorScale.x &&
            doorToOpen.transform.localScale.y >= initialDoorScale.y &&
            doorToOpen.transform.localScale.z >= initialDoorScale.z) {
            doorToOpen.transform.localScale = initialDoorScale;
            doorToOpen.transform.position = initialDoorPosition;
            isOpening = 0;
            isOpened = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ButtonShrinkAndExpansion();
        if (doorToOpen) {
            if (isOpening != 0) {
                if (isOpening == 1) {
                    OpenDoor();
                }
                else if (isOpening == -1) {
                    CloseDoor();
                }
            }
        }
    }
}
