using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TownCanvas : MonoBehaviour {

	public TownManager _townManager;
	public Market _market;
	public Canvas _canvas;
	public GameObject _welcomeGO;
	public RumourGenerator _rumourGen;
	public List <GameObject> _buildings = new List<GameObject>();
	public List <GameObject> _buildingCanvas = new List<GameObject>();
	public List <Button> _workshopButtons = new List <Button>();
	public Text _marketName, _innName, _smithName;

	void Start(){
		PopulateGOList();
		_marketName = GameObject.Find("MarketButtonText").GetComponent<Text>();
		_innName = GameObject.Find("InnButtonText").GetComponent<Text>();
		_smithName = GameObject.Find("SmithButtonText").GetComponent<Text>();
		_rumourGen = gameObject.GetComponent<RumourGenerator>();
		_canvas = gameObject.GetComponent<Canvas>();
		_market = gameObject.GetComponent<Market>();
		_welcomeGO = GameObject.Find("Welcome");
		_canvas.enabled = false;
	}

	public void OpenCanvas(){
		SetActiveBuildings();
		Time.timeScale = 0.0f;
		_canvas.enabled = true;
		_market._townManager = _townManager;
		_market.UpdatePrices();		
	}

	public void CloseCanvas(){
		_canvas.enabled = false;
		Time.timeScale = 1.0f;
		WM_UI.UpdateUI();
	}

	public void OpenBuilding(int index){
		_buildingCanvas[index].SetActive(true);
		_welcomeGO.SetActive(false);
		WorkshopActivate();
		_rumourGen.EnterText();
	}

	public void CloseBuilding(int index){
		_buildingCanvas[index].SetActive(false);
		_welcomeGO.SetActive(true);
		SetActiveBuildings();
	}

	void PopulateGOList(){
		_buildings.AddRange(GameObject.FindGameObjectsWithTag("Building"));
		_buildings.Reverse();
		_buildingCanvas.AddRange(GameObject.FindGameObjectsWithTag("BuildingCanvas"));
		_buildingCanvas.Reverse();
		for (int i = 0; i < _buildingCanvas.Count; i++){
			_buildingCanvas[i].SetActive(false);
		}
	}

	void SetActiveBuildings(){
		for (int i = 0; i < _buildings.Count; i++){
			_buildings[i].SetActive(_townManager._activeBuildings[i]);
		}
	}

	void WorkshopActivate(){
		for (int i = 0; i < _workshopButtons.Count; i++){
			_workshopButtons[i].interactable = !_townManager._activeBuildings[i] /* && Resouces available*/;
		}
	}

	public void PurchaseBuilding(int index){
		_townManager._activeBuildings[index] = true;
		WorkshopActivate();
		UpgradableBuildingNames();
	}

	void UpgradableBuildingNames(){
		if (_townManager._activeBuildings[12]){
			_marketName.text = "Exchange";
		}
		else if (_townManager._activeBuildings[10]){
			_marketName.text = "Bazaar";
		}
		if (_townManager._activeBuildings[11] && _townManager._activeBuildings[2]){
			_innName.text = "Town Hall";
		}
		if (_townManager._activeBuildings[13] && _market._townManager._activeBuildings[5]){
			_smithName.text = "Arsenal";
		}
	}
}
