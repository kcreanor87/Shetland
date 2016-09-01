using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatOver : MonoBehaviour{

	public GameObject _managerGO;
	public Canvas _ui;
	public Canvas _endCanvas;
	public Image _endImage;
	public Sprite _victory;
	public Sprite _defeat;
	public bool _playerWin;

	void Start(){
		_managerGO = GameObject.Find("_manager");
		_ui = GameObject.Find("UI").GetComponent<Canvas>();
		_endCanvas = gameObject.GetComponent<Canvas>();
		_endImage = GameObject.Find("Victory/Defeat").GetComponent<Image>();
		_endCanvas.enabled = false;
	}

	public void EnableEndCanvas(){
		_ui.enabled = false;
		_endCanvas.enabled = true;
		_endImage.sprite = (_playerWin) ? _victory : _defeat;
	}

	public void Restart(){
		Destroy(_managerGO);
		SceneManager.LoadScene(0);
	}
}
