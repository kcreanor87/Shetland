//Controls the turn order
using UnityEngine;

public class TurnManager : MonoBehaviour {
	//Access relevant scripts of combatants/generator
	public CombatGenerator _generator;
	public CombatStats _activeCombatant;
	public CombatOver _combatOver;
	public EnemyScript _enemy;
	public PlayerTurn _player;
	//Store current index as int
	public int _turnIndex;
	//Variables for preventing move while camera updates
	public bool _waitingForSeconds;
	public float _timer = 1.2f;

	public int _activeEnemies;

	void Start(){
		//Initialize the scripts
		_player = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_generator = gameObject.GetComponent<CombatGenerator>();		
		_combatOver = GameObject.Find("EndCanvas").GetComponent<CombatOver>();
		//Initialize the _enemy script by picking one from the _generator script
		for (int i = 1; i < _generator._sortedInit.Count; i++){
			if (_generator._sortedInit[i]._charName != "Player"){
				_enemy = _generator._sortedInit[i]._go.GetComponent<EnemyScript>();
			}
		}
		//Begin the turn
		ActivateGO();
	}

	void Update(){
		TurnTimeBuffer();
	}

	//Find and activate the script for the current turn GO
	void ActivateGO(){
		//Using the turn index, access the script of the relative combatant and initialize
		_activeCombatant = _generator._sortedInit[_turnIndex];
		if (_activeCombatant._charName == "Player"){
			_player.Initialize();
			//Enable timer to allow camera time to move
			_waitingForSeconds = true;
		}
		else{
			_enemy = _activeCombatant._go.GetComponent<EnemyScript>();
			_enemy.Initialize();
			if (!_enemy._dead){
				//Enable timer to allow camera time to move
				_waitingForSeconds = true;
			}
			else{
				NextTurn();
			}
		}
	}

	//End the current turn
	public void NextTurn(){
		if (_activeEnemies == 0){
			_combatOver._playerWin = true;
			_combatOver.EnableEndCanvas();
			return;
		}
		if (_player._dead) return;
		//Disable the active variable for the current combatant and activate NavMeshObstacle
		_enemy._active = false;
		//_enemy._agent.enabled = false;
		//_enemy._obstacle.enabled = true;
		_enemy._agent.Stop();
		_player._active = false;
		//_player._agent.enabled = false;
		//_player._obstacle.enabled = true;
		_player._agent.Stop();

		//Increase the turn index by 1 or drop to 0, update the portrait highlighter
		if (_turnIndex < (_generator._sortedInit.Count - 1)){
			_turnIndex++;
		}
		else{
			_turnIndex = 0;
		}
		//Call the function to enable the next GO
		ActivateGO();
	}

	//Function to stop current combatant taking actions
	//Used by camera and animations
	void TurnTimeBuffer(){
		if (!_waitingForSeconds) return;
		_timer -= Time.deltaTime;
		if (_timer <= 0){
		_waitingForSeconds = false;
		_timer = 1.2f;
		}
	}
}
