using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using EZCameraShake;

using Photon.Pun;
using Photon.Realtime;

public class PlayerController_Main : MonoBehaviourPun, IPunObservable
{
	#region Fields
	////Player_Status p_status;
	Enemy_Status1 e_status;

	public PlayerBlueprint playerBlueprint;

	public GunBlueprint gunBlueprint;

	Vector3 cameraLocalPos;
	GameObject playerCamera;
	float originalCameraMagnification;

	public CharacterController player;

	public Transform shoulderPos;
	public Transform rbPos;
	public Transform reboundPos;

	public Rig zoomRig;
	public Rig reboundRig;
	public Rig weaponDeathRig;
	public MultiRotationConstraint headRot;
	public MultiPositionConstraint aimPos;

	float h;
	float v;
	bool jump;

	float speed = 0.0f;
	public float gravity = -9.81f;

	[HideInInspector] // 11.07 추가
	public GameObject aimPoint; // 11.06 수정

	public Transform groundCheck;
	public float groundDistance = 0.1f;
	public LayerMask groundMask;

	Vector3 velocity;

	public float cur_hp;// 현재 자신의 체력
	private float max_hp;// 현재 자신의 체력

	bool isJumping = true;

	int isMoving;

	[HideInInspector] // 11.07 추가
	public Animator animator;

	private GameObject bullet;

	public GameObject firePoint;
	public Transform aim;

	bool isClick = false;
	bool isFireRate = false;
	bool isReload = false;
	bool isdeath = true;
	bool isheal = true;
	bool issound;

	float timer = 0.0f;

	int magazine;

	public ParticleSystem bulletMuzzle;

	public GameObject effectObj;

	int playernumber;

	/// <summary>
	public PhotonView PV;
	private Rigidbody rigidbody;
	/// </summary>
	#endregion

	#region MonoBehaviourCallBacks
	void Awake()
	{
		//PV = this.GetComponent<PhotonView>();

		playerCamera = this.transform.GetChild(1).gameObject;
		cameraLocalPos = playerCamera.transform.localPosition;
		originalCameraMagnification = playerCamera.GetComponent<Camera>().fieldOfView; //조준 되기 전의 fieldOfView값
																					   //aim.localPosition = new Vector3(0.0f, 0.0f, gunBlueprint.fireRange); // 에임 위치 설정
	}

	void Start()
	{
		Setup();
		if(PV.IsMine){
			Debug.Log("캐릭터 코드 : " + playerBlueprint.characterCode);
			MainUIManager.instance.setHPImage(playerBlueprint.characterCode);
		}
	}

	void Update()
	{
		if (isdeath && GameManager.instance.isPmove)
		{
			reboundPos.position = rbPos.position;
			reboundPos.rotation = rbPos.rotation;

			#region 주석
			/* // 플레이어 이동 방향 설정 1-1 (상대좌표로 변경)
			Vector3 dir = new Vector3(h, 0, v);
			dir = dir.normalized;

			// 카메라를 기준으로 방향 설정 1-2
			dir = Camera.main.transform.TransformDirection(dir); */

			// 캐릭터 수직 속도에 중력 값을 적용한다.
			//velocity.y += gravity * Time.deltaTime;
			//dir.y = velocity.y;
			#endregion

			//네트워크 동기화 관련 부문
			if (PV.IsMine)
			{
				MainUIManager.instance.myhp_bar.fillAmount = cur_hp / max_hp;
				MainUIManager.instance.bullet_bar.fillAmount = magazine / (float)gunBlueprint.magazine;
				#region (구)움직임 관련
				// 플레이어 입력
				h = Input.GetAxis("Horizontal");
				v = Input.GetAxis("Vertical");

				// 플레이어 움직임 제어
				Vector3 move = transform.right * h + transform.forward * v;
				player.Move(move.normalized * speed * Time.deltaTime);


				player.Move(velocity * Time.deltaTime);
				#endregion

				#region 점프 처리
				// 점프를 하고 있지 않다면 스페이스 바를 눌렀을 시 점프
				if (Input.GetButtonDown("Jump") && !isJumping)
				{
					// 캐릭터 수직 속도에 점프력을 적용하고 점프 상태로 변경한다.
					velocity.y = playerBlueprint.jumpHeight;
					isJumping = true;
				}
				else if (player.collisionFlags == CollisionFlags.Below) // 점프가 끝났는지 확인
				{
					// 점프 전 상태로 초기화한다.
					isJumping = false;
					// 캐릭터 수직 속도를 0으로 만든다.
					velocity.y = 0;
				}
				else
				{
					velocity.y += gravity * Time.deltaTime;
				}
				#endregion

				#region 줌 처리
				if (Input.GetButton("Fire2") && playerBlueprint.zoomCameraMagnification != 0)
				{
					//playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, playerBlueprint.cameraZoomPos.position, .2f);
					playerCamera.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(playerCamera.GetComponent<Camera>().fieldOfView, originalCameraMagnification - playerBlueprint.zoomCameraMagnification, .2f);
					PlayAnim(zoomRig, 1f, .2f);
					headRot.weight = Mathf.SmoothStep(headRot.weight, 1f, .2f);
					aimPos.weight = Mathf.SmoothStep(aimPos.weight, 0f, .2f);

					SetSpeed(-15); //11월7일 SetSpeed()메서드 추가
					aimPoint.SetActive(true);
				}
				else
				{
					//playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, cameraLocalPos, .2f);
					playerCamera.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(playerCamera.GetComponent<Camera>().fieldOfView, originalCameraMagnification, .2f);
					PlayAnim(zoomRig, 0f, .2f);
					headRot.weight = Mathf.SmoothStep(headRot.weight, 0f, .2f);
					aimPos.weight = Mathf.SmoothStep(aimPos.weight, 1f, .2f);
					//speed = Mathf.Lerp(0.0f, playerBlueprint.speed, isMoving * 0.3f); //11월6일 수정
					SetSpeed(0);
					aimPoint.SetActive(false);
				}
				#endregion

				#region 발사 입력
				isClick = Input.GetButton("Fire1");
				#endregion
			}

			// 움직이고 있는지 확인
			if (h != 0 || v != 0)
			{
				isMoving = 1;
			}
			else
			{
				isMoving = 0;
			}

			animator.SetFloat("Speed_f", speed);

			//PV.RPC("PlayAnim", RpcTarget.All, reboundRig, 0f, .1f);
			PlayAnim(reboundRig, 0f, .1f);

			if ((magazine == 0 || (Input.GetKeyDown(KeyCode.R) && magazine != gunBlueprint.magazine)) && !isReload)  // 자동 재장전 or 수동 재장전
			{
				isReload = true;
				animator.SetBool("Reload_b", isReload);
				timer = gunBlueprint.reloadTime;
				SoundManger.instance.GunReloding();

			}

			if (timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else if (isFireRate)
			{
				isFireRate = false;
			}
			else if (isReload)
			{
				isReload = false;
				animator.SetBool("Reload_b", isReload);
				magazine = gunBlueprint.magazine;
			}

			#region !PV.IsMine
			if (!PV.IsMine)
			{
				MainUIManager.instance.SetHP(playerBlueprint.characterCode, cur_hp, max_hp);
				//씬 내에서 3개의 오브젝트를 찾고, 하나하나 각 클라이언트의 주인 인스턴스인지를 pv를 이용해서 체크 후,
				//찾아낸다음 그한테 체력을 표시하기 위해 자신의 이름과 hp 값을 넘겨준다. (아마 될 것 같음)
			}
			#endregion
		}

		if (cur_hp <= 0 && isdeath)
		{
			isdeath = false;
			animator.SetBool("Death_b", true);
			weaponDeathRig.weight = 1f;

			//죽을 때 나는 소리
			SoundManger.instance.PlayerDie();
			Invoke("soundtrue", 2f);
		}

		if (isClick && !isFireRate && !isReload)
		{
			PV.RPC("PlayerAttack", RpcTarget.All);
		}
	}
	#endregion

	void Setup()
	{
		MainUIManager.instance.	SetPlayer(gameObject);
		//PV = this.transform.GetComponent<PhotonView>();
		aimPoint = GameObject.Find("AimPoint");
		rbPos.localPosition = new Vector3(rbPos.localPosition.x, rbPos.localPosition.y, rbPos.localPosition.z - (gunBlueprint.rebound / 10));
		animator = GetComponentInChildren<Animator>();
		animator.SetInteger("WeaponType_int", gunBlueprint.gunType);
		magazine = gunBlueprint.magazine;
		////p_status = FindObjectOfType<Player_Status>();
		//e_status = FindObjectOfType<Enemy_Status1>();
		cur_hp = playerBlueprint.hp;
		max_hp = playerBlueprint.hp;
		//아래부터 희진 코드
		animator = GetComponentInChildren<Animator>();
		animator.SetInteger("WeaponType_int", gunBlueprint.gunType);
		magazine = gunBlueprint.magazine;
	}

	void SetSpeed(float increase)
	{
		speed = Mathf.Lerp(0.0f, playerBlueprint.speed + increase, isMoving * 0.3f);//11월6일 수정
	}

	void soundtrue()
	{
		issound = false;
	}

	//[PunRPC]
	void PlayAnim(Rig rig, float weight, float speed)
	{
		rig.weight = Mathf.SmoothStep(rig.weight, weight, speed);
	}

	#region CollisionCallBacks

	[PunRPC]
	public void HitByExplosion(Vector3 explosionPos, int damage) // 폭발형
	{
		cur_hp -= damage;
	}

	void OnTriggerEnter(Collider other)
	{
		if (cur_hp <= 0) // 자신이 기절일 때 다른 플레이어와 만난다면 살아나기
		{
			Debug.Log("HP = " + (cur_hp <= 0));
			if (other.gameObject.CompareTag("Player"))
			{
				Debug.Log("플레이어 접촉됨");
				Resurrection(); // 살아나기
			}
		}

		if (other.tag == "Map2") // 다음 맵 도착 시 알려주는 것
		{
			GameManager.instance.nextMap = true;
			GameManager.instance.isnext = false;
			GameManager.instance.flare.SetActive(false);
		}


		if (other.tag == "Heal" && isheal == true) // 체력 아이템을 먹은 경우 체력 회복
		{
			isheal = false;
			StartCoroutine(Heal());

		}
	}

	#endregion

	public void PlayerAttacked(int damage, string name) // 적에게 공격 받았을 때
	{
		cur_hp -= damage;
		SoundManger.instance.PlayerHurt();
		Debug.Log("[PCM]PlayerAttack / enemy.name : " + name);
	}

	void Resurrection() // 기절 회복
	{
		Debug.Log("회복 중");
		cur_hp = 30;
		isdeath = true;

		//animator.SetInteger("DeathType_int", 0);
		animator.SetBool("Death_b", false);
		gameObject.SetActive(false);
		gameObject.SetActive(true);
		GameObject healobj = Instantiate(effectObj, transform.position, effectObj.transform.rotation);
		Destroy(healobj, 1f);
		weaponDeathRig.weight = 0f;

	}

	IEnumerator Heal() // 체력 회복
	{
		yield return new WaitForSeconds(1f);
		if (max_hp < cur_hp)
		{
			cur_hp = max_hp;
		}
		else
		{
			cur_hp += 30;
		}
		isheal = true;
		GameObject healobj = Instantiate(effectObj, transform.position, effectObj.transform.rotation);
		Destroy(healobj, 1f);
	}

	[PunRPC] //인스턴스의 동작 동기화를 위해 사용 할 것임.
	public void PlayerAttack()
	{
		reboundRig.weight = 1f;
		//zoomRig.weight = 0f;
		for (int i = 0; i < gunBlueprint.fireCount; i++)
		{
			bullet = Instantiate(gunBlueprint.bullet, firePoint.transform.position, firePoint.transform.rotation * Quaternion.Euler(Random.Range(-gunBlueprint.gunSpread, gunBlueprint.gunSpread), Random.Range(-gunBlueprint.gunSpread, gunBlueprint.gunSpread), 0f));
			bullet.name = "" + gunBlueprint.damage;
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * gunBlueprint.bulletSpeed);
			Destroy(bullet, 2.0f);
		}
		SoundManger.instance.ShootUp();
		//CameraShaker.Instance.ShakeOnce(1f, 1f, .1f, 1f);

		bulletMuzzle.Play();
		magazine--;
		isFireRate = true;
		timer = gunBlueprint.fireRate;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(reboundRig.weight);
			stream.SendNext(cur_hp);
		}
		else if (stream.IsReading)
		{
			reboundRig.weight = (float)stream.ReceiveNext();
			cur_hp = (float)stream.ReceiveNext();
		}

	}
}