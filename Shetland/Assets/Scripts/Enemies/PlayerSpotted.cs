using UnityEngine;

public class PlayerSpotted : MonoBehaviour {

	public PlayerSpotted _thisScript;
	public EnemyWM _parentScript;
	public bool _chasing;
	public SaveGame _saveGame;

	void Start(){
		_thisScript = gameObject.GetComponent<PlayerSpotted>();
		_parentScript = transform.GetComponentInParent<EnemyWM>();
		_saveGame = GameObject.Find("Loader").GetComponent<SaveGame>();
	}

	void OnWillRenderObject(){
		if (Camera.current.name == "Main Camera" && !_chasing){
			Seen();
		}
	}

	void Seen(){
		_saveGame.Save();
		_chasing = true;
		_parentScript.ChasePlayer();		
	}
}
