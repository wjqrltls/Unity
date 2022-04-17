using UnityEngine;

public class Build_Manager : MonoBehaviour {

	public static Build_Manager instance;

	void Awake () {

		if (instance != null) {
			return;
		}
		instance = this;
	}

	private TurretBlueprint turretToBuild;

	public bool CanBuild{
		get {
			return turretToBuild != null;
		}
	}

	public void BuildTurretOn (Turret_Position turretPosition){
		if (MoneyCounter.Money < turretToBuild.cost){
			return;
		}

		MoneyCounter.Money -= turretToBuild.cost;

		GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, turretPosition.GetBuildPosition(), Quaternion.identity);
		turret.GetComponent<Turret>().durability = turretToBuild.durability;
		turretPosition.turret = turret;


	}

	public void SelectTurretToBuild (TurretBlueprint turret) {
		turretToBuild = turret;
		
	}
}
