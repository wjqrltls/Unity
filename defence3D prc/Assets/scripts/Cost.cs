using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cost : MonoBehaviour{
	public Text cost_text;
	public int cost;

	void Start(){
		cost_text.text = " :  " + cost;
	}

}
