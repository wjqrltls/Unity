using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour{

	public Enemy enemy;

	public WaveSpawner waveSpawner;

	public void ReStart(){

		SceneManager.LoadScene("MainScene");
		LifeManager.life = 10;
		MoneyCounter.Money = 1000;
		WaveSpawner.waveIndex = 0;
		WaveSpawner.difficulty = 50;
		enemy.hp = 100;
	}

	public void EndGame(){

		Application.Quit();
	}
}