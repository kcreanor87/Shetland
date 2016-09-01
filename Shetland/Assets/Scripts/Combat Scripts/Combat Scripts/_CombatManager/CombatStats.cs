//Stores specific enemy stats, and create system to 
//make it simple to declare in the EemyDatabase script

using UnityEngine;

[System.Serializable]
public class CombatStats {

	//Create base stats, model and portrait
	public string _charName;
	public int _id;
	public int _maxHP;
	public int _att;
	public int _dam;
	public int _def;
	public int _init;
	public float _range;
	public float _moveSpeed;
	public Sprite _portrait;
	public GameObject _go;

	//For use when instantiating an empty CombatStats file, for populating later via EnemyDatabase
	public CombatStats(){

	}

	//For use in the EnemyDatabase script, make each variable editable when instantiating the script.
	public CombatStats(string _name, int id, int maxHP, int att, int dam, int def, int init, float range, float speed){
		_charName = _name;
		_id = id;
		_maxHP = maxHP;
		_att = att;
		_dam = dam;
		_def = def;
		_init = init;
		_range = range;
		_moveSpeed = speed;
		_go = Resources.Load("Prefabs/Enemies/" + _charName, typeof(GameObject)) as GameObject;
	}
	//A stat selection for the player, so it can be comparable with those of the enemies via List function
	public CombatStats(string ThisIsThePlayer){
		_charName = "Player";
		_def = _CombatManager._armourRating;
		_go = Resources.Load("Prefabs/Player/" + _charName, typeof(GameObject)) as GameObject;
	}
}
