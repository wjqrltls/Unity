using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour{

	public Text damageText;
	private int damage;

	public Text durabilityText;
	public Slider durabilityBar;


	private GameObject turret;

	void Update(){
		Bar();
		Text();
		if(turret == null){
			TurretSell();
		}
	}

    public void Position(GameObject turret_){

        turret = turret_;
        durabilityBar.maxValue = turret.GetComponent<Turret>().durability;

    }

	public void TurretUpgrade(int damage){
		damageText.text = "Turret Damage : " + damage;
	}
	
	public void TurretSell(){
		damageText.text = " ";
	}

	void Bar(){
		durabilityBar.value = turret.GetComponent<Turret>().durability;
	}

	void Text(){
		durabilityText.text = "" + turret.GetComponent<Turret>().durability;
	}
}
