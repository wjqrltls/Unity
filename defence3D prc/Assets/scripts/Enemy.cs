using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public float speed = 10f;
	public int hp = 100;

	private Bullet bullet;
	public MoneyCounter moneyCounter;
	public int coin = 100;

	private GameObject ImpactEffect;
	private GameObject explosion;

	private Transform target;
	private int wavepointIndex = 0;

	public static int lessLife = 1;

	void Start (){
		target = Waypoints.points[0];
		moneyCounter = GameObject.Find("coinCounter").GetComponent<MoneyCounter>();
	} 

	void Update (){

		Vector3 dir = target.position - transform.position;
		transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
		
		if (Vector3.Distance(transform.position, target.position) <= 0.4f) {
			GetNextWaypoint();
		}
		
	}

	void GetNextWaypoint() {

		if (wavepointIndex >= Waypoints.points.Length - 1) {
			SoundManager.instance.PlaySound_lL();
			LifeManager.life -= lessLife;
			Destroy(this.gameObject);
			return;
		}
		wavepointIndex++;
		target = Waypoints.points[wavepointIndex];
	}


	private void OnTriggerEnter(Collider other){
		bullet = other.gameObject.GetComponent<Bullet>();
		if (other.gameObject.CompareTag("missile_bullet")){

			GameObject explosionIns = (GameObject)Instantiate(bullet.explosion, transform.position, transform.rotation);
			Destroy(explosionIns, 1f);
		}

		LoseHp(bullet.damage);
	}

	public void LoseHp(int damage){
		if (hp - damage <= 0){
			SoundManager.instance.PlaySound_eD();
			Destroy(this.gameObject);
			GameObject effectIns = (GameObject)Instantiate(bullet.ImpactEffect, transform.position, transform.rotation);
			Destroy(effectIns, 2f);
			moneyCounter.Coin(coin);
		}
		else{
			hp -= damage;
			Destroy(bullet);
		}
	}

	public void MaxHp(int difficulty){
		hp = difficulty;
	}
}