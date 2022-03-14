using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeSpawner : MonoBehaviour
{
    private DateTime lastSpawn;
    private Dictionary<int, KeyCode> lineToCharDictionary = new Dictionary<int, KeyCode>(){{0, KeyCode.A}, {1, KeyCode.S}, {2, KeyCode.D}};
    public float timeInterval;
    public GameObject start;
    public List<GameObject> lineList;
    public GameObject cubePrefab;
    public int misses = 0;
    public int hits = 0;

    private void SpawnCube(int lineIndex) {
        GameObject cube = GameObject.Instantiate(cubePrefab);
        cube.transform.position = new Vector3(lineList[lineIndex].transform.position.x, start.transform.position.y, -1);
        Cube script = cube.GetComponent<Cube>();
        script.destructionKey = lineToCharDictionary[lineIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = DateTime.Now;
        for (int i = 0; i < lineList.Count; i++) {
            if (UnityEngine.Random.Range(0,3) == 0) {
                SpawnCube(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((DateTime.Now - lastSpawn).Seconds >= timeInterval) {
            lastSpawn = DateTime.Now;
            for (int i = 0; i < lineList.Count; i++) {
                if (UnityEngine.Random.Range(0,3) == 0) {
                    SpawnCube(i);
                }
            }
        }
    }
}
