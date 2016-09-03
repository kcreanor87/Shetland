using UnityEngine;
using System.Collections.Generic;

public class Factories : MonoBehaviour {

	public string _name;
	public int _type;
	public int _perDay;
	public Factories _script;
	public List <int> _costs = new List<int>();
	public int _factoryLevel;
	public ActivateFactory _activateFactory;
	public bool _active;
	public bool _seen;
	public List <GameObject> _meshes = new List <GameObject>();
	public Transform _player;

	void Awake(){
		_script = gameObject.GetComponent<Factories>();
		_activateFactory = GameObject.Find("Player").GetComponent<ActivateFactory>();
		_player	= GameObject.Find("Player").GetComponent<Transform>();
		CheckStartDistance();
		ValuePerDay();
		PopulateMeshList();
		PopulateCostList();
		SwitchMesh();
		//Calculate upgrade cost
		UpgradeCost();		
	}

	void ValuePerDay(){
		switch(_type){
			case 0:
			_name = "Potential Sawmill";
			_perDay = 3;
			break;
			case 1:
			_name = "Potential Quarry";
			_perDay = 2;
			break;
			case 2:
			_name = "Iron Source";
			_perDay = 1;
			break;
			case 3:
			_name = "Coal Source";
			_perDay = 1;
			break;
		}
	}
	public void EnableFactory(){
		_activateFactory._active = _active;
		_activateFactory._activeFactory = _script;
		_activateFactory._name = _name;
		_activateFactory._type = _type;
		_activateFactory._amount = _perDay;
		_activateFactory.Open();
	}

	public void UpgradeFactory(){		
		_manager._factoryOuput[_type] += _perDay;
		_perDay *= 2;		
		WM_UI.UpdateUI();
		UpgradeCost();
	}

	public void SwitchMesh(){
		for (int i = 0; i < _meshes.Count; i++){
			_meshes[i].SetActive(i == _factoryLevel);
		}
	}

	void PopulateMeshList(){
		_meshes.Add(transform.FindChild("Lvl_0").gameObject);
		_meshes.Add(transform.FindChild("Lvl_1").gameObject);
		_meshes.Add(transform.FindChild("Lvl_2").gameObject);
		_meshes.Add(transform.FindChild("Lvl_3").gameObject);
	}

	void PopulateCostList(){
		_costs.Add(0);
		_costs.Add(0);
		_costs.Add(0);
		_costs.Add(0);
	}

	public void UpgradeCost(){
		var x = _type + 2*_factoryLevel;
		UpgradeCases(x);
	}

	public void UpgradeCases(int tier){
		switch(tier){
			case 0:
			_costs[0] = 1;
			_costs[1] = 0;
			_costs[2] = 0;
			_costs[3] = 0;
			break;
			case 1:
			_costs[0] = 3;
			_costs[1] = 0;
			_costs[2] = 0;
			_costs[3] = 0;
			break;
			case 2:
			_costs[0] = 6;
			_costs[1] = 2;
			_costs[2] = 0;
			_costs[3] = 0;
			break;
			case 3:
			_costs[0] = 12;
			_costs[1] = 4;
			_costs[2] = 2;
			_costs[3] = 0;
			break;
			case 4:
			_costs[0] = 24;
			_costs[1] = 8;
			_costs[2] = 4;
			_costs[3] = 2;
			break;
			case 5:
			_costs[0] = 36;
			_costs[1] = 16;
			_costs[2] = 8;
			_costs[3] = 4;
			break;
			case 6:
			_costs[0] = 48;
			_costs[1] = 32;
			_costs[2] = 16;
			_costs[3] = 8;
			break;
			case 7:
			_costs[0] = 48;
			_costs[1] = 32;
			_costs[2] = 16;
			_costs[3] = 16;
			break;
		}
	}
	public void Names(){
		switch (_type){
			case 0:
			_name = "Sawmill Lvl " + _factoryLevel;
			break;
			case 1:
			_name = "Quarry Lvl " + _factoryLevel;
			break;
			case 2:
			_name = "Iron Mine Lvl " + _factoryLevel;
			break;
			case 3:
			_name = "Coal Mine Lvl " + _factoryLevel;
			break;
		}
	}

	void OnTriggerEnter(Collider other){
		_seen |= (other.gameObject.tag == "Player");
	}

	void CheckStartDistance(){
		_seen |= (Vector3.Distance(_player.position, transform.position) < 10.0f);
	}
}
