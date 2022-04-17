using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Esc : MonoBehaviour {
	public GameObject esc;

	void Start(){

		esc.SetActive(false);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Time.timeScale = 0;
			esc.SetActive(true);			
		}
	}

	public void Main(){
		SceneManager.LoadScene("Start");
	}

	public void Continue(){
		esc.SetActive(false);
		Time.timeScale = 1;
	}

	public void EndGame(){
		Application.Quit();
	}

}