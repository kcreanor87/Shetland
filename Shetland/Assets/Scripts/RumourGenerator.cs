using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RumourGenerator : MonoBehaviour {

	public Text _rumourText;
	public Text _rumourButtonText;
	public GameObject _rumourGO;
	public Text _rumourTypeTxt, _rumourTownTxt, _rumourModTxt, _rumourTimeTxt;
	public Button _rumourButton;
	public Button _restButton;
	public List <GameObject> _towns = new List<GameObject>();
	public TownManager _activeTown;
	public TownCanvas _townCanvas;
	public DayTimer _dayTimer;
	public bool _rumourActive;
	public int _cost = 50;
	public int _loadedRumourTown;
	public int _loadedRumourType;
	public bool _increase;

	// Use this for initialization
	void Start () {
		_rumourText = GameObject.Find("RumourText").GetComponent<Text>();
		_rumourButton = GameObject.Find("Get Rumour").GetComponent<Button>();
		_restButton = GameObject.Find("Rest").GetComponent<Button>();
		_rumourButtonText = GameObject.Find("RumourButtonText").GetComponent<Text>();
		_rumourTypeTxt = GameObject.Find("RumourType").GetComponent<Text>();
		_rumourTownTxt = GameObject.Find("RumourTown").GetComponent<Text>();
		_rumourModTxt = GameObject.Find("RumourMod").GetComponent<Text>();
		_rumourTimeTxt = GameObject.Find("TimeRemaining").GetComponent<Text>();
		_rumourGO = GameObject.Find("RumourGO");
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
		_townCanvas = gameObject.GetComponent<TownCanvas>();
		_rumourGO.SetActive(false);
		GetTowns();
		if (_rumourActive) LoadRumour();
	}

	public void GenerateRumour(){
		_activeTown._rumourMod = 1.0f;
		var chance = Random.Range(0, 101);
		_loadedRumourTown = Random.Range(0, _towns.Count);
		_activeTown = _towns[_loadedRumourTown].GetComponent<TownManager>();
		if (_towns.Count > 1){
			while (_activeTown._name == _townCanvas._townManager._name){
				_loadedRumourTown = Random.Range(0, _towns.Count);
				_activeTown = _towns[_loadedRumourTown].GetComponent<TownManager>();
			}
		} 
		_loadedRumourType = Random.Range(0, _manager._resources.Count);
		if (chance > 75){			
			//Increase in price
			_activeTown._rumourMod = (Random.Range(1.5f, 2.0f));
			_activeTown._rumourType = _loadedRumourType;
			_increase = true;
			SpawnUI();
			_rumourText.text = "You hear word that " + _manager._resourceNames[_loadedRumourType] + " is scarce in " + _activeTown._name;
		}
		else if (chance > 50){
			//Decrese in price
			_activeTown._rumourMod = (Random.Range(0.3f, 0.6f));
			_activeTown._rumourType = _loadedRumourType;
			_increase = false;
			SpawnUI();
			_rumourText.text = "Somebody mentions that sources of " + _manager._resourceNames[_loadedRumourType] + " are abundant in " + _activeTown._name + " at the moment.";
		}
		else{
			//You hear nothing
			_rumourText.text = "You hear nothing but local anecdotes and petty arguments";
		}
		_manager._obols -= _cost;
		WM_UI.UpdateUI();
		_activeTown.GeneratePrices();
		_rumourActive = true;
		_rumourButton.interactable = false;
	}

	public void ClearRumour(){
		_rumourGO.SetActive(false);
		_activeTown._rumourMod = 1.0f;
		_rumourActive = false;
		_activeTown.GeneratePrices();
	}

	public void EnterText(){
		_rumourText.text = (_rumourActive) ? "There is nothing new to find out, you should come back tomorrow" : "You can spend a few coins to hear the latest rumours";
		_rumourButtonText.text = "Rumour (" + _cost + "o)";
		_rumourButton.interactable = (!_rumourActive && _manager._obols >= _cost);
		_restButton.interactable = (_dayTimer._hours >= 12 && _manager._obols >= 20);
	}

	void GetTowns(){
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		_activeTown = _towns[0].GetComponent<TownManager>();
	}

	void SpawnUI(){
		_rumourGO.SetActive(true);
		_rumourTypeTxt.text = _manager._resourceNames[_activeTown._rumourType];
		_rumourTownTxt.text = _activeTown._name;
		_rumourTimeTxt.text = (24-_dayTimer._rumourTimer).ToString();
		_rumourModTxt.text = (_activeTown._rumourMod > 1.0f) ? "+" : "-";
	}

	public void LoadRumour(){
		_activeTown = _towns[_loadedRumourTown].GetComponent<TownManager>();
		if (_increase){		
			_activeTown._rumourMod = (Random.Range(1.5f, 2.0f));
			_activeTown._rumourType = _loadedRumourType;
			SpawnUI();
		}
		else{
			_activeTown._rumourMod = (Random.Range(0.3f, 0.6f));
			_activeTown._rumourType = _loadedRumourType;
			SpawnUI();
		}
		WM_UI.UpdateUI();
		_activeTown.GeneratePrices();
		_rumourActive = true;
		_rumourButton.interactable = false;
	}
}
