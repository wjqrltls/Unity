using UnityEngine;
using UnityEngine.EventSystems;

public class Turret_Position : MonoBehaviour {

	public Color hoverColor;
	public Vector3 positionOffset;

	[Header("Optional")]
	public GameObject turret;

	private Renderer rend;
	private Color startColor;

	Build_Manager build_Manager;

    void Start() {

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        build_Manager = Build_Manager.instance;
    }

    public Vector3 GetBuildPosition (){
    	return transform.position + positionOffset;
    }

    void OnMouseDown () {

    	if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}


    	if (!build_Manager.CanBuild) {
    		return;
    	}

    	if (turret != null) {
    		return;
    	}
    	build_Manager.BuildTurretOn(this);
    }

	void OnMouseEnter () {

		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		if (!build_Manager.CanBuild) {
    		return;
    	}

		rend.material.color = hoverColor;
	}

	void OnMouseExit () {

		rend.material.color = startColor;
	}

}
