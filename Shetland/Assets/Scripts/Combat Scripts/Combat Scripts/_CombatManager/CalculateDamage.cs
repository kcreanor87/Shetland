//Script that determines the damage being dealt by the active gameObject

using UnityEngine;

public class CalculateDamage : MonoBehaviour {

	//Store variables including scripts and UI to access
	private TurnManager _tM;
	public PlayerTurn _pT;
	public CombatUI _combatUI;
	public AuxilliaryPower _aux;
	public int _chanceToHit;
	public bool _ranged;
	public int _att;
	public int _def;
	public int _dam;
	public int _totalDamage;
	public int _toHitModifier;
	public int _rangeModifier;

	public int _accuracy;

    //On scene load find the scripts and UI in the scene
    void Start()
    {	
    	_aux = gameObject.GetComponent<AuxilliaryPower>();
		_tM = gameObject.GetComponent<TurnManager>();
		_pT = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_combatUI = gameObject.GetComponent<CombatUI>();
	}

	public void CalculateChance(){
		//If active GO is the player, set DEF to the target enemy
		if (_tM._activeCombatant._charName == "Player"){
			var enemy = _pT._enemyTrans.GetComponent<EnemyScript>();
			_def = enemy._def;
			if (_pT._meleeToggle){
				_att = 100;
				_dam = Mathf.FloorToInt((_CombatManager._meleeDam + _aux._auxDam) * _aux._damageBonus * _CombatManager._str);
				_rangeModifier = 0;
			}
			else{
				_att = 40 +(_CombatManager._dex*5);
				_dam = _CombatManager._rangedDam;
			}
		}
		//If active GO is not the player, set DEF to that in _manager script
		//and _att to that of the enemy
		else{
			_def = Mathf.FloorToInt((_CombatManager._armourRating + _aux._auxArmour) * _aux._armourBonus) ;
			_att = _tM._activeCombatant._att;
			_dam = _tM._activeCombatant._dam;
			_aux._aimBonus = 1.0f;
			_toHitModifier = 0;
			_rangeModifier = 0;
		}
		//Calculate ChanceToHit, including modifiers from attack type and range
		_chanceToHit = Mathf.FloorToInt((_att - _def + _toHitModifier + _rangeModifier)*_aux._aimBonus);
		if (_chanceToHit > 100){
			_chanceToHit = 100;
		}
		else if (_chanceToHit < 1){
			_chanceToHit = 0;
		}
	}

	//Funciton for calling by other scripts when an attack is successful
	public void Damage(){
		//Reset the damage to 0
		_totalDamage = 0;		
		//Make Attack Roll
		var AttRoll = Random.Range(1, 101);
		//Check if the attack hits
		if (AttRoll <=  _chanceToHit){
			_totalDamage = _dam;
			//If the player is active, deduct the damage from the targe enemy script
			if (_tM._activeCombatant._charName == "Player"){
				var enemy = _pT._enemyTrans.GetComponent<EnemyScript>();
				enemy._currentHP -= _totalDamage;
				enemy.BeenHit();
				enemy.Health();
				_pT.Health();
			}
			//Otherwise, deduct from player health in _manager script
			else{
				_CombatManager._currentHealth -= _totalDamage;
				//Update the UI display to show new health
				_combatUI.UpdateHealth();
			}
		}
		else{			
			_totalDamage = 0;
			if (_tM._activeCombatant._charName == "Player"){
				var enemy = _pT._enemyTrans.GetComponent<EnemyScript>();
				enemy.BeenHit();
			}
		}
	}
}
