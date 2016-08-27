using UnityEngine;

public class CombatTrigger : MonoBehaviour {

	public EnemyWM _parentScript;

	void Start(){
		_parentScript = transform.GetComponentInParent<EnemyWM>();
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player"){
			//Start combat
			_parentScript.AtPlayer();
		}
	}
}
