using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public int _obolReq;
	public Canvas _harbourCanvas;
	public int _cost = 1000;
	public int _fameRequired = 1000;
	public bool _partA, _partB, _partC, _partD;
	public bool _ticket, _built, _bought, _bragged;
	public int _winType;
	public bool _affordable;
	public GameObject _build, _buy, _brag, _board, _boughtGO, _braggedGO, _builtGO;
	public Button _partAbutton, _partBbutton, _partCbutton, _partDbutton;
	public GameObject _openScreen;
	public GameObject _buildScreen;

	void Start(){		
		_build = GameObject.Find("Build");
		_buy = GameObject.Find("Buy");
		_brag = GameObject.Find("Brag");
		_board = GameObject.Find("Board");
		_builtGO = GameObject.Find("Built");
		_boughtGO = GameObject.Find("Bought");
		_braggedGO = GameObject.Find("Bragged");
		_openScreen = GameObject.Find("OpeningScreen");
		_buildScreen = GameObject.Find("BuildScreen");
		_harbourCanvas = gameObject.GetComponent<Canvas>();
		_harbourCanvas.enabled = false;
	}

	public void OpenCanvas(){
		CalculateOptions();
		Time.timeScale = 0.0f;
		_harbourCanvas.enabled = true;
		PlayerControls_WM._inMenu = true;		
	}

	void CalculateOptions(){
		//Activate and deactivate GameObjects based on current stats
		CheckBuildStatus();
		CheckResources();
		_build.SetActive(!_built);
		_buy.SetActive(_manager._obols >= _cost && !_bought);
		_brag.SetActive(_manager._repute >= _fameRequired && !_bragged);
		_builtGO.SetActive(_built);
		_boughtGO.SetActive(_bought);
		_braggedGO.SetActive(_bragged);
		_board.SetActive(_ticket);
		_buildScreen.SetActive(false);
		WM_UI.UpdateUI();
	}

	public void CloseCanvas(){
		_harbourCanvas.enabled = false;
		Time.timeScale = 1.0f;
		PlayerControls_WM._inMenu = false;
	}

	public void Buy(){
		_manager._obols -= _cost;	
		_bought = true;
		_winType = 1;
		CalculateOptions();
	}

	public void Brag(){
		_bragged = true;
		_winType = 2;	
		CalculateOptions();
	}

	public void Build(){
		//Open Build canvas
		_buildScreen.SetActive(true);
		_openScreen.SetActive(false);
	}

	public void Back(){
		_openScreen.SetActive(true);
		_buildScreen.SetActive(false);
		CalculateOptions();
	}
	public void BuyPart(int part){
		switch (part){
			case 0:
			_partA = true;
			_partAbutton.interactable = false;
			_manager._resources[0] -= 1000;
			break;
			case 1:
			_partB = true;
			_partBbutton.interactable = false;
			_manager._resources[3] -= 1000;
			break;
			case 2:
			_partC = true;
			_partCbutton.interactable = false;
			_manager._resources[4] -= 1000;
			break;
			case 3:
			_partD = true;
			_partDbutton.interactable = false;
			_manager._resources[5] -= 1000;
			break;
		}
		WM_UI.UpdateUI();
		_built = (_partA && _partB && _partC && _partD);
	}

	void CheckBuildStatus(){
		_ticket = ((_partA && _partB && _partC && _partD) || _bought || _bragged);
	}

	void CheckResources(){
		_partAbutton.interactable = (_manager._resources[0] >= 1000 && !_partA);
		_partBbutton.interactable = (_manager._resources[3] >= 1000 && !_partB);
		_partCbutton.interactable = (_manager._resources[4] >= 1000 && !_partC);
		_partDbutton.interactable = (_manager._resources[5] >= 1000 && !_partD);
	}
}
