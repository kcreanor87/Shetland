using UnityEngine;
using System.Collections.Generic;

public class SaveGame : MonoBehaviour {

	public List <GameObject> _towns = new List <GameObject>();
	public List <TownManager> _buildingScripts = new List<TownManager>();
	public List <Factories> _factories = new List <Factories>();
	public List <GameObject> _factoryGO = new List <GameObject>();
	public List <GameObject> _resourceSpawns = new List <GameObject>();
	public List <ResourceGen> _resourceGens = new List <ResourceGen>();
	public List <GameObject> _fowGO = new List <GameObject>();
	public List <FOW> _fow = new List <FOW>();
	public List <GameObject> _spawnGOs = new List <GameObject>();
	public List <SpawnPoint> _spawns = new List <SpawnPoint>();
	public EndGame _endGame;
	public Vector3 _playerPos;
	public GameObject _player;
	public PlayerControls_WM _playerControls;
	public DayTimer _dayTimer;
	public DayCycle _dayCycle;
	public RumourGenerator _rumourScript;
	public bool _combatOver;
	public bool _skipDay;

	void Awake(){
		PopulateLists();
		if (!NewGame._newGame) Load();
		else PlayerPrefs.DeleteAll();
	}

	void Start(){
		if (!NewGame._newGame) LoadTowns();
	}

	void PopulateLists(){
		_player = GameObject.FindWithTag("Player");
		_endGame = GameObject.Find("HarbourCanvas").GetComponent<EndGame>();
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
		_dayCycle = GameObject.Find("MainLight").GetComponent<DayCycle>();
		_rumourScript = GameObject.Find("TownCanvas").GetComponent<RumourGenerator>();
		_playerControls = _player.GetComponent<PlayerControls_WM>();
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		for (int i = 0; i < _towns.Count; i++){
			_buildingScripts.Add(_towns[i].GetComponent<TownManager>());
		}
		_factoryGO.AddRange(GameObject.FindGameObjectsWithTag("Factory"));
		for (int i = 0; i < _factoryGO.Count; i++){
			_factories.Add(_factoryGO[i].GetComponent<Factories>());
		}
		_resourceSpawns.AddRange(GameObject.FindGameObjectsWithTag("ResourceSpawn"));
		for (int i = 0; i < _resourceSpawns.Count; i++){
			_resourceGens.Add(_resourceSpawns[i].GetComponent<ResourceGen>());
		}
		_fowGO.AddRange(GameObject.FindGameObjectsWithTag("FOW"));
		for (int i = 0; i < _fowGO.Count; i++){
			_fow.Add(_fowGO[i].GetComponent<FOW>());
		}
		_spawnGOs.AddRange(GameObject.FindGameObjectsWithTag("Spawn Point"));
		for (int i = 0; i < _spawnGOs.Count; i++){
			_spawns.Add(_spawnGOs[i].GetComponent<SpawnPoint>());
		}
	}

	public void Save(){
		SaveTowns();
		SaveFactories();
		SavePlayerPos();
		SavePlayerResources();
		SaveResourceSpawns();
		SaveTimeOfDay();
		SaveRumour();
		SaveHarbourProgress();
		SaveFOW();
		SaveEnemies();
		SaveCombatStats();
		NewGame._newGame = false;
	}

	public void Load(){
		LoadResourceSpawns();		
		LoadFactories();				
		LoadHarbourProgress();
		LoadFOW();
		LoadEnemies();
		LoadCombatStats();
		//Load Combat Specific Stats
		if (_CombatManager._inCombat){
			PostCombatLoad();
		}
		else{
			NormalLoad();
		}		
	}

	void NormalLoad(){
		LoadPlayerPos();
		LoadPlayerResources();
		LoadTimeOfDay();
		LoadRumour();		
	}

	void PostCombatLoad(){
		print("Combat Load");
		if (_CombatManager._victory){
			NormalLoad();
		}
		else{
			LoadLastTownPos();
			LoadTimeOfDay();
			_skipDay = true;
			LoadRumour();
			LoadPlayerResources();
			LoseResources();
		}		
		_CombatManager._inCombat = false;
		_CombatManager._victory = false;
		Save();
	}

	void SaveTowns(){
		for (int i = 0; i < _buildingScripts.Count; i++){
			for (int j = 0; j < _buildingScripts[i]._activeBuildings.Count; j++){
				PlayerPrefs.SetInt(_buildingScripts[i]._name + i + "_" + j, _buildingScripts[i]._activeBuildings[j] ? 1 : 0);				
			}
			PlayerPrefs.SetInt("TownSeen" + i, (_buildingScripts[i]._seen ? 1 : 0));
			PlayerPrefs.SetInt("TownVisited" + i, (_buildingScripts[i]._visited ? 1 : 0));
		}
	}

	void LoadTowns(){
		for (int i = 0; i < _buildingScripts.Count; i++){
			for (int j = 0; j < _buildingScripts[i]._activeBuildings.Count; j++){
				_buildingScripts[i]._activeBuildings[j] = PlayerPrefs.GetInt(_buildingScripts[i]._name + i + "_" + j) > 0;				
			}
			_buildingScripts[i]._seen = (PlayerPrefs.GetInt("TownSeen" + i) > 0);
			_buildingScripts[i]._visited = (PlayerPrefs.GetInt("TownVisited" + i) > 0);
		}
	}

	void SaveFactories(){
		for (int i = 0; i < _factories.Count; i++){
			PlayerPrefs.SetInt("Factory" + i, _factories[i]._factoryLevel);
			PlayerPrefs.SetInt("FactorySeen" + i, (_factories[i]._seen ? 1 : 0));
		}
	}

	void LoadFactories(){
		for (int i = 0; i < _factories.Count; i++){
			_factories[i]._factoryLevel = PlayerPrefs.GetInt("Factory" + i);
			_factories[i]._seen = (PlayerPrefs.GetInt("FactorySeen" + i) > 0);
			_factories[i].SwitchMesh();
		}
	}

	void SavePlayerPos(){
		PlayerPrefs.SetFloat("PosX", _player.transform.position.x);
		PlayerPrefs.SetFloat("PosY", _player.transform.position.y);
		PlayerPrefs.SetFloat("PosZ", _player.transform.position.z);
	}
	void LoadPlayerPos(){
		var pos = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
		_player.transform.position = pos;
		if (_playerControls._agent != null)	_playerControls._agent.SetDestination(pos);
	}
	public void SaveLastTownPos(Vector3 town){
		PlayerPrefs.SetFloat("TownPosX", town.x);
		PlayerPrefs.SetFloat("TownPosY", town.y);
		PlayerPrefs.SetFloat("TownPosZ", town.z);
	}
	void LoadLastTownPos(){
		var pos = new Vector3(PlayerPrefs.GetFloat("TownPosX"), PlayerPrefs.GetFloat("TownPosY"), PlayerPrefs.GetFloat("TownPosZ"));
		_player.transform.position = pos;
		if (_playerControls._agent != null)	_playerControls._agent.SetDestination(pos);
	}

	void SavePlayerResources(){
		for (int i = 0; i < _manager._resources.Count; i++){
			PlayerPrefs.SetInt("Resource" + i, _manager._resources[i]);
		}
		PlayerPrefs.SetInt("Obols", _manager._obols);
		PlayerPrefs.SetInt("Repute", _manager._repute);
		for (int i = 0; i < _manager._factoryOuput.Count; i++){
			PlayerPrefs.SetInt("Output" + i, _manager._factoryOuput[i]);
		}
	}

	void LoadPlayerResources(){
		for (int i = 0; i < _manager._resources.Count; i++){
			_manager._resources[i] = PlayerPrefs.GetInt("Resource" + i);
		}
		_manager._obols = PlayerPrefs.GetInt("Obols");
		_manager._repute = PlayerPrefs.GetInt("Repute");
		for (int i = 0; i < _manager._factoryOuput.Count; i++){
			_manager._factoryOuput[i] = PlayerPrefs.GetInt("Output" + i);
		}
	}
	void SaveResourceSpawns(){
		for (int i = 0; i < _resourceGens.Count; i++){
			PlayerPrefs.SetInt("RandomResource" + i, (_resourceGens[i]._taken ? 1 : 0));
		}
	}

	void LoadResourceSpawns(){
		for (int i = 0; i < _resourceGens.Count; i++){
			_resourceGens[i]._taken = (PlayerPrefs.GetInt("RandomResource" + i) > 0);
		}
	}
	void SaveTimeOfDay(){
		PlayerPrefs.SetInt("Days", _dayTimer._days);
		PlayerPrefs.SetInt("Hours", _dayTimer._hours);
		PlayerPrefs.SetInt("RumourTimer", _dayTimer._rumourTimer);
		PlayerPrefs.SetInt("RumourActive", (_rumourScript._rumourActive ? 1 : 0));
	}

	void LoadTimeOfDay(){
		_dayTimer._days = PlayerPrefs.GetInt("Days");
		_dayTimer._hours = PlayerPrefs.GetInt("Hours");
		_dayTimer._rumourTimer = PlayerPrefs.GetInt("RumourTimer");
		_rumourScript._rumourActive = (PlayerPrefs.GetInt("RumourActive") > 0);
		_dayCycle.AdvanceTime();
	}

	void SaveRumour(){
		PlayerPrefs.SetInt("RumourType", _rumourScript._loadedRumourType);
		PlayerPrefs.SetInt("RumourTown", _rumourScript._loadedRumourTown);
		PlayerPrefs.SetInt("Increase", (_rumourScript._increase ? 1 : 0));
	}

	void LoadRumour(){
		_rumourScript._rumourActive = (PlayerPrefs.GetInt("RumourActive") > 0);
		_rumourScript._loadedRumourTown = PlayerPrefs.GetInt("RumourTown");
		_rumourScript._loadedRumourType = PlayerPrefs.GetInt("RumourType");
		_rumourScript._increase = (PlayerPrefs.GetInt("Increase") > 0);
	}

	void SaveHarbourProgress(){
		PlayerPrefs.SetInt("PartA", (_endGame._partA ? 1 : 0));
		PlayerPrefs.SetInt("PartB", (_endGame._partB ? 1 : 0));
		PlayerPrefs.SetInt("PartC", (_endGame._partC ? 1 : 0));
		PlayerPrefs.SetInt("PartD", (_endGame._partD ? 1 : 0));
		PlayerPrefs.SetInt("Built", (_endGame._built ? 1 : 0));
		PlayerPrefs.SetInt("Bought", (_endGame._bought ? 1 : 0));
		PlayerPrefs.SetInt("Bragged", (_endGame._bragged ? 1 : 0));
		PlayerPrefs.SetInt("Ticket", (_endGame._ticket ? 1 : 0));
		PlayerPrefs.SetInt("WinType", _endGame._winType);
		PlayerPrefs.SetInt("HarbourSeen", (_endGame._seen ? 1 :0));
	}

	void LoadHarbourProgress(){
		_endGame._partA = (PlayerPrefs.GetInt("PartA") > 0);
		_endGame._partB = (PlayerPrefs.GetInt("PartB") > 0);
		_endGame._partC = (PlayerPrefs.GetInt("PartC") > 0);
		_endGame._partD = (PlayerPrefs.GetInt("PartD") > 0);
		_endGame._built = (PlayerPrefs.GetInt("Built") > 0);
		_endGame._bought = (PlayerPrefs.GetInt("Bought") > 0);
		_endGame._bragged = (PlayerPrefs.GetInt("Bragged") > 0);
		_endGame._ticket = (PlayerPrefs.GetInt("Ticket") > 0);
		_endGame._winType = PlayerPrefs.GetInt("WinType");
		_endGame._seen = (PlayerPrefs.GetInt("HarbourSeen") > 0);
	}

	void SaveFOW(){
		for (int i = 0; i < _fow.Count; i++){
			PlayerPrefs.SetInt("FOW" + i, (_fow[i]._active ? 1 : 0));
		}
	}

	void LoadFOW(){
		for (int i = 0; i < _fow.Count; i++){
			_fow[i]._active = (PlayerPrefs.GetInt("FOW" + i) > 0);
		}
	}

	void SaveEnemies(){
		for (int i = 0; i < _spawns.Count; i++){
			PlayerPrefs.SetInt("SpawnFought" + i, (_spawns[i]._fought ? 1 : 0));
		}
	}

	void LoadEnemies(){
		for (int i = 0; i < _spawns.Count; i++){
			_spawns[i]._fought = (PlayerPrefs.GetInt("SpawnFought" + i) > 0);
		}
	}

	void SaveCombatStats(){
		PlayerPrefs.SetInt("STR", _CombatManager._str);
		PlayerPrefs.SetInt("DEX", _CombatManager._dex);
		PlayerPrefs.SetInt("VIT", _CombatManager._vit);
		PlayerPrefs.SetInt("Init", _CombatManager._init);
		PlayerPrefs.SetInt("Melee", _CombatManager._weaponDb._meleeDatabase.IndexOf(_CombatManager._equipMelee));
		PlayerPrefs.SetInt("Ranged", _CombatManager._weaponDb._rangedDatabase.IndexOf(_CombatManager._equipRanged));
		PlayerPrefs.SetInt("Head", _CombatManager._armourDb._headDatabase.IndexOf(_CombatManager._headSlot));
		PlayerPrefs.SetInt("Chest", _CombatManager._armourDb._chestDatabase.IndexOf(_CombatManager._chestSlot));
		PlayerPrefs.SetInt("Legs", _CombatManager._armourDb._legDatabase.IndexOf(_CombatManager._legSlot));
		for (int i = 0; i < _CombatManager._skills.Count; i++){
			PlayerPrefs.SetInt("Skill" + i, _CombatManager._skills[i]);
		}
	}

	void LoadCombatStats(){
		_CombatManager._str = PlayerPrefs.GetInt("STR");
		_CombatManager._dex = PlayerPrefs.GetInt("DEX");
		_CombatManager._vit = PlayerPrefs.GetInt("VIT");
		_CombatManager._init = PlayerPrefs.GetInt("Init");
		_CombatManager._currentHealth = PlayerPrefs.GetInt("CurrentHealth");
		_CombatManager._equipMelee = _CombatManager._weaponDb._meleeDatabase[PlayerPrefs.GetInt("Melee")];
		_CombatManager._equipRanged = _CombatManager._weaponDb._rangedDatabase[PlayerPrefs.GetInt("Ranged")];
		_CombatManager._headSlot = _CombatManager._armourDb._headDatabase[PlayerPrefs.GetInt("Head")];
		_CombatManager._chestSlot = _CombatManager._armourDb._chestDatabase[PlayerPrefs.GetInt("Chest")];
		_CombatManager._legSlot = _CombatManager._armourDb._legDatabase[PlayerPrefs.GetInt("Legs")];
		_CombatManager._skills.Clear();
		for (int i = 0; i < 10; i++){
			if (PlayerPrefs.HasKey("Skill" + i)){
				_CombatManager._skills.Add(i);
			}
		}
		_CombatManager.CalculateStats();
	}

	void LoseResources(){
		float loss = Random.Range(0.6f, 0.8f);
		for (int i = 0; i < _manager._resources.Count; i++){
			_manager._resources[i] = Mathf.CeilToInt((float)loss * _manager._resources[i]);
			print (_manager._resources[i]);
		}
	}
}
