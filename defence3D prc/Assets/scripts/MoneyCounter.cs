using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour{

	public Text haveMoney;
    
    public static int Money = 1000;

    void Awake(){
    	Money = 1000;
    }

    void Update(){
    	UpdateText();
    }

	public void Coin(int coin){
		Money += coin;
	}

	public void Cost(int coin){
		Money -= coin;
	}

	void UpdateText(){
		haveMoney.text = "  :  " + Money;
	}
}
