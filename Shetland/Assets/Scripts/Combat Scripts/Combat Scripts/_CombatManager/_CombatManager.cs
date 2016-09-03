//Store the bulk of the static variables that
//make up the player statistics and game progress

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class _CombatManager : MonoBehaviour {

	//PLAYER STATS;
	public static int _maxHealth;
	public static int _currentHealth;
	public static int _maxAP;
	public static int _str = 5;
	public static int _dex = 5;
	public static int _vit = 5;
	public static int _meleeDam;
	public static int _rangedDam;
	public static int _armourRating;
	public static int _init = 50;
	public static List <int> _skills = new List<int>();
	public static int _auxPower;

	//ITEM SLOTS
	public static Weapon _equipMelee;
	public static Weapon _equipRanged;
	public static Armour _headSlot;
	public static Armour _chestSlot;
	public static Armour _legSlot;
	public static bool _inCombat;
	public static bool _victory;

	//Scripts accessed
	public static WeaponDatabase _weaponDb;
	public static ArmourDatabase _armourDb;

	//Initialized once only, stop _manager GO from being destroyed when loading a new scene
	void Awake () {
		print("_CombatManager Awake() Called");
		if (NewGame._newGame){
			_skills.Add(0);
			_skills.Add(1);
			_skills.Add(2);
			_skills.Add(3);
		}		
		DontDestroyOnLoad(gameObject);
		_weaponDb = gameObject.GetComponent<WeaponDatabase>();
		_armourDb = gameObject.GetComponent<ArmourDatabase>();
		_equipMelee = _weaponDb._meleeDatabase[0];
		_equipRanged = _weaponDb._rangedDatabase[0];
		_headSlot = _armourDb._headDatabase[0];
		_chestSlot = _armourDb._chestDatabase[0];
		_legSlot = _armourDb._legDatabase[0];
		CalculateStats();		
	}

	//Calculate derivative stats
	public static void CalculateStats(){
		_rangedDam = _equipRanged._dam;
		_meleeDam = _equipMelee._dam;
		_armourRating = _headSlot._armourBonus + _chestSlot._armourBonus + _legSlot._armourBonus;
		_maxHealth = 300 + 50*_vit;
		_maxAP = 40 + 5*_dex;	
		_currentHealth = _maxHealth;	
	}
}
