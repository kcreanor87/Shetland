using UnityEngine;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {

	public List <GameObject> _enemyDatabase = new List <GameObject>();
	public int _spawnChance = 50;
	public EnemyWM _spawnScript;

	public void StartSpawn(){

	}

	void CheckCurrentSpawn(){
		if (!_spawnScript._spotted){			
			CheckRNG();
		}
	}

	void CheckRNG(){
		var chance = Random.Range(0, 101);
		if (chance >= _spawnChance){
			Destroy(_spawnScript.gameObject);
			SpawnEnemy();
		}
	}

	void SpawnEnemy(){
		var enemyType = Random.Range(0, _enemyDatabase.Count);
		Instantiate(_enemyDatabase[enemyType], transform.position, Quaternion.identity);
	}
}
