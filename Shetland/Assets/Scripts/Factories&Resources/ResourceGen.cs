using UnityEngine;

public class ResourceGen : MonoBehaviour {

	public int _type;
	public int _amount;
	public bool _taken;
	public GameObject _childPrefab;
	public GameObject _child;
	public ResourceGen _script;

	// Use this for initialization
	void Start () {
		SpawnChild();
		GenerateType();
		GenerateAmount();
		_script = gameObject.GetComponent<ResourceGen>();		
	}
	
	void GenerateType(){
		_type = Random.Range(0, 4);
	}
	void GenerateAmount(){
		switch(_type){
			case 0:
			_amount = Random.Range(2, 7);
			break;
			case 1:
			_amount = Random.Range(1, 5);
			break;
			case 2:
			_amount = Random.Range(1, 4);
			break;
			case 3:
			_amount = Random.Range(1, 3);
			break;
		}
	}

	public void AddResource(){
		switch(_type){
			case 0:
			_manager._resources[0] += _amount;
			break;
			case 1:
			_manager._resources[1] += _amount;
			break;
			case 2:
			_manager._resources[2] += _amount;
			break;
			case 3:
			_manager._resources[3] += _amount;
			break;
		}
		_taken = true;
	}

	public void Activate(){
		WM_UI._resourceScript = _script;
		WM_UI.ResourcePrompt();
		Destroy(_child);
	}

	void SpawnChild(){
		if (!_taken) _child = (GameObject) Instantiate (_childPrefab, transform.position, Quaternion.identity, transform);
	}
}
