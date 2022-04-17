using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select_Turret : MonoBehaviour{
	public Color hoverColor;
	public Vector3 positionOffset;

	public Upgrade upgrade;
	public GameObject turret;

	[Header("Optional")]

	private Renderer rend;
	private Color startColor;

	public int ifSelect = 0;


    void Start() {
    	upgrade = GameObject.Find("EventSystem").GetComponent<Upgrade>();
    	turret = this.gameObject;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        
    }

    void OnMouseDown () {
    	if(ifSelect == 0){
           	upgrade.Select(turret);
    		ifSelect = 1;		
    	}

    }

	void OnMouseEnter () {
		if(ifSelect == 0){
			rend.material.color = hoverColor;
		}
	}

	public void OnMouseExit () {
		if(ifSelect == 0){

			rend.material.color = startColor;
		}
	}

}
