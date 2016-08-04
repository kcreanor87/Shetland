using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour {

	public TownManager _townManager;
	public Text _buy0, _buy1, _buy2, _buy3;
	public Text _sell0, _sell1, _sell2, _sell3;
	public Text _stock0, _stock1, _stock2, _stock3;
	public Text _obols;

	void Awake(){
		_buy0 = GameObject.Find("WoodBP").GetComponent<Text>();
		_buy1 = GameObject.Find("StoneBP").GetComponent<Text>();
		_buy2 = GameObject.Find("IronBP").GetComponent<Text>();
		_buy3 = GameObject.Find("CoalBP").GetComponent<Text>();

		_sell0 = GameObject.Find("WoodSP").GetComponent<Text>();
		_sell1 = GameObject.Find("StoneSP").GetComponent<Text>();
		_sell2 = GameObject.Find("IronSP").GetComponent<Text>();
		_sell3 = GameObject.Find("CoalSP").GetComponent<Text>();

		_stock0 = GameObject.Find("WoodStock").GetComponent<Text>();
		_stock1 = GameObject.Find("StoneStock").GetComponent<Text>();
		_stock2 = GameObject.Find("IronStock").GetComponent<Text>();
		_stock3 = GameObject.Find("CoalStock").GetComponent<Text>();

		_obols = GameObject.Find("ObolsStock").GetComponent<Text>();
	}

	public void BuyResource(int resource){
		if (_manager._obols >= _townManager._buyPrices[resource]){
			_manager._resources[resource]++;
			_manager._obols -= _townManager._buyPrices[resource];
			UpdatePrices();
		}		
	}

	public void SellResource(int resource){
		if (_manager._resources[resource] > 0){
			_manager._resources[resource]--;
			_manager._obols += _townManager._sellPrices[resource];
			UpdatePrices();
		}		
	}

	public void UpdatePrices(){
		_buy0.text = _townManager._buyPrices[0].ToString();
		_buy1.text = _townManager._buyPrices[1].ToString();
		_buy2.text = _townManager._buyPrices[2].ToString();
		_buy3.text = _townManager._buyPrices[3].ToString();

		_sell0.text = _townManager._sellPrices[0].ToString();
		_sell1.text = _townManager._sellPrices[1].ToString();
		_sell2.text = _townManager._sellPrices[2].ToString();
		_sell3.text = _townManager._sellPrices[3].ToString();

		_obols.text = _manager._obols.ToString();

		_stock0.text = _manager._resources[0].ToString();
		_stock1.text = _manager._resources[1].ToString();
		_stock2.text = _manager._resources[2].ToString();
		_stock3.text = _manager._resources[3].ToString();
	}	
}
