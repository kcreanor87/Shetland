using UnityEngine;
using System.Collections.Generic;

public class TownManager : MonoBehaviour {

	public string _name;
	public bool _visited;
	public List <float> _basePrice = new List <float>();
	public List <int> _buyPrices = new List <int>();
	public List <int> _sellPrices = new List <int>();
	public List <bool> _activeBuildings = new List<bool>();
	public float _marketBuyMod = 1.3f;
	public float _marketSellMod = 0.7f;

	public float _rumourMod = 1.0f;
	public int _rumourType;

	// Use this for initialization
	void Start () {
		SpawnPrices();
		PopulateBuildings();
	}

	void SpawnPrices(){
		_basePrice.Add(Random.Range(5.0f, 15.0f));
		_basePrice.Add(Random.Range(10.0f, 20.0f));
		_basePrice.Add(Random.Range(25.0f, 40.0f));
		_basePrice.Add(Random.Range(25.0f, 40.0f));
		_basePrice.Add(Random.Range(175.0f, 265.0f));
		_basePrice.Add(Random.Range(275.0f, 440.0f));

		for (int i = 0; i < _basePrice.Count; i++){
			_buyPrices.Add(Mathf.FloorToInt(_basePrice[i]*_marketBuyMod));
			_sellPrices.Add(Mathf.FloorToInt(_basePrice[i]*_marketSellMod));
		}
	}
	
	public void GeneratePrices(){
		_basePrice[0] = Random.Range(5.0f, 15.0f);
		_basePrice[1] = Random.Range(10.0f, 20.0f);
		_basePrice[2] = Random.Range(25.0f, 40.0f);
		_basePrice[3] = Random.Range(25.0f, 40.0f);
		_basePrice[4] = Random.Range(175.0f, 265.0f);
		_basePrice[5] = Random.Range(275.0f, 440.0f);

		_basePrice[_rumourType] *= _rumourMod;

		UpdatePrices();
	}

	public void UpdatePrices(){
		for (int i = 0; i < _basePrice.Count; i++){
			_buyPrices[i] = Mathf.FloorToInt(_basePrice[i] * _marketBuyMod);
			_sellPrices[i] = Mathf.FloorToInt(_basePrice[i] * _marketSellMod);
		}
	}

	void PopulateBuildings(){
		for (int i = 0; i < 14; i++){
			_activeBuildings.Add(false);
		}
		_activeBuildings[0] = true;
		_activeBuildings[1] = true;
 	}
}
