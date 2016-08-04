using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RumourGenerator : MonoBehaviour {

	public Text _rumourText;
	public Text _rumourButtonText;
	public Button _rumourButton;
	public List <GameObject> _towns = new List<GameObject>();
	public TownManager _activeTown;
	public TownCanvas _townCanvas;
	public bool _rumourActive;
	public int _cost = 50;

	// Use this for initialization
	void Start () {
		_rumourText = GameObject.Find("RumourText").GetComponent<Text>();
		_rumourButton = GameObject.Find("Get Rumour").GetComponent<Button>();
		_rumourButtonText = GameObject.Find("RumourButtonText").GetComponent<Text>();
		_townCanvas = gameObject.GetComponent<TownCanvas>();
		GetTowns();
	}

	public void GenerateRumour(){
		_activeTown._rumourMod = 1.0f;
		var chance = Random.Range(0, 101);
		var town = Random.Range(0, _towns.Count);
		_activeTown = _towns[town].GetComponent<TownManager>();
		if (_towns.Count > 1){
			while (_activeTown._name == _townCanvas._townManager._name){
				town = Random.Range(0, _towns.Count);
				_activeTown = _towns[town].GetComponent<TownManager>();
			}
		}
		var resource = Random.Range(0, _manager._resources.Count);
		if (chance > 75){			
			//Increase in price
			_activeTown._rumourMod = (Random.Range(1.5f, 2.0f));
			_activeTown._rumourType = resource;
			_rumourText.text = "You hear word that " + _manager._resourceNames[resource] + " is scarce in " + _activeTown._name;
		}
		else if (chance > 50){
			//Decrese in price
			_activeTown._rumourMod = (Random.Range(0.3f, 0.6f));
			_activeTown._rumourType = resource;
			_rumourText.text = "You hear word that " + _manager._resourceNames[resource] + " is abundant in " + _activeTown._name;
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
		_activeTown._rumourMod = 1.0f;
		_rumourActive = false;
	}

	public void EnterText(){
		_rumourText.text = (_rumourActive) ? "There is nothing new to find out, you should come back tomorrow" : "You can spend a few coins to hear the latest rumours";
		_rumourButtonText.text = "Rumour (" + _cost + "o)";
		_rumourButton.interactable = (!_rumourActive && _manager._obols >= _cost);
	}

	void GetTowns(){
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		_activeTown = _towns[0].GetComponent<TownManager>();
	}

}
