using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DayTimer : MonoBehaviour {

	public int _hours;
	public int _days;
	public Text _dayCounter;
	public GameObject _clock;
	public int _rumourTimer;
	public List <GameObject> _towns = new List <GameObject>();
	public RumourGenerator _rumourScript;
	public DayCycle _dayCycle;
	public SaveGame _saveGame;
	public PlayerSight _playerSight;

	void Start(){
		_dayCycle = GameObject.Find("MainLight").GetComponent<DayCycle>();
		_clock = GameObject.Find("Clock");
		_dayCounter = transform.FindChild("DayCounter").GetComponent<Text>();
		StartCoroutine(Timer());
		_towns.AddRange(GameObject.FindGameObjectsWithTag("Town"));
		_rumourScript = GameObject.Find("TownCanvas").GetComponent<RumourGenerator>();		
		_playerSight = GameObject.Find("SightRadius").GetComponent<PlayerSight>();
		UpdateClock();
		Time.timeScale =1.0f;
		_saveGame = GameObject.Find("Loader").GetComponent<SaveGame>();
	}

	IEnumerator Timer(){
		yield return new WaitForSeconds(5.0f);
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
		_dayCycle.SetLightColor();
	}
	void EndOfDay(bool restart){
		_manager._resources[0] += _manager._factoryOuput[0];
		_manager._resources[1] += _manager._factoryOuput[1];
		_manager._resources[2] += _manager._factoryOuput[2];
		_manager._resources[3] += _manager._factoryOuput[3];
		WM_UI.UpdateUI();
		ResetPrices();
		_saveGame.Save();	
		if (restart) StartCoroutine(Timer());		
	}
	void RumourTimer(){
		if (_rumourTimer >= 24){
			_rumourScript.ClearRumour();
			_rumourTimer = 0;
		}
		else{
			_rumourScript._rumourTimeTxt.text = (24 - _rumourTimer).ToString();
		}
	}

	void UpdateClock(){
		float rot = ((float)_hours/24.0f) * -360.0f;
		_clock.transform.eulerAngles = new Vector3(0,0,rot);
		_dayCounter.text = _days.ToString();
		_playerSight.ChangeSightSize();
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
			_days++;						
			_rumourTimer += amount;
			RumourTimer();
			_hours = remainder;
			UpdateClock();
			EndOfDay(false);
		}
		else {
			_hours += amount;
			_rumourTimer += amount;
			RumourTimer();
			UpdateClock();
		}
		_dayCycle.AdvanceTime();
	}

	public void Sleep(){
		_days++;
		int amount = 24 - _hours;					
		_rumourTimer += amount;
		RumourTimer();
		_hours = 0;
		UpdateClock();
		EndOfDay(false);
		_dayCycle.AdvanceTime();
	}
}
