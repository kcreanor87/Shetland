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

	void Start(){		
		_wmCam = GameObject.Find("WorldMapCamera").GetComponent<Camera>();
		_worldMap = GameObject.Find("WorldMap");
		_wmToggle = GameObject.Find("ToggleMap");
		_playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
		_mainLight = GameObject.Find("MainLight").GetComponent<Light>();
		_wmLight = GameObject.Find("WMLight").GetComponent<Light>();
		FindBuildings();
		SpawnIcons();
		_wmCam.enabled = false;
		_worldMap.SetActive(false);
	}

	public void OpenMap(){
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
			var pos = _wmCam.WorldToScreenPoint(_townManagers[i].gameObject.transform.position);
			Instantiate(_prefabs[0], pos, rot, _worldMap.transform);
		}
	}
}
