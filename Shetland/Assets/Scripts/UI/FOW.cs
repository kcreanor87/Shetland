using UnityEngine;

public class FOW : MonoBehaviour {

	public bool _active;
	public MeshRenderer _mesh;

	void Start(){
		_mesh = gameObject.GetComponent<MeshRenderer>();
		if (_active) Seen();
	}

	public void Seen(){
		_mesh.enabled = false;
		gameObject.tag = "Untagged";
	}
}
