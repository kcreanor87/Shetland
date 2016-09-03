using UnityEngine;
using UnityEngine.UI;

public class AdditionalResources : MonoBehaviour {

	public Button _button;
	public Text _buttonText;
	public Text _ironText;
	public Text _coalText;
	public int _type;

	void Start(){

		_button = GameObject.Find("CreateResource").GetComponentInChildren<Button>();
		_buttonText = _button.transform.GetComponentInChildren<Text>();
		_ironText = GameObject.Find("AvailableIron").GetComponent<Text>();
		_coalText = GameObject.Find("AvailableCoal").GetComponent<Text>();
	}

	public void CheckResources(int type){
		_type = type;
		_button.interactable = (type > 0) ? (_manager._resources[3] > 9) : (_manager._resources[2] > 3 && _manager._resources[3] > 2);
		if (type > 0){
			_ironText.enabled = false;
			_coalText.text = "Coal: " + _manager._resources[3] + "(-10)";
			var pos = new Vector3(45f, _coalText.transform.localPosition.y, _coalText.transform.localPosition.z);
			_coalText.transform.localPosition = pos;
			_buttonText.text = "Diamonds +1";
		}
		else{
			_ironText.enabled = true;
			_ironText.text = "Iron: " + _manager._resources[2] + "(-4)";
			_coalText.text = "Coal: " + _manager._resources[3] + "(-2)";
			var pos = new Vector3(100, _coalText.transform.localPosition.y, _coalText.transform.localPosition.z);
			_coalText.transform.localPosition = pos;
			_buttonText.text = "Steel +1";
		}		
	}
	
	public void AddResource(){
		if (_type > 0){
			_manager._resources[3] -= 10;
			_manager._resources[5] += 1;
		}
		else{
			_manager._resources[2] -= 4;
			_manager._resources[3] -= 2;
			_manager._resources[4] += 1;
		}
		CheckResources(_type);
		WM_UI.UpdateUI();
	}
}
