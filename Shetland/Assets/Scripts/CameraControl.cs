using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Transform _player;
	public float _speed = 30.0f;

	void Start(){
		_player = GameObject.Find("Player").GetComponent<Transform>();
		transform.position = _player.position;
	}

	void Update(){
		transform.position = Vector3.MoveTowards(transform.position, _player.position, _speed*Time.deltaTime);
	}
}
