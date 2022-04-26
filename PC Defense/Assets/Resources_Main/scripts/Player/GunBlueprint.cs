using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunBlueprint
{
    public string gunName;
    public int gunType;
    public int magazine;
    public int damage;
    public float fireRate;
    public float fireRange;
    public float reloadTime;
    public GameObject bullet;
    public float rebound;
    public float bulletSpeed;
    public int fireCount; // 한번에 발사되는 총알 개수
    public float gunSpread; // 탄퍼짐
    /*
    장전시간은 플레이어 애니메이터의 WeaponType_int(guntype)에
    의해 설정되도록 설계되어 있음
    */
}
