using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using EZCameraShake;

using Photon.Pun;
using Photon.Realtime;

public class VR_PlayerController_Main : MonoBehaviour
{
	public PlayerBlueprint playerBlueprint;

	public GunBlueprint gunBlueprint;

	Vector3 cameraLocalPos;
	GameObject playerCamera;
	float originalCameraMagnification;

	public CharacterController player;

	public Transform shoulderPos;
	public Transform weaponAnimPos;
	public Transform rbPos;
	public Transform reboundPos;

	//public OVR카메라리그 어쩌고

	public Rig zoomRig;
	public Rig reboundRig;
	public MultiRotationConstraint headRot;
	public MultiPositionConstraint aimPos;

	float h;
	float v;

	public float speed = 0.0f;
	public float gravity = -9.81f;

	[HideInInspector] // 11.07 추가
	public GameObject aimPoint; // 11.06 수정

	public Transform groundCheck;
	public float groundDistance = 0.1f;
	public LayerMask groundMask;

	Vector3 velocity;

	private float cur_hp;// 현재 자신의 체력
	private float max_hp;// 현재 자신의 체력

	bool isJumping;

	int isMoving;

	[HideInInspector] // 11.07 추가
	public Animator animator;

	private GameObject bullet;

	public GameObject firePoint;

	float isClick;
	bool isFireRate = false;
	bool isReload = false;
	bool isdeath = true;
	bool isheal = true;
	bool issound;

	float timer = 0.0f;

	int magazine;

	public ParticleSystem bulletMuzzle;

	public GameObject effectObj;

	public OVRPlayerController OVRpcontroller;

	//int playernumber;

	/// <summary>
	public PhotonView PV;
	/// </summary>

	void Awake()
	{
		playerCamera = this.transform.Find("CenterEyeAnchor").gameObject;
		cameraLocalPos = playerCamera.transform.localPosition;
		originalCameraMagnification = playerCamera.GetComponent<Camera>().fieldOfView;
		OVRManager.profile.eyeHeight = 2.35f;//얘 늘려도 그대로지?
		//trackingSpace.localPosition = new Vector3(0f, 1.35f, 0.4f);
		//aim.localPosition = new Vector3(0.0f, 0.0f, gunBlueprint.fireRange); // 에임 위치 설정
	}

	void Start()
	{
		Setup();

	}

	//void FixedUpdate()
	//{
	//	x = Input.GetAxis("Horizontal");
	//	z = Input.GetAxis("Vertical");
	//	jump = Input.GetButtonDown("Jump");
	//	ifClick = Input.GetButton("Fire1");
	//}

	void Update()
	{
		//if (isdeath && GameManager.instance.isPmove)
		{
			// 플레이어 입력
			//h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");

			/*
			// 플레이어 이동 방향 설정 1-1 (상대좌표로 변경)
			Vector3 dir = new Vector3(h, 0, v);
			dir = dir.normalized;

			// 카메라를 기준으로 방향 설정 1-2
			dir = Camera.main.transform.TransformDirection(dir);
			*/

			// 점프가 끝났는지 확인
			if (isJumping && player.collisionFlags == CollisionFlags.Below)
			{
				// 점프 전 상태로 초기화한다.
				isJumping = false;
				// 캐릭터 수직 속도를 0으로 만든다.
				velocity.y = 0;
			}
			// 점프를 하고 있지 않다면 스페이스 바를 눌렀을 시 점프
			if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch) != 0 && !isJumping)
			{
				// 캐릭터 수직 속도에 점프력을 적용하고 점프 상태로 변경한다.

				velocity.y = playerBlueprint.jumpHeight;
				isJumping = true;
			}

			// 캐릭터 수직 속도에 중력 값을 적용한다.
			//velocity.y += gravity * Time.deltaTime;
			//dir.y = velocity.y;




			//if(PV.IsMine)
			//{
			//Debug.Log("PV!");
			// 플레이어 움직임 제어
			Vector3 move = transform.right * h + transform.forward * v;
			//Vector3 move =  h;
			player.Move(move * speed * Time.deltaTime);

			// 캐릭터의 이동 애니메이션 제어
			velocity.y += gravity * Time.deltaTime;
			player.Move(velocity * Time.deltaTime);
			//}


			// 움직이고 있는지 확인
			if (h != 0 || v != 0)
			{
				isMoving = 1;
			}
			else
			{
				isMoving = 0;
			}
			//speed = Mathf.Lerp(0.0f, playerBlueprint.speed, isMoving * 0.3f);
			animator.SetFloat("Speed_f", speed);


			if (Input.GetButton("Fire2") && playerBlueprint.zoomCameraMagnification != 0)
			{
				//playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, playerBlueprint.cameraZoomPos.position, .2f);
				playerCamera.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(playerBlueprint.zoomCameraMagnification, playerCamera.GetComponent<Camera>().fieldOfView, .2f);
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
				SetSpeed(0);
				aimPoint.SetActive(false);
			}

			PlayAnim(reboundRig, 0f, .1f);

			//isClick = Input.GetButton("Fire1");
			isClick = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
			// 캐릭터 공격
			if (isClick != 0 && !isFireRate && !isReload)
			{
				reboundRig.weight = 1f;
				zoomRig.weight = 0f;
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
			else
			{
				reboundPos.position = rbPos.position;
				reboundPos.rotation = rbPos.rotation;
			}
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
		}
		if (cur_hp <= 0 && isdeath)
		{
			isdeath = false;
			animator.SetBool("Death_b", true);

			//죽을 때 나는 소리
			SoundManger.instance.PlayerDie();
			Invoke("soundtrue", 2f);
		}
	}
	void SetSpeed(float increase)
	{
		speed = Mathf.Lerp(0.0f, playerBlueprint.speed + increase, isMoving * 0.3f);//11월6일 수정
	}

	void soundtrue()
	{
		issound = false;
	}

	void PlayAnim(Rig rig, float weight, float speed)
	{
		rig.weight = Mathf.SmoothStep(rig.weight, weight, speed);
	}



	void Setup()
	{
		PV = this.transform.GetComponent<PhotonView>();
		aimPoint = GameObject.Find("AimPoint");
		rbPos.localPosition = new Vector3(rbPos.localPosition.x, rbPos.localPosition.y, rbPos.localPosition.z - (gunBlueprint.rebound / 10));
		animator = GetComponentInChildren<Animator>();
		animator.SetInteger("WeaponType_int", gunBlueprint.gunType);
		magazine = gunBlueprint.magazine;
		cur_hp = playerBlueprint.hp;
		max_hp = playerBlueprint.hp;
		Debug.Log("[PlayreController]OntriggerEnter/cur_hp : " + cur_hp);
		Debug.Log("[PlayreController]OntriggerEnter/max_hp : " + playerBlueprint.hp);
		//아래부터 희진 코드
		animator = GetComponentInChildren<Animator>();
		animator.SetInteger("WeaponType_int", gunBlueprint.gunType);
		magazine = gunBlueprint.magazine;
		Debug.Log("[PlayreController]Setup/ playerType" + playerBlueprint.characterName);


		//cur_hp = playerHpManager.player1_;
		Debug.Log("[PlayreController]OntriggerEnter/cur_hp : " + cur_hp);
		//max_hp = p_status.defalt_Health;
		Debug.Log("[PlayreController]OntriggerEnter/max_hp : " + playerBlueprint.hp);
	}

	public void HitByExplosion(Vector3 explosionPos, int damage) // 폭발형
	{
		cur_hp -= damage;
		Debug.Log("Explosion_Enemy_atk : " + cur_hp);
	}

	void OnTriggerEnter(Collider other)
	{


		if (other.tag == "Map2")
		{
			Debug.Log("[PlayreController]" + playerBlueprint.characterName + "_OntriggerEnter/nextMap : " + GameManager.instance.nextMap);
			GameManager.instance.nextMap = true;
			GameManager.instance.flare.SetActive(false);
		}
		if (cur_hp < 0)
		{
			if (other.tag == "Player")
			{
				Debug.Log("[PlayreController]" + playerBlueprint.characterName + "_OntriggerEnter/Resurrection : ");
				Resurrection();
			}
		}
		if (other.tag == "Heal" && isheal == true)
		{
			Debug.Log("[PlayreController]" + playerBlueprint.characterName + "_OntriggerEnter/Healobj : ");
			isheal = false;
			StartCoroutine(Heal());

		}
	}

	public void PlayerAttack(int damage, string name) // 적에게 공격 받았을 때
	{
		cur_hp -= damage;
		SoundManger.instance.PlayerHurt();
		Debug.Log("[PCM]PlayerAttack / enemy.name : " + name);
	}

	void Resurrection()
	{
		cur_hp = 30;
		Debug.Log("[PlayreController]" + playerBlueprint.characterName + "Resurrection : " + cur_hp);
		isdeath = true;

		//animator.SetInteger("DeathType_int", 0);
		animator.SetBool("Death_b", false);
		gameObject.SetActive(false);
		gameObject.SetActive(true);
		GameObject healobj = Instantiate(effectObj, transform.position, effectObj.transform.rotation);
		Destroy(healobj, 1f);

	}

	IEnumerator Heal()
	{
		yield return new WaitForSeconds(1f);
		if (playerBlueprint.hp < cur_hp)
		{
			cur_hp = playerBlueprint.hp;
			Debug.Log("[PlayreController]" + playerBlueprint.characterName + "_OntriggerEnter/Heal : " + cur_hp);
		}
		else
		{
			cur_hp += 30;
			Debug.Log("[PlayreController]" + playerBlueprint.characterName + "_OntriggerEnter/Heal : " + cur_hp);
		}
		isheal = true;
		GameObject healobj = Instantiate(effectObj, transform.position, effectObj.transform.rotation);
		Destroy(healobj, 1f);
	}


}
