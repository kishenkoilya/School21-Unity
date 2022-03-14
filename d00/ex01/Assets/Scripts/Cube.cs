using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private float speed;
    private GameObject finish;
    public KeyCode destructionKey;
    public CubeSpawner spawnerScript;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(2f, 5f);
        finish = GameObject.Find("Finish");
        spawnerScript = GameObject.Find("Spawner").GetComponent<CubeSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y < finish.transform.position.y - 0.75f) {
            Destroy(gameObject);
            spawnerScript.misses++;
        }
        if (transform.position.y >= finish.transform.position.y - 0.75f && transform.position.y < finish.transform.position.y + 0.75f && Input.GetKeyDown(destructionKey)) {
            Destroy(gameObject);
            spawnerScript.hits++;
            Debug.Log(Mathf.Abs(transform.position.y - finish.transform.position.y));
        }
    }
}
