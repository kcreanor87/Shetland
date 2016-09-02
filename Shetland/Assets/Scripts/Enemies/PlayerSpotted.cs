using UnityEngine;

public class PlayerSpotted : MonoBehaviour {

	public PlayerSpotted _thisScript;
	public EnemyWM _parentScript;
	public bool _chasing;

	void Start(){
		_thisScript = gameObject.GetComponent<PlayerSpotted>();
		_parentScript = transform.GetComponentInParent<EnemyWM>();
	}

	void OnWillRenderObject(){
		if (Camera.current.name == "Main Camera" && !_chasing){
			Seen();
		}
	}

	void Seen(){
		_chasing = true;
		_parentScript.ChasePlayer();		
	}
}
