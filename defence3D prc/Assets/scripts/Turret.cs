using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
    
    private Transform target;

    [Header("Attributes")]

    public float range = 15f;
    public float reloadTime = 0.5f;
    private float fireCountdown = 0f;

    public int turret_damage;

    public int b_p_cnt = 1;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    private Transform firePoint;
    private int firePointIndex = 0;

    public Bullet bullet;

    int turretFix = 1;
    public int durability;
    int maxdurability;
    private int fixCountdown = 0;
    public int fixTime;
    private static int damage;

    public GameObject uiPrefab;
    private GameObject turret;
    private TurretUI turretUI;

    private float countdown = 0;

    void Start() {
        maxdurability = durability;
        firePoint = transform.GetChild(0);
        InvokeRepeating ("UpdateTarget", 0f, 0.5f);
        damage = bullet.durabilityDamage;
        turret = (GameObject)Instantiate(uiPrefab, firePoint.position, firePoint.rotation);
        turretUI = turret.GetComponent<TurretUI>();
        turretUI.Position(this.gameObject);
        turretUI.TurretUpgrade(turret_damage);
    }

    void UpdateTarget () {

    	GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
    	float shortestDistance = Mathf.Infinity;
    	GameObject nearestEnemy = null;
    	foreach (GameObject enemy in enemies) {
    		float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
    		if (distanceToEnemy < shortestDistance) {
    			shortestDistance = distanceToEnemy;
    			nearestEnemy = enemy;
    		}
    	}

    	if (shortestDistance <= range) {
    		target = nearestEnemy.transform;
    	}
   		else {
			target = null;
		} 

    }

    void Update() {

        TurretFix();
        countdown += Time.deltaTime;
    	
    	if (target == null) {
    		return;
    	}

    	Vector3 dir = target.position - transform.position;
    	Quaternion lookRotation = Quaternion.LookRotation(dir);
    	Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
    	partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);

    	if (fireCountdown <= 0) {
    		Shoot();
            TurretDurabilityDamage();
    	}
    	fireCountdown -= Time.deltaTime;
    }

    public void TurretUpgrade(int sumDamage){
        maxdurability += sumDamage * 2;
        turretFix += sumDamage;
        turret_damage += sumDamage;
        turretUI.TurretUpgrade(turret_damage);
    }

    public void TurretSell(){
        Destroy(turret);
        Destroy(this.gameObject);
    }

    void Shoot () {

    	GetNextfriePoints();
    	GameObject bulletGO = (GameObject)Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Seek(target, turret_damage);
    }

    void TurretFix(){
        if(fixCountdown == 1){
            fixCountdown = 0;
            countdown = 0;
            if(durability != maxdurability){

                durability += turretFix;
            }
        }
        fixCountdown += (int)countdown / fixTime;
    }

    public void TurretDurabilityDamage(){
        if(durability - damage > 0){

            durability -= damage;
        }
        else{
            Destroy(turret);
            Destroy(gameObject);
        }
    }

    void GetNextfriePoints() {

		if (firePointIndex < b_p_cnt - 1) {
			fireCountdown = reloadTime / 100f;
			firePoint = transform.GetChild(firePointIndex++);
		}
        else if (b_p_cnt == 1) {
            fireCountdown = reloadTime / 10f;
        }
    	else {
			fireCountdown = reloadTime / 10f;
			firePointIndex = 0;
			firePoint = transform.GetChild(0);
		}
	}

    void OnDrawGizmosSelected () {

    	Gizmos.color = Color.red;
    	Gizmos.DrawWireSphere(transform.position, range);

    }
}
