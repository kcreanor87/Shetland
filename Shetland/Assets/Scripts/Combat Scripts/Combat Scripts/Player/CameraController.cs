//Controls the movement of the camera during combat

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	//Store Transform of object to follow
	public Transform _target;
	//Variables to store mouse status
	public Vector3 _mousePos;
	public Vector3 _originPos;
	public Vector3 _clickedPos;
	public Vector3 _playerTarget;
	//Variables for player settings
	public float _scrollSpeed = 25.0f;
	public float _clickTimer = 0.3f;
	//Store GO and scripts accessed later in script
	public CombatGenerator _generator;
	public TurnManager _turnManager;
	public PlayerTurn _player;
	public GameObject _midPoint;
	//Bools to store camera and mouse statuses
	public bool _cameraActive;
    public bool _shooting;

    //Initialize
    void Start()
    {
		_turnManager = GameObject.Find("_combatMan").GetComponent<TurnManager>();
		_generator = GameObject.Find("_combatMan").GetComponent<CombatGenerator>();
		_player = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_target = GameObject.Find("Player").GetComponent<Transform>();
		_midPoint = GameObject.Find("MidPoint");
		_originPos = transform.position;
	} 

	void Update () {
		//Check status
		ActivateCamera();
		//Follow active GO if player isn't active
		if (_cameraActive || _turnManager._waitingForSeconds){
			FollowGO();
			if (!_player._active && !_shooting){
				if (Camera.main.fieldOfView < 50.0f){
					Camera.main.fieldOfView += (Time.deltaTime*30);
				}
				else if (Camera.main.fieldOfView > 50.0f){
					Camera.main.fieldOfView = 50.0f;
				}				
			}
			else if (_shooting){
				CheckFOV(_player.gameObject.transform.position, _target.position);
			}
		}
		else if (_shooting){
			FollowGO();
			CheckFOV(_player.gameObject.transform.position, _playerTarget);
		}
		//Otherwise, call the camera controlling function
		else{
			//Enable camera zooming 
			ZoomFunction();
			//Enable Camera panning
			ScrollingCam();
			//if (_shooting) FollowGO();
		}		
	}
	//Function to check if the player is currently active
	void ActivateCamera(){
		_cameraActive = (!_player._active || _player._moving);
	}
	//Functon to keep the camera focused over the active GO when moving
	void FollowGO(){
		_target = _shooting ? _midPoint.transform : _turnManager._activeCombatant._go.transform;
		transform.position = Vector3.MoveTowards(transform.position, _target.position, _scrollSpeed*Time.deltaTime);
	}
	//Function for controlling camera movement via mouse mouve/clicks
	void ScrollingCam(){
		//When the middle mouse-button is first pressed, store the current mouse position
		if (Input.GetMouseButtonDown(2)){
			_mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			_originPos = transform.position;
		}		
		//When the middle mouse button is held and moved, move the camera in the same direction to
		//the mouse movement to simulate drag.
		if (Input.GetMouseButton(2)){
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - _mousePos;
			pos.z = pos.y;
			pos.y = 0;
			transform.position = _originPos + (-pos*_scrollSpeed);
		}
		//Script for camera scrolling at edge of screen, disabled during development
		/*else{
			if(Input.mousePosition.x <= 0){
				transform.Translate(Vector3.left*Time.deltaTime*_scrollSpeed);
			}
			else if (Input.mousePosition.x >= Screen.width){
				transform.Translate(Vector3.right*Time.deltaTime*_scrollSpeed);
			}
			if(Input.mousePosition.y <= 0){
				transform.Translate(Vector3.back*Time.deltaTime*_scrollSpeed);
			}
			else if(Input.mousePosition.y >= Screen.height){
				transform.Translate(Vector3.forward*Time.deltaTime*_scrollSpeed);
			}
		}*/
	}
	//Function controlling camera zooming
	void ZoomFunction(){
		//Store the input from the scrollwheel
		float d = Input.GetAxis ("Mouse ScrollWheel");
		//If the scrollwheel input is positive, zoom in until threshold reached
		if (d > 0f && Camera.main.fieldOfView > 20){
			Camera.main.fieldOfView -= 5;
		}
		//Repeat for zoom out/input negative
		else if (d < 0f && Camera.main.fieldOfView < 60){
			Camera.main.fieldOfView += 5;
		}
	}

	void CheckFOV(Vector3 subject, Vector3 target){
		var TargetA = subject - Camera.main.transform.position;
		var angleA = Vector3.Angle(TargetA, Camera.main.transform.forward);
		var TargetB = target - Camera.main.transform.position;
		var angleB = Vector3.Angle(TargetB, Camera.main.transform.forward);
		if (angleA >= Camera.main.fieldOfView/2 || angleB >= Camera.main.fieldOfView/2){
			Camera.main.fieldOfView += Time.deltaTime*25;
		} 
	}

	public void EnableShotCam(){ 
		_shooting = true;		
		StartCoroutine(ShotCam());
	}

	IEnumerator ShotCam(){
		yield return new WaitForSeconds(2.0f);
		_shooting = false;		
	}
}
