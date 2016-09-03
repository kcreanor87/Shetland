//When called, populates a level with enemies 

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombatGenerator : MonoBehaviour {

	//Store the enemies in the scene a list
	//and the spawn points in an array
	public List<CombatStats> _combatants = new List<CombatStats>();
	public List<CombatStats> _sortedInit;
	public List<GameObject> _spawnList = new List<GameObject>();
	public GameObject[] _spawnPoints;

	//Access the EnemyDatabase script and
	//get the expected statistics for the scene
	public EnemyDatabase _db;
	//Create a variable to store the EnemyScript of
	//a combatant when spawned, to set vars such as HP
	public EnemyScript _enemyScript;
	//Turn Manager Script - Allows allocating of enemies remaining in fight
	public TurnManager _tM;
	//Generator stats - how many of what enemy should be spawned
	//***********To be separated into own class**************
	public int _totalSpawned = 3;
	public int _type;
	public int _enemyRangeLower;
	public int _enemyRangeUpper = 2;

	//Initializes the EnemyDatabase script and
	//creates an array of the available spawn points
	void Awake(){
		_db = gameObject.GetComponent<EnemyDatabase>();
		_tM = gameObject.GetComponent<TurnManager>();
		_spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		for (int i = 0; i < _spawnPoints.Length; i++){
			_spawnList.Add(_spawnPoints[i]);
		}
		//Set spawn number to the number of spawn points
		//_totalSpawned = _spawnPoints.Length;
		//Populate the scene
		GenerateEnemies();
		//Call the sort function
		SortByInit();
	}

	//Spawns an enemy of one of the appropriate types until
	//the number of enemies equals the expected value.
	void GenerateEnemies(){
		for (int i = 0; i < _totalSpawned; i++){
			_type = Random.Range(_enemyRangeLower, _enemyRangeUpper);
			_combatants.Add(new CombatStats(
				_db._enemyDB[_type]._charName,
				_db._enemyDB[_type]._id,
				_db._enemyDB[_type]._maxHP,
				_db._enemyDB[_type]._att,
				_db._enemyDB[_type]._dam,
				_db._enemyDB[_type]._def,
				_db._enemyDB[_type]._init,
				_db._enemyDB[_type]._range,
				_db._enemyDB[_type]._moveSpeed));
			_combatants[i]._init += Random.Range(1, 101);
			int j = Random.Range(0, _spawnList.Count);
			var go = Instantiate(_combatants[i]._go, _spawnList[j].transform.position, _spawnList[j].transform.rotation) as GameObject;
			_spawnList.RemoveAt(j);
			_combatants[i]._go = go;
			_enemyScript = _combatants[i]._go.GetComponent<EnemyScript>();
			_enemyScript._maxHP = _combatants[i]._maxHP;
			_enemyScript._def = _combatants[i]._def;
		}
		_combatants.Add(new CombatStats("player"));
		var player = _combatants.First(item => item._charName == "Player");
		player._init = _CombatManager._init + Random.Range(1, 101);
		var playerGO = GameObject.Find("Player");
		player._go = playerGO;
		_tM._activeEnemies = _totalSpawned;
	}

	//Organises the currently active combatants by their initiative value
	//and updates the UI accordingly
	public void SortByInit(){
		_sortedInit = _combatants.OrderByDescending(o=>o._init).ToList();
	}
}
