using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;
public class EnemyController : MonoBehaviour, IPunObservable
{

    Animator Enemyanimator; // 애니메이터 
    public GameObject[] players = null; //타켓 추적용

    public Enemy_Status1 enemy_Status; // enemy status 적의 기본 데이터

    NavMeshAgent nav; // 추적AI
    Rigidbody rigid; // 리지디바디 받아오기

    public Transform target; // 플레이어 추적
    public Transform point; // 포인트 추적 
    public GameObject healingobj; // 죽었을 때 떨어트리는 체력아이템
    public GameObject bloodobj; // 맞는 이펙트


    public GameObject effectObj; //폭발 적 전용 이펙트

    bool Move; // 움직일 지 말지
    bool isdelay; // 적이 죽고나서의 딜레이

    public float health; //현재 적 체력

    public PhotonView PV;

    private GameObject explosion;

    void Awake()
    {
        Enemyanimator = GetComponent<Animator>(); // 적 애니메이터
        rigid = GetComponent<Rigidbody>(); // 적 리지티바디 : 적과 플레이어 가 만났을 때 밀리는 것을 방지하기 위한 리지디바디
        nav = GetComponent<NavMeshAgent>(); // AI 
    }

    void Start()
    {
        health = enemy_Status.hp; // 적 체력 받아오기
        Move = true;
        players = GameObject.FindGameObjectsWithTag("Player"); //  플레이어 추적 대상 찾아오기
        point = GameObject.FindWithTag("Defanse_Point").transform; // 디펜스 포인트 추적 대상 찾아오기
        isdelay = true;

    }


    void EnemyMove()
    {
        if ((target.position - transform.position).magnitude < (point.position - transform.position).magnitude) // 플레이어 타겟 추적
        {
            if ((target.position - transform.position).magnitude >= 3)
            {
                Enemyanimator.SetBool("Forward", true);
                nav.SetDestination(target.position); // 타켓 추적 이동
            }
            if ((target.position - transform.position).magnitude < 3)
            {
                Enemyanimator.SetBool("Forward", false);
            }
        }

        if ((target.position - transform.position).magnitude >= (point.position - transform.position).magnitude) //거점 포인트 추적
        {
            if ((point.position - transform.position).magnitude >= 3)
            {
                Enemyanimator.SetBool("Forward", true);
                nav.SetDestination(point.position); // 타켓 추적 이동
            }

            if ((point.position - transform.position).magnitude < 3)
            {
                Enemyanimator.SetBool("Forward", false);
            }
        }
    }

    void Target() // 타켓 추적
    {
        Transform near_p = null;

        foreach (GameObject p in players)
        {
            if (Vector3.Distance(transform.position, p.transform.position) <= 30f) // 자신과 거리가 30이하라면
            {
                if (!near_p || Vector3.Distance(p.transform.position, transform.position) < Vector3.Distance(near_p.position, transform.position)) // 현재 가장 가까이 있는 플레이어보다 다른 플레이어가 더 가깝다면
                {
                    near_p = p.transform; // 다른걸로 바꾸기

                }

            }
            else
            {
                near_p = point.transform; // 아니면 포인트로 바꾸기
            }

            target = near_p;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Target(); // 타켓 추적
        if (Move)
        {
            EnemyMove(); // 움직이기 
        }

        Debug.LogFormat(this.gameObject.name, "Target : ", target);
    }

    void FreezeVelocity() // 플레이어와 적이 서로 밀리지 않도록 하기
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void EnemyAttack()
    {
        Debug.Log("적이 공격했다!");
        if ((target.position - transform.position).magnitude <= 3) // 플레이어
        {
            Enemyanimator.SetInteger("Attack int", Random.Range(1, enemy_Status.aimationCount + 1)); // 랜덤으로 공격 모션 설정 및 공격
            if (enemy_Status.name == "Explosion") // 만약 폭발 몹이라면 코루틴 시작
            {
                StartCoroutine(Explosion());
            }
        }
        if ((point.position - transform.position).magnitude <= 3) // 거점
        {
            Enemyanimator.SetInteger("Attack int", Random.Range(1, enemy_Status.aimationCount + 1)); // 랜덤으로 공격 모션 설정 및 공격
            if (enemy_Status.name == "Explosion") // 만약 폭발 몹이라면 코루틴 시작
            {
                StartCoroutine(Explosion());
            }
        }
    }

    void freezeenemy()
    {
        Debug.Log("[DEC]freezeenemy / Freeze"); // 공격 시 움직임 멈춤
        Enemyanimator.SetInteger("Attack int", 0);
        Move = false;
    }
    void unfreezeenemy()
    {
        Debug.Log("[DEC]unfreezeenemy / UnFreeze"); // 공격이 끝날 시 움직임 시작
        Invoke("Move_Ture", 3f); // 3초뒤에 시작
    }

    void Move_Ture()
    {
        Move = true;
    }

    void Death()
    {
        Debug.Log("Death");
        Enemyanimator.Play("Die");
        transform.Find("HitBox").gameObject.SetActive(false);

        if (enemy_Status.name == "Explosion") // 만약 폭발 적이라면 코루틴 시작
        {
            StartCoroutine(Explosion());
        }
        else
        {
            Destroy(gameObject, 3f); // 아니면 그냥 파괴
        }
        nav.speed = 0; //죽으면 움직이는 속도 0


        int h = 1; // 랜던 값 부여
        //h = (int)Random.Range(0, 9);
        if (h == 1)
        {
            Instantiate(healingobj, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity); // 회복 아이템 확률에 따른 드랍
        }

        if (enemy_Status.name == "Middle") // 중간 보스 몹이 죽었을 댸 알려주는것
        {
            GameManager.instance.middleBossDeath++;
        }
        else if (enemy_Status.name == "Final") // 마지막 보스가 죽엇을 때 알려주는 것
        {
            GameManager.instance.finalBossDeath++;
        }
        else
        {
            GameManager.instance.enemy_Death++; // 일반몹이 죽었을 때 알려주는 것(폭발 몹 제외)
        }
        GameManager.instance.score += enemy_Status.score; // 접수 부여
        Debug.Log("[EC]" + "Round" + GameManager.instance.round + "AND Death / Death : " + GameManager.instance.enemy_Death); //라운드에 죽은 몹 수
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") // 플레이어 총알 맞았을 때
        {
            GameObject blood = Instantiate(bloodobj, other.transform.position, other.transform.rotation); // 피 튀기는 효과

            Destroy(blood, 3f); // 3초뒤 파괴     

            PhotonNetwork.Destroy(other.gameObject);
            health -= int.Parse(other.name); // 총알 데미지
            if (health <= 0 && isdelay == true)
            {
                Death();
                isdelay = false; // 딜레이

            }
        }
        if (other.tag == "DefensePoint") // 거점 공격
        {
            other.GetComponent<DefensePoint>().PointAttack(enemy_Status.damage);
        }
        if (other.tag == "Player") // 플레이어 공격
        {
            if (other.GetComponent<PlayerController_Main>() != null)
            {
                other.GetComponent<PlayerController_Main>().PlayerAttacked(enemy_Status.damage, enemy_Status.name);
            }//VR_Player 나중에 추가
        }
    }

    IEnumerator Explosion() // 폭발 적 전용 코루틴
    {
        yield return new WaitForSeconds(3f);

        PV.RPC("Explosion_Effect_Instantiate", RpcTarget.All);

        /*explosion = Instantiate(effectObj, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(5, 5, 5);*/

        RaycastHit[] rayHitPoint = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0f, LayerMask.GetMask("Point")); // 포인트가 근처에 있을 시 데미지 계산
        foreach (RaycastHit hitobj in rayHitPoint)
        {
            hitobj.transform.GetComponent<DefensePoint>().PV.RPC("HitByExplosion", RpcTarget.All, transform.position);
            //HitByExplosion(transform.position);
        }

        RaycastHit[] rayHitPlayer = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0f, LayerMask.GetMask("Player")); //플레이어가 근처에 있을 시 데미지 계산
        foreach (RaycastHit hitobj in rayHitPlayer)
        {
            hitobj.transform.GetComponent<PlayerController_Main>().PV.RPC("HitByExplosion", RpcTarget.All, transform.position, enemy_Status.damage);
            //HitByExplosion(transform.position, enemy_Status.damage);
        }

        PV.RPC("Explosion_Destroy", RpcTarget.All);
        GameManager.instance.enemy_Death++;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else if (stream.IsReading)
        {
            health = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Explosion_Effect_Instantiate()
    {
        explosion = Instantiate(effectObj, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(5, 5, 5);
    }

    [PunRPC]
    public void Explosion_Destroy()
    {
        Destroy(gameObject);
    }
}