using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public Transform enemyprefab;
    public Transform coinenemyprefab;
	int coin = 0;
    
    public Transform spawnPoint;

	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

	public Text waveCountdownText;

	public static int waveIndex = 0;

	public static int difficulty = 100;
	public Enemy enemy;

	void Update () {

		if (countdown <= 0f) {
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
		}

		countdown -= Time.deltaTime;

		waveCountdownText.text = "WAVE : " + waveIndex + "\nNEXT WAVE : " + Mathf.Round(countdown);
	}

	IEnumerator SpawnWave () {

		if(waveIndex % 5 == 0 && waveIndex != 0){
			difficulty += 50;
			if(waveIndex % 25 == 0){
				waveIndex = 10;
				difficulty *= 2;
				Enemy.lessLife += 1;
				enemy.coin += 50;
			}
			enemy.MaxHp(difficulty);
			coin = 1;
		}
		
		waveIndex++;

		for (int i = 0; i < waveIndex; i++) {
			if(coin == 1){
				coin = 0;
				SpawnCoinEnemy();
				yield return new WaitForSeconds(0.5f);
			}
			SpawnEnemy();
			yield return new WaitForSeconds(0.5f);
		}
	}

	void SpawnEnemy () {

		Instantiate(enemyprefab, spawnPoint.position, spawnPoint.rotation);
	}

	void SpawnCoinEnemy () {

		Instantiate(coinenemyprefab, spawnPoint.position, spawnPoint.rotation);
	}

}
