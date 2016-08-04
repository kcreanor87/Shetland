using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActivateFactory : MonoBehaviour {

	public static GameObject _factoryPrompt;
	public static Text _factoryName;
	public static Text _factoryText;
	public static Text _buttonText;
	public static Text _woodCost, _stoneCost, _ironCost, _coalCost;
	public static string _name;
	public static int _type;
	public static int _amount;
	public static bool _active;
	public static bool _affordable;
	public static Factories _activeFactory;
	public static Button _activateButton;

	public WM_UI _ui;

	// Use this for initialization
	void Start () {
		_factoryPrompt = GameObject.Find("FactoryPrompt");
		_buttonText = GameObject.Find("ActivateText").GetComponent<Text>();
		_factoryName = GameObject.Find("FactoryName").GetComponent<Text>();
		_factoryText = GameObject.Find("FactoryText").GetComponent<Text>();
		_woodCost = GameObject.Find("WoodCost").GetComponent<Text>();
		_stoneCost = GameObject.Find("StoneCost").GetComponent<Text>();
		_ironCost = GameObject.Find("IronCost").GetComponent<Text>();
		_coalCost = GameObject.Find("CoalCost").GetComponent<Text>();
		_activateButton = GameObject.Find("FactoryActivate").GetComponent<Button>();
		_factoryPrompt.SetActive(false);
		_ui = GameObject.Find("UI").GetComponent<WM_UI>();

	}
	public static void OpenUI(){
		CheckResources();
		_factoryName.text = _name;
		_factoryText.text = "+ " + _amount + " " + _manager._resourceNames[_type] + " Per Day";
		_buttonText.text = (_active) ? "Upgrade" : "Activate";
		_activateButton.interactable = _affordable;
		_woodCost.text = _activeFactory._costs[0].ToString();
		_stoneCost.text = _activeFactory._costs[1].ToString();
		_ironCost.text = _activeFactory._costs[2].ToString();
		_coalCost.text = _activeFactory._costs[3].ToString();
		_factoryPrompt.SetActive(true);
		Time.timeScale = 0f;
	}
	public void Cancel(){
		_factoryPrompt.SetActive(false);
		Time.timeScale = 1.0f;
	}
	public void Activate(){		
		if (!_active && _affordable){
			PayResources();	
			_activeFactory._active = true;
			_activeFactory._factoryLevel++;
			_activeFactory.SwitchMesh();
			_activeFactory.UpgradeCost();			
			_manager._factoryOuput[_type] += _amount;
			Time.timeScale = 1.0f;
			_factoryPrompt.SetActive(false);
			WM_UI.UpdateUI();
					
		}
		else if (_affordable){
			if (_activeFactory._factoryLevel < 3){
				PayResources();
				_activeFactory._factoryLevel++;
				_activeFactory.UpgradeFactory();
				_activeFactory.SwitchMesh();
				_factoryPrompt.SetActive(false);
				Time.timeScale = 1.0f;
			}			
		}
	}

	public static void Open(){
		OpenUI();
	}

	static void CheckResources(){
		int _checked = 0;
		_affordable = false;
		for (int i = 0; i < _activeFactory._costs.Count; i++){
			if (_manager._resources[i] >= _activeFactory._costs[i])	_checked++;
			_affordable = (_checked == 4);
		}
	}

	void PayResources(){
		for (int i = 0; i < _activeFactory._costs.Count; i++){
			_manager._resources[i] -= _activeFactory._costs[i];
		}
	}
}
