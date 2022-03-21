using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{    
    [SerializeField] private float hitPointsMax, currentHitPoints, deadTime;
    [SerializeField] private Transform hitPointsBar;
    [SerializeField] private Transform hitPointsBackground;
    [SerializeField] private bool spawnsUnits;
    [SerializeField] private bool dead = false;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float lastSpawnTime = 0;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private float timeSinceLastAttack = 500;
    [SerializeField] public List<UnitScript> attackingEnemyUnits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetDamage(float dmg, UnitScript enemy) {
        if (attackingEnemyUnits.Find(x => x == enemy) == null) attackingEnemyUnits.Add(enemy);
        if (timeSinceLastAttack > 10) gameObject.GetComponent<AudioSource>().Play();
        timeSinceLastAttack = 0;
        currentHitPoints -= dmg;
        if (currentHitPoints < 0) currentHitPoints = 0;
        hitPointsBar.localScale = new Vector3(hitPointsBackground.localScale.x * (currentHitPoints / hitPointsMax), 
                                    hitPointsBackground.localScale.y, 
                                    hitPointsBackground.localScale.z);
        if (currentHitPoints <= 0 && deadTime == 0) {
            gameObject.layer = LayerMask.NameToLayer("Default");
            dead = true;
            if (gameObject.name == "OrcHQ") {
                Camera.main.GetComponent<ControllerScript>().YouWin();
            }
            else if (gameObject.name == "HumanHQ") {
                Camera.main.GetComponent<ControllerScript>().GameOver();
            }
            else {
                GameObject HQ = GameObject.Find("OrcHQ");
                HQ.GetComponent<BuildingScript>().SpawnIntervalChange(2.5f);
            }
        }
    }
    public float GetCurrentHP() {
        return currentHitPoints;
    }

    public void SpawnIntervalChange(float delta) {
        spawnInterval += delta;
    }

    void FlushAttackingEnemyUnits() {
        for (int i = 0; i < attackingEnemyUnits.Count; i++) {
            if (!attackingEnemyUnits[i]) attackingEnemyUnits.Remove(attackingEnemyUnits[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        FlushAttackingEnemyUnits();
        if (spawnsUnits && !dead) {
            lastSpawnTime += Time.deltaTime;
            if (lastSpawnTime >= spawnInterval) {
                GameObject unit = Instantiate(unitPrefab, new Vector3(transform.position.x - 1.2f, transform.position.y - 1.5f, 0), Quaternion.AngleAxis(0,Vector3.forward));
                lastSpawnTime = 0;
            }
        }
        if (dead) {
            deadTime += Time.deltaTime;
            if (deadTime >= 5) {
                Destroy(gameObject);
            }
        }
        timeSinceLastAttack += Time.deltaTime;
    }
}
