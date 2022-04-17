using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public TurretBlueprint basic_Turret;
	public TurretBlueprint missile_Luncher;

	public GameObject select_BasicTurret;
	public GameObject select_MissileLuncher;
	public Text basicTurret_Cost;
	public Text missileLuncher_Cost;
	public MoneyCounter moneycounter;

	Build_Manager build_Manager;

	void Start () {

		basicTurret_Cost.text = " :  " + basic_Turret.cost;
		missileLuncher_Cost.text = " :  " + missile_Luncher.cost;

		build_Manager = Build_Manager.instance;
		select_BasicTurret.SetActive(false);
		select_MissileLuncher.SetActive(false);
	}

	public void PurchaseBasicTurret () {

		build_Manager.SelectTurretToBuild(basic_Turret);

		select_BasicTurret.SetActive(true);
		select_MissileLuncher.SetActive(false);
	}

	public void PurchaseMissileLuncher () {

		build_Manager.SelectTurretToBuild(missile_Luncher);

		select_MissileLuncher.SetActive(true);
		select_BasicTurret.SetActive(false);
	}

}
