using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTransform;
    [SerializeField] private LayerMask clickableObjects;
    [SerializeField] private Vector3 lClickStartPos;
    [SerializeField] private List<UnitScript> chosenUnits;
    [SerializeField] private (float, float) offsetLimits;
    [SerializeField] private GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        selectionAreaTransform.gameObject.SetActive(false);
    }

    public void GameOver() {
        canvas.GetComponent<CanvasScript>().GameOver();
    }
    public void YouWin() {
        canvas.GetComponent<CanvasScript>().YouWin();
    }
    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>

    (float, float) ChangeOffsets((float, float) offsets) {
        if (offsets == offsetLimits) {
            offsetLimits = (offsetLimits.Item1 + 1, offsetLimits.Item2 + 1);
            return (offsetLimits.Item1 - 1, offsetLimits.Item2);
        }
        else if (offsets.Item1 > -offsetLimits.Item1 && offsets.Item2 == offsetLimits.Item2) return (--offsets.Item1, offsets.Item2);
        else if (offsets.Item1 == -offsetLimits.Item1 && offsets.Item2 > -offsetLimits.Item2) return (offsets.Item1, --offsets.Item2);
        else if (offsets.Item1 < offsetLimits.Item1 && offsets.Item2 == -offsetLimits.Item2) return (++offsets.Item1, offsets.Item2);
        else if (offsets.Item1 == offsetLimits.Item1 && offsets.Item2 < offsetLimits.Item2) return (offsets.Item1, ++offsets.Item2);
        else return offsets;
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < chosenUnits.Count; i++) {
            if (!chosenUnits[i]) chosenUnits.RemoveAt(i);
        }
        if (Input.GetMouseButtonDown(0)) {
            lClickStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectionAreaTransform.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0)) {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(lClickStartPos.x, currentMousePosition.x),
                Mathf.Min(lClickStartPos.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(
                Mathf.Max(lClickStartPos.x, currentMousePosition.x),
                Mathf.Max(lClickStartPos.y, currentMousePosition.y));
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        if (Input.GetMouseButtonUp(0)) {
            selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(lClickStartPos, Camera.main.ScreenToWorldPoint(Input.mousePosition), clickableObjects, -1, 1);
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) {
                foreach (UnitScript unit in chosenUnits) {
                    foreach (Transform frame in unit.frames) {
                        frame.gameObject.SetActive(false);
                    }
                }
                chosenUnits.Clear();
            }
            foreach (Collider2D collider in collider2DArray) {
                if (collider.tag == "Player") {
                    UnitScript unit = collider.GetComponent<UnitScript>();
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                        if (chosenUnits.Find(x => x == unit)) continue;
                    if (unit != null) {
                        chosenUnits.Add(unit);  
                        foreach(Transform frame in unit.frames) frame.gameObject.SetActive(true);                      
                    }
                }
            }
            if (chosenUnits.Count > 0) {
                chosenUnits[0].SoundSelected();
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                                                                    Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                                                                    clickableObjects, -1, 1);
            if (collider2DArray.Length > 0) {
                if (collider2DArray[0].gameObject.tag != chosenUnits[0].gameObject.tag) {
                    foreach(UnitScript unit in chosenUnits) unit.currentAttackTarget = collider2DArray[0].gameObject;
                    chosenUnits[0].SoundAcknowledged();
                }
            }
            else {
                (float, float) offsets = (0,0);
                offsetLimits = (0, 0);
                Vector3 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                foreach (UnitScript unit in chosenUnits) {
                    unit.currentDestination = new Vector3(destination.x + offsets.Item1, destination.y + offsets.Item2, destination.z);
                    unit.currentAttackTarget = null;
                    offsets = ChangeOffsets(offsets);
                }
                if (chosenUnits.Count > 0) {
                    chosenUnits[0].SoundAcknowledged();
                }
            }
        }
        
    }
}
