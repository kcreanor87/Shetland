using UnityEngine;

public class PlayerSpotted : MonoBehaviour {

	public PlayerSpotted _thisScript;
	public EnemyWM _parentScript;

	void Start(){
		_thisScript = gameObject.GetComponent<PlayerSpotted>();
		_parentScript = transform.GetComponentInParent<EnemyWM>();
	}

	void OnWillRenderObject(){
		if (Camera.current.name == "Main Camera"){
			Seen();
		}
	}

	void Seen(){
		_parentScript.ChasePlayer();
		_thisScript.enabled = false;
	}
}
