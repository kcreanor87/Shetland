using UnityEngine;

public class CombatTrigger : MonoBehaviour {

	public EnemyWM _parentScript;
	public SaveGame _saveGame;

	void Start(){
		_parentScript = transform.GetComponentInParent<EnemyWM>();
		_saveGame = GameObject.Find("Loader").GetComponent<SaveGame>();
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player"){
			//Start combat
			_saveGame.Save();
			_parentScript.AtPlayer();
		}
	}
}
