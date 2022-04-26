using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBlueprint
{
	public string characterName;
	public int characterCode; //캐릭터의 스킬을 불러올 때 사용
	public float speed;
	public float jumpHeight;
	public int hp;
	public Transform cameraZoomPos;
	public float zoomCameraMagnification;
}
