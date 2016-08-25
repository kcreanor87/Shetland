using UnityEngine;
using UnityEngine.UI;

public class ActivateFactory : MonoBehaviour {

	public GameObject _factoryPrompt;
	public Text _factoryName;
	public Text _factoryText;
	public Text _buttonText;
	public Text _woodCost, _stoneCost, _ironCost, _coalCost;
	public string _name;
	public int _type;
	public int _amount;
	public bool _active;
	public bool _affordable;
	public Factories _activeFactory;
	public Button _activateButton;
	public GameObject _activateGO;
	public SaveGame _saveGame;
	public GameObject _mapToggle;
	public WM_UI _ui;

	// Use this for initialization
	void Start () {
		_saveGame = GameObject.Find("Loader").GetComponent<SaveGame>();
		_factoryPrompt = GameObject.Find("FactoryPrompt");
		_buttonText = GameObject.Find("ActivateText").GetComponent<Text>();
		_factoryName = GameObject.Find("FactoryName").GetComponent<Text>();
		_factoryText = GameObject.Find("FactoryText").GetComponent<Text>();
		_woodCost = GameObject.Find("WoodCost").GetComponent<Text>();
		_stoneCost = GameObject.Find("StoneCost").GetComponent<Text>();
		_ironCost = GameObject.Find("IronCost").GetComponent<Text>();
		_coalCost = GameObject.Find("CoalCost").GetComponent<Text>();
		_activateGO = GameObject.Find("FactoryActivate");
		_activateButton = GameObject.Find("FactoryActivate").GetComponent<Button>();
		_mapToggle = GameObject.Find("MapToggle");
		_factoryPrompt.SetActive(false);
		_ui = GameObject.Find("UI").GetComponent<WM_UI>();
		_active = false;

	}
	public void OpenUI(){
		_mapToggle.SetActive(false);		
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
		_saveGame.Save();
	}
	public void Cancel(){
		_mapToggle.SetActive(true);
		_factoryPrompt.SetActive(false);
		Time.timeScale = 1.0f;
	}
	public void Activate(){		
		if (!_active && _affordable){
			_activeFactory._factoryLevel++;
			PayResources();	
			_activeFactory._active = true;			
			_activeFactory.SwitchMesh();
			_activeFactory.UpgradeCost();			
			_manager._factoryOuput[_type] += _amount;
			_activeFactory.Names();
			Time.timeScale = 1.0f;
			_factoryPrompt.SetActive(false);
			_mapToggle.SetActive(true);
			WM_UI.UpdateUI();
					
		}
		else if (_affordable){
			if (_activeFactory._factoryLevel < 3){
				_activeFactory._factoryLevel++;
				PayResources();
				_activeFactory.UpgradeFactory();
				_activeFactory.SwitchMesh();
				_activeFactory.Names();
				_factoryPrompt.SetActive(false);
				_mapToggle.SetActive(true);
				Time.timeScale = 1.0f;
			}			
		}
	}

	public void Open(){
		OpenUI();
	}

	void CheckResources(){
		int _checked = 0;
		_affordable = false;
		for (int i = 0; i < _activeFactory._costs.Count; i++){
			if (_manager._resources[i] >= _activeFactory._costs[i])	_checked++;
			_affordable = (_checked == 4);
		}
		_activateGO.SetActive((_activeFactory._factoryLevel < 3));
	}

	void PayResources(){
		for (int i = 0; i < _activeFactory._costs.Count; i++){
			_manager._resources[i] -= _activeFactory._costs[i];
		}
	}
}
