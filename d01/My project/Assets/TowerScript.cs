using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 shootingDirection;
    public float shootingInterval;
    [SerializeField] private float timeSinceLastShot = 0;

    float CalculateAngle(float x, float y) {
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + 90;
        return angle;
    }
    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab,
                                        transform.position + new Vector3(0, 2.25f, 0), 
                                        Quaternion.AngleAxis(CalculateAngle(shootingDirection.x, shootingDirection.y), Vector3.forward), 
                                        transform);
        bullet.GetComponent<BulletScript>().shootingDirection = shootingDirection;
        bullet.tag = gameObject.tag;
        bullet.GetComponent<SpriteRenderer>().color = gameObject.GetComponentInChildren<SpriteRenderer>().color;
    }
    // Start is called before the first frame update
    void Start()
    {
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootingInterval) {
            Shoot();
            timeSinceLastShot = 0;
        }
    }
}
