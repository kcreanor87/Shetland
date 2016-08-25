using UnityEngine;
using System.Collections;

public class DetectCollisions : MonoBehaviour {

	public GameObject _parent;

	void Start(){
		_parent = transform.parent.gameObject;
	}

	void OnTriggerEnter(Collider other){
		if (_parent.tag == "Town"){
			if (other.tag == "Player"){
				var script = _parent.GetComponent<TownManager>();
				script._seen = true;
			}
		}
		else if (_parent.tag == "Factory"){
			if (other.tag == "Parent"){
				var script = _parent.GetComponent<Factories>();
				script._seen = true;
			}
		}		
	}
}
