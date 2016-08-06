using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DayTimer : MonoBehaviour {

	public int _hours;
	public int _days;
	public GameObject _clock;
	public int _rumourTimer;
	public List <GameObject> _towns = new List <GameObject>();
	public RumourGenerator _rumourScript;

	void Start(){
		_clock = GameObject.Find("Clock");
		StartCoroutine(Timer());
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		_rumourScript = GameObject.Find("TownCanvas").GetComponent<RumourGenerator>();

	}

	IEnumerator Timer(){
		yield return new WaitForSeconds(1.0f);
		_hours++;
		UpdateClock();
		if (_hours >= 24){
			_days++;
			_hours = 0;
			EndOfDay(true);					
		}
		else{
			StartCoroutine(Timer());
		}
		if (_rumourScript._rumourActive){
			_rumourTimer++;
			RumourTimer();
		}
	}
	void EndOfDay(bool restart){
		_manager._resources[0] += _manager._factoryOuput[0];
		_manager._resources[1] += _manager._factoryOuput[1];
		_manager._resources[2] += _manager._factoryOuput[2];
		_manager._resources[3] += _manager._factoryOuput[3];
		WM_UI.UpdateUI();
		ResetPrices();		
		if (restart) StartCoroutine(Timer());
	}
	void RumourTimer(){
		if (_rumourTimer >= 24){
			_rumourScript.ClearRumour();
			_rumourTimer = 0;
		}
	}

	void UpdateClock(){
		float rot = ((float)_hours/24.0f) * -360.0f;
		_clock.transform.eulerAngles = new Vector3(0,0,rot);
	}

	void ResetPrices(){
		for (int i = 0; i < _towns.Count; i++){
			var _script = _towns[i].GetComponent<TownManager>();
			_script.GeneratePrices();
		}
	}

	public void AdvanceTime(int amount){
		if (_hours + amount >= 24){
			var remainder = _hours + amount - 24;
			EndOfDay(false);
			_days++;
			_rumourTimer += amount;
			RumourTimer();
			_hours = remainder;
			UpdateClock();
		}
		else {
			_hours += amount;
		}
	}
}
