using UnityEngine;
using System.Collections;

public class MoveIndicator : MonoBehaviour {

	public LineRenderer _line;
	public Material _moveMat;
	public Texture2D _moveTex;
	public Texture2D _noMoveTex;
	public float _distance;	
	public GameObject _moveInd;
	public PlayerTurn _pT;

	// Use this for initialization
	void Start () {		
		_line = gameObject.GetComponent<LineRenderer>();
		_moveInd = GameObject.Find("Indicator");		
		_moveMat =_moveInd.GetComponent<Renderer>().material;
		_pT = gameObject.GetComponent<PlayerTurn>();		
	}

	public void DrawPath(Vector3 target, NavMeshPath path){
		StartCoroutine(Draw(target, path));
	}

	//Function for displaying the intended path via Line Renderer
	IEnumerator Draw (Vector3 _target, NavMeshPath _path){
		yield return new WaitForEndOfFrame();
		_line.enabled = true;
		//Set distance to 0, then calculate distance between corners on the path and combine
		_distance = 0f;			
		_line.SetVertexCount(_path.corners.Length);
		_line.SetPosition(0, transform.position);
		for (int i = 1; i < _path.corners.Length; i++){
			_line.SetPosition(i, _path.corners[i]);
			_distance += Vector3.Distance(_path.corners[i - 1], _path.corners[i]);
		}
		_line.material.SetTextureScale("_MainTex", new Vector2(_distance*5, 1.0f));
		_pT._distance = _distance;
	}

	public void ActiveStatus(bool lineActive, bool indicatorActive){
		_line.enabled = lineActive;
		_moveInd.SetActive(indicatorActive);
		if (!_line.enabled) _moveMat.mainTexture = _noMoveTex;
		else{
			_moveMat.mainTexture = _moveTex;
			_line.material.color = Color.white;
		}
		if (_moveInd.activeInHierarchy) _moveInd.transform.position = _pT._targetPos;
	}
}
