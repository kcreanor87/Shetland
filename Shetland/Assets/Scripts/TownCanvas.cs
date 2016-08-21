using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TownCanvas : MonoBehaviour {

	public TownManager _townManager;
	public Market _market;
	public Canvas _canvas;
	public GameObject _welcomeGO;
	public RumourGenerator _rumourGen;
	public DayTimer _dayTimer;
	public Caravan _caravan;
	public List <GameObject> _buildings = new List<GameObject>();
	public List <GameObject> _buildingCanvas = new List<GameObject>();
	public List <Button> _workshopButtons = new List <Button>();
	public Text _marketName, _innName, _smithName;
	public Text _townName;
	public AdditionalResources _additionalResources;
	public List <BuildingCost> _buildingCosts = new List <BuildingCost>();
	public bool _affordable;

	void Start(){
		PopulateGOList();
		_marketName = GameObject.Find("MarketButtonText").GetComponent<Text>();
		_innName = GameObject.Find("InnButtonText").GetComponent<Text>();
		_smithName = GameObject.Find("SmithButtonText").GetComponent<Text>();
		_townName = GameObject.Find("TownName").GetComponent<Text>();
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
		_additionalResources = gameObject.GetComponent<AdditionalResources>();
		_rumourGen = gameObject.GetComponent<RumourGenerator>();
		_caravan = gameObject.GetComponent<Caravan>();
		_canvas = gameObject.GetComponent<Canvas>();
		_market = gameObject.GetComponent<Market>();
		_welcomeGO = GameObject.Find("Welcome");
		_canvas.enabled = false;
	}

	public void OpenCanvas(){
		_townName.text = _townManager._name;
		UpgradableBuildingNames();
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
		_welcomeGO.SetActive(false);
		WorkshopActivate();
		_rumourGen.EnterText();
		_caravan.Open();		
		if (index == 4){
			_additionalResources.CheckResources(0);
			_buildingCanvas[4].SetActive(true);			
		}
		else if (index == 7){			
			_additionalResources.CheckResources(1);
			_buildingCanvas[4].SetActive(true);
		}
		else{
			_buildingCanvas[index].SetActive(true);
		}
	}

	public void CloseBuilding(int index){
		_buildingCanvas[index].SetActive(false);
		_welcomeGO.SetActive(true);
		SetActiveBuildings();
	}

	void PopulateGOList(){
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
			CheckAffordability(i);
			_workshopButtons[i].interactable = !_townManager._activeBuildings[i] && _affordable;
			_affordable = false;
		}
	}

	public void PurchaseBuilding(int index){
		_manager._resources[0] -= _buildingCosts[index]._woodCost;
		_manager._resources[1] -= _buildingCosts[index]._stoneCost;
		_manager._resources[2] -= _buildingCosts[index]._ironCost;
		_manager._resources[3] -= _buildingCosts[index]._coalCost;
		WM_UI.UpdateUI();
		_townManager._activeBuildings[index] = true;
		WorkshopActivate();
		UpgradableBuildingNames();
	}

	void CheckAffordability(int buildingType){
		var increment = 0;
		if (_manager._resources[0] >= _buildingCosts[buildingType]._woodCost) increment++;
		if (_manager._resources[1] >= _buildingCosts[buildingType]._stoneCost) increment++;
		if (_manager._resources[2] >= _buildingCosts[buildingType]._ironCost) increment++;
		if (_manager._resources[3] >= _buildingCosts[buildingType]._coalCost) increment++;
		_affordable = (increment == 4);
	}

	public void Rest(){
		_dayTimer.Sleep();
		CloseBuilding(2);		
	}

	void UpgradableBuildingNames(){
		if (_townManager._activeBuildings[10]){
			_marketName.text = "Exchange";
			_townManager._marketBuyMod = 1.05f;
			_townManager._marketSellMod = 0.9f;
			_townManager.UpdatePrices();
		}
		else if (_townManager._activeBuildings[8]){
			_marketName.text = "Bazaar";
			_townManager._marketBuyMod = 1.15f;
			_townManager._marketSellMod = 0.8f;
			_townManager.UpdatePrices();
		}
		if (_townManager._activeBuildings[9] && _townManager._activeBuildings[2]){
			_innName.text = "Town Hall";
		}
		if (_townManager._activeBuildings[11] && _market._townManager._activeBuildings[5]){
			_smithName.text = "Arsenal";
		}
	}
}
