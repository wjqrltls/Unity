using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float speed = 70f;
    public GameObject explosion;
    public GameObject ImpactEffect;
    public int damage;
    public int durabilityDamage;

    public float explosionRadius = 0f;

	public void Seek (Transform _target, int _damage) {
		target = _target;
        damage = _damage;
	}


    void Update() {
        
        if (target == null) {

            Destroy(Instantiate(explosion, this.transform.position, this.transform.rotation), 1f);
        	Destroy(gameObject);
            
        	return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate (dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("enemy")){
            HitTarget();
            return;
        }
    }

    void HitTarget () {

        Destroy(gameObject);

        if(explosionRadius > 0f){
            Damage(target);
            Explode();
        }
        else{
            Damage(target);
        }
    }

    void Explode(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders){
            if(collider.tag == "enemy"){
                damage /= 3;
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy){
        enemy.GetComponent<Enemy>().LoseHp(damage);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

    }

}
