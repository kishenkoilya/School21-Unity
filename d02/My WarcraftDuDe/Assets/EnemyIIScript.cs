using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIIScript : MonoBehaviour
{
    [SerializeField] private GameObject myHQ, enemyHQ;
    [SerializeField] private List<UnitScript> units;
    [SerializeField] private LayerMask clickableObjects;
    // Start is called before the first frame update
    void Start()
    {
        myHQ = GameObject.Find("OrcHQ");
    }

    void CollectUnits() {
        GameObject[] iiObjects = GameObject.FindGameObjectsWithTag("Enemy");
        units.Clear();
        foreach (GameObject obj in iiObjects) {
            if (obj.GetComponent<UnitScript>()) {
                units.Add(obj.GetComponent<UnitScript>());
            }
        }
    }

    void AttackHQ() {
        foreach (UnitScript unit in units) {
            if (unit.currentAttackTarget == null) unit.currentAttackTarget = enemyHQ;
        }
    }

    void CheckNearbyEnemies() {
        foreach (UnitScript unit in units) {
            if (!unit.currentAttackTarget.GetComponent<UnitScript>()) unit.FindTarget(1.5f, true);
        }
    }

    void AttackDestroyersOfHQ() {
        foreach (UnitScript enemy in myHQ.GetComponent<BuildingScript>().attackingEnemyUnits) {
            if (enemy && !enemy.dead) {
                foreach (UnitScript unit in units) {
                    unit.currentAttackTarget = enemy.gameObject;
                }
                break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CollectUnits();
        AttackHQ();
        CheckNearbyEnemies();
        AttackDestroyersOfHQ();
    }
}
