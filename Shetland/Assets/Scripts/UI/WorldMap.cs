using UnityEngine;
using System.Collections.Generic;

public class WorldMap : MonoBehaviour {

	public GameObject _worldMap;
	public GameObject _wmToggle;
	public Camera _wmCam;
	public Light _playerLight;
	public Light _mainLight;
	public Light _wmLight;
	public List <GameObject> _towns = new List <GameObject>();
	public List <TownManager> _townManagers = new List <TownManager>();
	public List <GameObject> _factories = new List <GameObject>();
	public List <Factories> _factoryScripts = new List <Factories>();
	public List <GameObject> _prefabs = new List <GameObject>();
	public List <GameObject> _activeIcons = new List <GameObject>();
	public EndGame _harbourScript;
	public GameObject _harbour;

	void Start(){		
		_wmCam = GameObject.Find("WorldMapCamera").GetComponent<Camera>();
		_worldMap = GameObject.Find("WorldMap");
		_wmToggle = GameObject.Find("MapToggle");
		_playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
		_mainLight = GameObject.Find("MainLight").GetComponent<Light>();
		_wmLight = GameObject.Find("WMLight").GetComponent<Light>();
		_harbour = GameObject.Find("Harbour");
		_harbourScript = GameObject.Find("HarbourCanvas").GetComponent<EndGame>();
		FindBuildings();
		_wmCam.enabled = false;
		_worldMap.SetActive(false);
	}

	public void OpenMap(){
		SpawnIcons();
		Time.timeScale = 0.0f;
		PlayerControls_WM._inMenu = true;
		_wmCam.enabled = true;
		_playerLight.enabled = false;
		_mainLight.enabled = false;
		_wmLight.enabled = true;
		_worldMap.SetActive(true);
		_wmToggle.SetActive(false);
	}

	public void CloseMap(){
		_playerLight.enabled = true;
		_mainLight.enabled = true;
		_wmLight.enabled = false;
		_wmCam.enabled = false;
		_worldMap.SetActive(false);
		_wmToggle.SetActive(true);
		Time.timeScale = 1.0f;
		PlayerControls_WM._inMenu = false;
		ClearIcons();

	}

	void FindBuildings(){
		_factories.AddRange(GameObject.FindGameObjectsWithTag("Factory"));
		for (int i = 0; i < _factories.Count; i++){
			_factoryScripts.Add(_factories[i].GetComponent<Factories>());
		}
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		for (int i = 0; i <_towns.Count; i++){
			_townManagers.Add(_towns[i].GetComponent<TownManager>());
		}
	}

	void SpawnIcons(){
		Quaternion rot = Quaternion.Euler(0, 0, 45f);
		for (int i = 0; i < _townManagers.Count; i++){			
			if (_townManagers[i]._seen){
				var pos = _wmCam.WorldToScreenPoint(_townManagers[i].gameObject.transform.position);
				var prefab = (GameObject) Instantiate(_prefabs[0], pos, rot, _worldMap.transform);
				var script = prefab.GetComponent<WorldMap_Icons>();
				script._name = _townManagers[i]._name;
				_activeIcons.Add(prefab);
			}			
		}
		for (int i = 0; i < _factoryScripts.Count; i++){
			if (_factoryScripts[i]._seen){
				var pos = _wmCam.WorldToScreenPoint(_factoryScripts[i].gameObject.transform.position);
				var type = _factoryScripts[i]._type + 1;
				var prefab = (GameObject) Instantiate(_prefabs[type], pos, rot, _worldMap.transform);
				var script = prefab.GetComponent<WorldMap_Icons>();
				script._name = _factoryScripts[i]._name;
				_activeIcons.Add(prefab);
			}			
		}
		if (_harbourScript._seen){
			var pos = _wmCam.WorldToScreenPoint(_harbour.transform.position);
			var prefab = (GameObject) Instantiate(_prefabs[5], pos, rot, _worldMap.transform);
			var script = prefab.GetComponent<WorldMap_Icons>();
			script._name = "Harbour";
			_activeIcons.Add(prefab);
		}
		var playerPos = _wmCam.WorldToScreenPoint(_playerLight.gameObject.transform.position);
		var player = (GameObject) Instantiate(_prefabs[6], playerPos, rot, _worldMap.transform);
		var playerScript = player.GetComponent<WorldMap_Icons>();
		playerScript._name = "Player";
		_activeIcons.Add(player);
	}

	void ClearIcons(){
		for (int i = 0; i < _activeIcons.Count; i++){
			Destroy(_activeIcons[i]);
		}
		_activeIcons.Clear();
	}
}
