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
	public Text _woodCost, _stoneCost, _ironCost, _coalCost, _preReqs, _buildName, _buildDesc;
	public AdditionalResources _additionalResources;
	public List <BuildingCost> _buildingCosts = new List <BuildingCost>();
	public bool _affordable;
	public Button _buildButton;
	public int _activeBuilding;
	public SaveGame _saveGame;
	public GameObject _mapToggle;

	void Start(){
		CollectTextElements();	
		_saveGame = GameObject.Find("Loader").GetComponent<SaveGame>();			
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
		_buildButton = GameObject.Find("BuildButton").GetComponent<Button>();
		_additionalResources = gameObject.GetComponent<AdditionalResources>();
		_rumourGen = gameObject.GetComponent<RumourGenerator>();
		_caravan = gameObject.GetComponent<Caravan>();
		_canvas = gameObject.GetComponent<Canvas>();
		_market = gameObject.GetComponent<Market>();
		_welcomeGO = GameObject.Find("Welcome");
		PopulateGOList();
		_canvas.enabled = false;
	}

	void CollectTextElements(){
		_mapToggle = GameObject.Find("MapToggle");
		_woodCost = GameObject.Find("WoodBuild").GetComponent<Text>();
		_stoneCost = GameObject.Find("StoneCost").GetComponent<Text>();
		_ironCost = GameObject.Find("IronCost").GetComponent<Text>();
		_coalCost = GameObject.Find("CoalCost").GetComponent<Text>();
		_preReqs = GameObject.Find("PreReqs").GetComponent<Text>();
		_buildName = GameObject.Find("BuildingName").GetComponent<Text>();
		_buildDesc = GameObject.Find("BuildingDescription").GetComponent<Text>();
		_marketName = GameObject.Find("MarketButtonText").GetComponent<Text>();
		_innName = GameObject.Find("InnButtonText").GetComponent<Text>();
		_smithName = GameObject.Find("SmithButtonText").GetComponent<Text>();
		_townName = GameObject.Find("TownName").GetComponent<Text>();
	}

	public void OpenCanvas(){
		_saveGame.Save();
		_mapToggle.SetActive(false);
		_townName.text = _townManager._name;
		UpgradableBuildingNames();
		SetActiveBuildings();
		Time.timeScale = 0.0f;
		_canvas.enabled = true;
		_market._townManager = _townManager;
		_market.UpdatePrices();
	}

	public void CloseCanvas(){
		_mapToggle.SetActive(true);
		_canvas.enabled = false;
		Time.timeScale = 1.0f;
		WM_UI.UpdateUI();
	}

	public void OpenBuilding(int index){		
		_welcomeGO.SetActive(false);
		_rumourGen.EnterText();
		_caravan.Open();
		UpdateWorkshopButtons();		
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
	void UpdateWorkshopButtons(){
		for (int i = 0; i < _workshopButtons.Count; i++){
			var image = _workshopButtons[i].gameObject.GetComponent<Image>();
			CheckAffordability(i);
			if (_affordable && !_townManager._activeBuildings[i]){
				image.color = Color.green;
			}			
			else if (!_affordable && !_townManager._activeBuildings[i]){
				image.color = Color.red;
			}
			else{
				image.color = Color.white;
			}
		}
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

	public void OpenBuildCanvas(int index){
		_activeBuilding = index;
		_woodCost.text = _buildingCosts[index]._woodCost.ToString();
		_stoneCost.text = _buildingCosts[index]._stoneCost.ToString();
		_ironCost.text = _buildingCosts[index]._ironCost.ToString();
		_coalCost.text = _buildingCosts[index]._coalCost.ToString();
		_buildName.text = _buildingCosts[index]._name;
		_buildDesc.text = _buildingCosts[index]._desc;
		_preReqs.text = (_buildingCosts[index]._preReq > 0) ? _preReqs.text = _buildingCosts[_buildingCosts[index]._preReq]._name : null;
		CheckAffordability(index);
		_buildButton.interactable = _affordable && !_townManager._activeBuildings[index];
		_buildingCanvas[7].SetActive(true);
	}

	public void PurchaseBuilding(){
		_manager._resources[0] -= _buildingCosts[_activeBuilding]._woodCost;
		_manager._resources[1] -= _buildingCosts[_activeBuilding]._stoneCost;
		_manager._resources[2] -= _buildingCosts[_activeBuilding]._ironCost;
		_manager._resources[3] -= _buildingCosts[_activeBuilding]._coalCost;
		WM_UI.UpdateUI();
		_townManager._activeBuildings[_activeBuilding] = true;
		UpdateWorkshopButtons();
		UpgradableBuildingNames();
		_buildingCanvas[7].SetActive(false);
	}

	void CheckAffordability(int buildingType){
		var increment = 0;
		if (_manager._resources[0] >= _buildingCosts[buildingType]._woodCost) increment++;
		if (_manager._resources[1] >= _buildingCosts[buildingType]._stoneCost) increment++;
		if (_manager._resources[2] >= _buildingCosts[buildingType]._ironCost) increment++;
		if (_manager._resources[3] >= _buildingCosts[buildingType]._coalCost) increment++;
		if (_townManager._activeBuildings[_buildingCosts[buildingType]._preReq]) increment++;
		_affordable = (increment == 5);
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
