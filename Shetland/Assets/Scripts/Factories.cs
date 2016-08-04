using UnityEngine;
using System.Collections.Generic;

public class Factories : MonoBehaviour {

	public string _name;
	public int _type;
	public int _perDay;
	public Factories _script;
	public List <int> _costs = new List<int>();
	public int _factoryLevel;

	public bool _active;
	public List <GameObject> _meshes = new List <GameObject>();

	void Start(){
		_script = gameObject.GetComponent<Factories>();
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
			_name = "Sawmill";
			_perDay = 3;
			break;
			case 1:
			_name = "Quarry";
			_perDay = 2;
			break;
			case 2:
			_name = "Iron Mine";
			_perDay = 1;
			break;
			case 3:
			_name = "Coal Mine";
			_perDay = 1;
			break;
		}
	}
	public void EnableFactory(){
		ActivateFactory._active = _active;
		ActivateFactory._activeFactory = _script;
		ActivateFactory._name = _name;
		ActivateFactory._type = _type;
		ActivateFactory._amount = _perDay;
		ActivateFactory.Open();
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
}
