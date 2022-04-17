using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

	public GameObject upgrade_Sell;
	public GameObject shop;

	GameObject select_turret;

	public MoneyCounter moneycounter;

	public int cost = 500;
	public Text costText;

	public void Start(){
		upgrade_Sell.SetActive(false);
	}

	void Update(){

		costText.text = "Cost : " + cost;
	}

	public void Select(GameObject turret){
		select_turret = turret;
		shop.SetActive(false);
		upgrade_Sell.SetActive(true);
	}

	public void Turret_Upgrade(){
		if(MoneyCounter.Money - cost >= 0){

			select_turret.GetComponent<Turret>().TurretUpgrade(1);
			moneycounter.Cost(cost);
		}
	}

	public void Turret_Sell(){
		if(select_turret.CompareTag("basic_turret")){
			moneycounter.Coin(125);	
		}

		else if(select_turret.CompareTag("missile_luncher")){
			moneycounter.Coin(375);
		}
		select_turret.GetComponent<Turret>().TurretSell();

		Close();
	}

	public void Close(){
		shop.SetActive(true);
		upgrade_Sell.SetActive(false);
		select_turret.GetComponent<Select_Turret>().ifSelect = 0;
		select_turret.GetComponent<Select_Turret>().OnMouseExit();
	}

}