using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour{
	public static int life = 10;
	public Text life_text;

	void Update(){
		Life_Text();
		if (life <= 0){
			SceneManager.LoadScene("GameOver");
		}
	}

	void Life_Text(){
		life_text.text = " LIFE   :  " + life;
	}
}
