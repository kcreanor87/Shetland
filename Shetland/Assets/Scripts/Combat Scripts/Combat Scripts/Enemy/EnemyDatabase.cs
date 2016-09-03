//Stores all potential enemies in an easily accessible database

using UnityEngine;
using System.Collections.Generic;

public class EnemyDatabase : MonoBehaviour {

	//Initialize a list of CombatStats scripts
	public List<CombatStats> _enemyDB = new List<CombatStats>();

	//Populate the list with a database of enemies for use throughout the game
	void Awake(){
		_enemyDB.Add(new CombatStats("Placeholder - Melee", 0, 300, 85, 150, 10, 50, 0f, 10.0f));
		_enemyDB.Add(new CombatStats("Placeholder - Ranged", 1, 300, 85, 120, 0, 50, 15.0f, 10.0f));
	}
}
