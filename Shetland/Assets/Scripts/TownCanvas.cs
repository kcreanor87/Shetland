using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TownCanvas : MonoBehaviour {

	public TownManager _townManager;
	public Market _market;
	public Canvas _canvas;
	public GameObject _welcomeGO;
	public Canvas _ui;
	public List <GameObject> _buildings = new List<GameObject>();
	public List <GameObject> _buildingCanvas = new List<GameObject>();
	public List <Button> _workshopButtons = new List <Button>();

	void Start(){
		PopulateGOList();
		_canvas = gameObject.GetComponent<Canvas>();
		_market = gameObject.GetComponent<Market>();
		_ui = GameObject.Find("UI").GetComponent<Canvas>();
		_welcomeGO = GameObject.Find("Welcome");
		_canvas.enabled = false;
	}

	public void OpenCanvas(){
		SetActiveBuildings();
		Time.timeScale = 0.0f;
		_ui.enabled = false;
		_canvas.enabled = true;
		_market._townManager = _townManager;
		_market.UpdatePrices();		
	}

	public void CloseCanvas(){
		_canvas.enabled = false;
		Time.timeScale = 1.0f;
		_ui.enabled = true;
		WM_UI.UpdateUI();
	}

	public void OpenBuilding(int index){
		_buildingCanvas[index].SetActive(true);
		_welcomeGO.SetActive(false);
		WorkshopActivate();

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
	}
}
