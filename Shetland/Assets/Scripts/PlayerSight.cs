using UnityEngine;

public class PlayerSight : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "FOW"){
			var script = col.gameObject.GetComponent<FOW>();
			script.Seen();
		}
	}
}
