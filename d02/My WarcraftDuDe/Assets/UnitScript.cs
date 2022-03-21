using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;
    public Vector3 currentDestination;
    [SerializeField] private List<AudioClip> unitResponces;
    [SerializeField] private Vector3 difference;
    [SerializeField] public List<Transform> frames;
    [SerializeField] public Transform hitPointsBar;
    [SerializeField] public Transform hitPointsBackground;
    [SerializeField] private float hitPointsMax, currentHitPoints, damage, attackDistance, attackTime;
    [SerializeField] private float currentAttackTime = 0;
    [SerializeField] private float deadTime = 0;
    [SerializeField] private bool attacking = false;
    [SerializeField] public GameObject currentAttackTarget;
    [SerializeField] private LayerMask clickableObjects;
    [SerializeField] public bool dead;
    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentDestination = transform.position;
    }



    // Update is called once per frame
    (Vector3, int, bool) MoveWhere(bool flip) {
        if (Mathf.Abs(currentDestination.x - transform.position.x) < 0.1 && Mathf.Abs(currentDestination.y - transform.position.y) < 0.1){
            return (Vector3.zero, -1, flip);
        }
        else {
            difference = currentDestination - transform.position;
            if (Mathf.Abs(difference.x) < 0.05) difference -= new Vector3 (difference.x,0,0);
            if (Mathf.Abs(difference.y) < 0.05) difference -= new Vector3 (0,difference.y,0);
            
            Vector3 movementVector = new Vector3();
            if (difference.x > 0) movementVector += new Vector3(1,0,0);
            if (difference.x < 0) movementVector += new Vector3(-1,0,0);
            if (difference.y > 0) movementVector += new Vector3(0,1,0);
            if (difference.y < 0) movementVector += new Vector3(0,-1,0);
            if (movementVector.x == 0 && movementVector.y == 1) return (movementVector, 0, false);
            else if (movementVector.x == 1 && movementVector.y == 1) return (movementVector, 1, false);
            else if (movementVector.x == 1 && movementVector.y == 0) return (movementVector, 2, false);
            else if (movementVector.x == 1 && movementVector.y == -1) return (movementVector, 3, false);
            else if (movementVector.x == 0 && movementVector.y == -1) return (movementVector, 4, false);
            else if (movementVector.x == -1 && movementVector.y == -1) return (movementVector, 3, true);
            else if (movementVector.x == -1 && movementVector.y == 0) return (movementVector, 2, true);
            else return (movementVector, 1, true);
        }
    }
    (int, bool) AttackWhere() {
        Vector3 distanceToEnemy = currentAttackTarget.transform.position - transform.position;
        if (Mathf.Abs(distanceToEnemy.x) <= attackDistance * 0.25f && distanceToEnemy.y > attackDistance * 0.75f) return (0, false);
        else if (distanceToEnemy.x > attackDistance * 0.75f && Mathf.Abs(distanceToEnemy.y) <= attackDistance * 0.25f) return (2, false);
        else if (distanceToEnemy.x < -attackDistance * 0.75f && Mathf.Abs(distanceToEnemy.y) <= attackDistance * 0.25f) return (2, true);
        else if (Mathf.Abs(distanceToEnemy.x) <= attackDistance * 0.25f && distanceToEnemy.y < -attackDistance * 0.75f) return (4, false);
        else if (distanceToEnemy.x > 0 && distanceToEnemy.y > 0) return (1, false);
        else if (distanceToEnemy.x > 0 && distanceToEnemy.y <= 0) return (3, false);
        else if (distanceToEnemy.x <= 0 && distanceToEnemy.y <= 0) return (3, true);
        else return (1, true);
    }
    public void SoundSelected() {
        if (Random.Range(0,2) == 1) {
            audioSource.clip = unitResponces[Random.Range(9,15)];
            audioSource.Play();
        }
    }

    public void SoundAcknowledged() {
        if (Random.Range(0,2) == 1) {
            audioSource.clip = unitResponces[Random.Range(0,4)];
            audioSource.Play();
        }
    }

    public void GetDamage(float dmg, UnitScript enemy = null) {
        currentHitPoints -= dmg;
        if (currentHitPoints < 0) currentHitPoints = 0;
        hitPointsBar.localScale = new Vector3(hitPointsBackground.localScale.x * (currentHitPoints / hitPointsMax), 
                                    hitPointsBackground.localScale.y, 
                                    hitPointsBackground.localScale.z);
        if (currentHitPoints <= 0 && deadTime == 0) {
            animator.SetBool("Dead", true);
            dead = true;
            currentAttackTarget = null;
            currentDestination = transform.position;
            AudioClip clip;
            if (unitResponces.Find(x => x.name == "death"))
                clip = unitResponces.Find(x => x.name == "death");
            else clip = unitResponces.Find(x => x.name == "dead");
            audioSource.clip = clip;
            audioSource.Play();
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    public float GetCurrentHP() {
        return currentHitPoints;
    }

    void Attacking() {
        if (currentAttackTarget != null) {
            currentDestination = currentAttackTarget.transform.position;
            if ((transform.position - currentAttackTarget.transform.position).magnitude <= attackDistance + currentAttackTarget.GetComponent<ItemScript>().itemSize) {
                currentDestination = transform.position;
                animator.SetInteger("Move_dir", -1);
                (int, bool) attackDirection = AttackWhere();
                if (animator.GetInteger("Attack_dir") == -1 && attackDirection.Item1 != -1) {
                    attacking = true;
                    currentAttackTime = 0;
                }
                animator.SetInteger("Attack_dir", attackDirection.Item1);
                spriteRenderer.flipX = attackDirection.Item2;
            }
            if (attacking) {
                currentAttackTime += Time.deltaTime;
                if (currentAttackTarget.GetComponent<UnitScript>() && currentAttackTarget.GetComponent<UnitScript>().GetCurrentHP() > 0) {
                    if (currentAttackTime >= attackTime) {
                        currentAttackTime = 0;
                        currentAttackTarget.GetComponent<UnitScript>().GetDamage(damage, gameObject.GetComponent<UnitScript>());
                        if (currentAttackTarget.GetComponent<UnitScript>().GetCurrentHP() <= 0) {
                            animator.SetInteger("Attack_dir", -1);
                            attacking = false;
                            currentAttackTarget = null;
                        }
                    }
                }
                else if (currentAttackTarget.GetComponent<BuildingScript>() && currentAttackTarget.GetComponent<BuildingScript>().GetCurrentHP() > 0) {
                    if (currentAttackTime >= attackTime) {
                        currentAttackTime = 0;
                        currentAttackTarget.GetComponent<BuildingScript>().GetDamage(damage, gameObject.GetComponent<UnitScript>());
                        if (currentAttackTarget.GetComponent<BuildingScript>().GetCurrentHP() <= 0) {
                            animator.SetInteger("Attack_dir", -1);
                            attacking = false;
                            currentAttackTarget = null;
                        }
                    }
                }
                else {
                    animator.SetInteger("Attack_dir", -1);
                    attacking = false;
                    currentAttackTarget = null;
                }
            }
        }
        else {
            animator.SetInteger("Attack_dir", -1);
            attacking = false;
        }
    }

    public void FindTarget(float offset = 1, bool findUnits = false) {
        Vector3 point_ld = new Vector3(transform.position.x - offset, transform.position.y - offset, -10);
        Vector3 point_ru = new Vector3(transform.position.x + offset, transform.position.y + offset, -10);
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(point_ld, point_ru, clickableObjects, -1, 1);
        if (collider2DArray.Length > 0) {
            GameObject enemy = null;
            float distanceToEnemy = 10;
            foreach (Collider2D collider in collider2DArray) {
                if (collider.gameObject.tag != gameObject.tag) {
                    if ((transform.position - collider.transform.position).magnitude < distanceToEnemy) {
                        enemy = collider.gameObject;
                        distanceToEnemy = (transform.position - collider.transform.position).magnitude;
                    }
                }
            }
            if (enemy != null)  {
                currentAttackTarget = enemy;
            }
        }
    }
    void Update() {
        if (dead == true) {
            deadTime += Time.deltaTime;
            if (deadTime >= 5) {
                Destroy(gameObject);
            }
        }
        else {
            (Vector3, int, bool) moveInfo = MoveWhere(spriteRenderer.flipX);
            transform.Translate(moveInfo.Item1 * Time.deltaTime);
            animator.SetInteger("Move_dir", moveInfo.Item2);
            spriteRenderer.flipX = moveInfo.Item3;

            Attacking();
        }
        if (animator.GetInteger("Move_dir") == -1 && animator.GetInteger("Attack_dir") == -1 && dead == false) {
            FindTarget();
        }
    }
}
