using UnityEngine;
using System.Collections.Generic;

public class TownManager : MonoBehaviour {

	public string _name;
	public List <float> _basePrice = new List <float>();
	public List <int> _buyPrices = new List <int>();
	public List <int> _sellPrices = new List <int>();

	// Use this for initialization
	void Start () {
		SpawnPrices();
	}

	void SpawnPrices(){
		_basePrice.Add(Random.Range(5.0f, 15.0f));
		_basePrice.Add(Random.Range(10.0f, 20.0f));
		_basePrice.Add(Random.Range(25.0f, 40.0f));
		_basePrice.Add(Random.Range(25.0f, 40.0f));

		_buyPrices.Add(Mathf.FloorToInt(_basePrice[0] * _manager._buyModifier));
		_buyPrices.Add(Mathf.FloorToInt(_basePrice[1] * _manager._buyModifier));
		_buyPrices.Add(Mathf.FloorToInt(_basePrice[2] * _manager._buyModifier));
		_buyPrices.Add(Mathf.FloorToInt(_basePrice[3] * _manager._buyModifier));

		_sellPrices.Add(Mathf.FloorToInt(_basePrice[0] * _manager._sellModifier));
		_sellPrices.Add(Mathf.FloorToInt(_basePrice[1] * _manager._sellModifier));
		_sellPrices.Add(Mathf.FloorToInt(_basePrice[2] * _manager._sellModifier));
		_sellPrices.Add(Mathf.FloorToInt(_basePrice[3] * _manager._sellModifier));
	}
	
	public void GeneratePrices(){
		_basePrice[0] = Random.Range(5.0f, 15.0f);
		_basePrice[1] = Random.Range(10.0f, 20.0f);
		_basePrice[2] = Random.Range(25.0f, 40.0f);
		_basePrice[3] = Random.Range(25.0f, 40.0f);

		_buyPrices[0] = Mathf.FloorToInt(_basePrice[0] * _manager._buyModifier);
		_buyPrices[1] = Mathf.FloorToInt(_basePrice[1] * _manager._buyModifier);
		_buyPrices[2] = Mathf.FloorToInt(_basePrice[2] * _manager._buyModifier);
		_buyPrices[3] = Mathf.FloorToInt(_basePrice[3] * _manager._buyModifier);

		_sellPrices[0] = Mathf.FloorToInt(_basePrice[0] * _manager._sellModifier);
		_sellPrices[1] = Mathf.FloorToInt(_basePrice[1] * _manager._sellModifier);
		_sellPrices[2] = Mathf.FloorToInt(_basePrice[2] * _manager._sellModifier);
		_sellPrices[3] = Mathf.FloorToInt(_basePrice[3] * _manager._sellModifier);
	}
}
