using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{
	AudioSource audioPalyer;
	
	public AudioClip enemyDead_Sound;
	public AudioClip lifeLess_Sound;

	public static SoundManager instance;

	void Awake(){
		if(SoundManager.instance == null){
			SoundManager.instance = this;
		}
	}

	void Start(){
		audioPalyer = GetComponent<AudioSource>();
	}

	public void PlaySound_eD(){
		audioPalyer.PlayOneShot(enemyDead_Sound);
	}

	public void PlaySound_lL(){
		audioPalyer.PlayOneShot(lifeLess_Sound);
	}
}
