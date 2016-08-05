using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Caravan : MonoBehaviour {

	public List<GameObject> _towns = new List<GameObject>();
	public List<TownManager> _townScripts = new List<TownManager>();
	public List <Button> _buttons = new List < Button>();
	public Transform _caravanParent;
	public Button _buttonPrefab;
	public Vector3 _destination;
	public float _distance;
	public int _cost;
	public TownCanvas _townCanvas;
	public Transform _player;

	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player").GetComponent<Transform>();
		_townCanvas = gameObject.GetComponent<TownCanvas>();
		_caravanParent = GameObject.Find("Caravan").GetComponent<Transform>();
		DetectAllTowns();		
	}

	public void Open(){
		GetActiveTowns();
	}

	void DetectAllTowns(){
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		for (int i = 0; i < _towns.Count; i++){
			_townScripts.Add(_towns[i].GetComponent<TownManager>());
		}
	}
	public void GetActiveTowns(){
		for (int x = 0; x < _buttons.Count; x++){
			Destroy(_buttons[x].gameObject);
		}
		_buttons.Clear();
		for (int i = 0; i < _townScripts.Count; i++){
			if (_townScripts[i]._visited && _townScripts[i]._name != _townCanvas._townManager._name){	
				var dist = Vector3.Distance(_player.position, _townScripts[i].gameObject.transform.position);
				var time = Mathf.FloorToInt(dist/20);
				_cost = time*10;	
				var button = (Button) Instantiate(_buttonPrefab, transform.position, Quaternion.identity);
				button.interactable = (_manager._obols >= _cost);
				var buttonScript = button.GetComponent<TravelButton>();
				buttonScript._destination = _townScripts[i].transform.FindChild("Entrance").GetComponent<Transform>().position;
				buttonScript._cost = _cost;
				button.transform.SetParent(_caravanParent);
				var buttonChild = button.transform.FindChild("Text");
				var buttonText = buttonChild.GetComponent<Text>();
				buttonText.text = _townScripts[i]._name + " (" + _cost + "o, " + time + "h)";
				_buttons.Add(button);
			}
		}
		for (int j = 0; j <_buttons.Count; j++){
			var pos = new Vector3(0, (100 - j*40), 0);
			_buttons[j].transform.localPosition = pos;
		}
	}
}
