using UnityEngine;
using System.Collections;

public class DayCycle : MonoBehaviour {

	public DayTimer _dayTimer;
	public Light _mainLight;
	public Light _playerLight;
	public bool _day;

	// Use this for initialization
	void Start () {
		_mainLight = gameObject.GetComponent<Light>();
		_playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
		SetLightColor();
	}
	
	// Update is called once per frame
	void Update () {
		MoveAcross();	
	}

	void MoveAcross(){
		if (_day){
			transform.Rotate(3.1f*Time.deltaTime, 0, 0);
			if (_dayTimer._hours > 10 && _mainLight.intensity > 0){
				_mainLight.intensity -= Time.deltaTime* 0.2f;
			}
			else if (_mainLight.intensity < 1.0f){
				_mainLight.intensity += Time.deltaTime* 0.2f;
			}
			if (_dayTimer._hours >= 9 & _playerLight.intensity < 1.0f) _playerLight.intensity += 0.1f*Time.deltaTime;
			else if (_dayTimer._hours >= 0 && _playerLight.intensity > 0) _playerLight.intensity -= 0.1f*Time.deltaTime; 
		}
	}

	public void SetLightColor(){
		if (_dayTimer._hours == 0 || _dayTimer._hours == 12) _mainLight.intensity = 0;
		if (_dayTimer._hours >= 0 && _dayTimer._hours < 12){
			_day = true;
			_mainLight.enabled = true;
			_playerLight.enabled = !(_dayTimer._hours > 2 && _dayTimer._hours < 9);
		}
		else{
			_day = false;
			_mainLight.enabled = false;
			transform.rotation = Quaternion.identity;
		}
	}
}
