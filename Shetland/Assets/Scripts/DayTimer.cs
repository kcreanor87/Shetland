using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DayTimer : MonoBehaviour {

	public int _hours;
	public int _days;
	public GameObject _clock;
	public List <GameObject> _towns = new List <GameObject>();

	void Start(){
		_clock = GameObject.Find("Clock");
		StartCoroutine(Timer());
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));

	}

	IEnumerator Timer(){
		yield return new WaitForSeconds(1.0f);
		_hours++;
		UpdateClock();
		if (_hours >= 24){
			_days++;
			_hours = 0;
			EndOfDay();						
		}
		else{
			StartCoroutine(Timer());
		}
	}
	void EndOfDay(){
		_manager._resources[0] += _manager._factoryOuput[0];
		_manager._resources[1] += _manager._factoryOuput[1];
		_manager._resources[2] += _manager._factoryOuput[2];
		_manager._resources[3] += _manager._factoryOuput[3];
		WM_UI.UpdateUI();
		ResetPrices();
		StartCoroutine(Timer());
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
}
