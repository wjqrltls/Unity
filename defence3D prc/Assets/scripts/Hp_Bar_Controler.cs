using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Bar_Controler : MonoBehaviour{
	
	public Enemy enemy;

	public Slider hp_Bar;
	public Text hp_Text;


	void Start(){
		hp_Bar.maxValue = enemy.hp;
	}

	void Update(){
		
		Bar();
		Text();
	}

	void Bar(){
		hp_Bar.value = enemy.hp;
	}

	void Text(){
		hp_Text.text = "" + enemy.hp;
	}
}