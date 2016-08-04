using UnityEngine;
using UnityEngine.UI;

public class TownCanvas : MonoBehaviour {

	public TownManager _townManager;
	public Market _market;
	public Canvas _canvas;
	public GameObject _welcomeGO;
	public GameObject _marketGO;
	public Canvas _ui;

	void Start(){
		_canvas = gameObject.GetComponent<Canvas>();
		_market = gameObject.GetComponent<Market>();
		_ui = GameObject.Find("UI").GetComponent<Canvas>();
		_welcomeGO = GameObject.Find("Welcome");
		_marketGO = GameObject.Find("Market");
		_canvas.enabled = false;
		CloseMarket();
	}

	public void OpenCanvas(){
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

	public void OpenMarket(){
		_marketGO.SetActive(true);
		_welcomeGO.SetActive(false);

	}

	public void CloseMarket(){
		_marketGO.SetActive(false);
		_welcomeGO.SetActive(true);
	}
}
