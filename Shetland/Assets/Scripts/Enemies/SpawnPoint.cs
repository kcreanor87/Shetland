using UnityEngine;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {

	public List <GameObject> _enemyDatabase = new List <GameObject>();
	public int _spawnChance = 100;
	public bool _generated;
	//Bool for determining whether this spawns enemy was in the latest combat
	public bool _fought;
	public bool _spawnOnScreen;
	public EnemyWM _spawnScript;

	public void StartSpawn(){
		transform.FindChild("Indicator").gameObject.SetActive(false);
		//SBegin spawn if not previously in combat
		if (!_fought){
			CheckCurrentSpawn();
		}
		//Reset _fought bool, now a generate turn has passed
		else{
			_fought = false;
		}
	}
	//Only run spawn RNG if the current enemy isn't currently chasing player, nor if the spawn is on screen
	void CheckCurrentSpawn(){
		//Is the spawn on screen? 
		var ScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		_spawnOnScreen = ((ScreenPos.x <= Screen.width && ScreenPos.x >= 0) && (ScreenPos.y <= Screen.height && ScreenPos.y >= 0));
		//Did this spawn create an enemy last time?
		if (_generated){
			//If so, has that enemy spotted the player? Is the Spawn Point on Screen?
			if (!_spawnScript._spotted && !_spawnOnScreen){
				//If not, destroy enemy and call the RNG again
				Destroy(_spawnScript.gameObject);			
				CheckRNG();
			}
		}
		//If not, no need to destroy anything, so skip straight to the RNG if the Spawn isn't on screen
		else{	
			if (!_spawnOnScreen){
				CheckRNG();
			}			
		}		
	}
	//Function to control the RNG that determines whether an enemy spawns or not
	void CheckRNG(){
		var chance = Random.Range(0, 101);
		if (chance <= _spawnChance){
			SpawnEnemy();
		}
		else{
			_generated = false;
		}
	}
	//Spawn a random prefab from the editor-populated list, assign it as the actice script and set _generated bool
	void SpawnEnemy(){
		var enemyType = Random.Range(0, _enemyDatabase.Count);
		var newEnemy = (GameObject) Instantiate(_enemyDatabase[enemyType], transform.position, Quaternion.identity);
		var script = newEnemy.GetComponent<EnemyWM>();
		script._index = enemyType;
		_spawnScript = script;
		_spawnScript._thisSpawn = gameObject.GetComponent<SpawnPoint>();
		_generated = true;
	}
}
