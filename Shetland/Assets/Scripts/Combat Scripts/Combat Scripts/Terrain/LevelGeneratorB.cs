using UnityEngine;
using System.Collections.Generic;

public class LevelGeneratorB: MonoBehaviour {

	public List<GameObject> _cornerPieces = new List<GameObject>();
	public List<GameObject> _edgePieces = new List<GameObject>();
	public List<GameObject> _centrePieces = new List<GameObject>();

	private List<GameObject> _builtPieces = new List<GameObject>();

	public GameObject _parent;
	private GameObject _pieceClone;

	private Vector3 _angle;
	public int _maxHeight;
	public int _maxWidth;
	private int _index;
	public int _tileSize;

	void Awake(){
		GenerateLevel();
	}

	void GenerateLevel(){
		for (int i = 0; i <= _maxHeight; i++){
			for (int j = 0; j <= _maxWidth; j++){
				MoveGenerator(i, j);
				if (i == 0){
					if (j == 0){
						GenerateTile(0, 0);
					}
					else if (j < _maxWidth){
						GenerateTile(1, 0);
					}
					else{
						GenerateTile(0, 90);
					}
				}
				else if (i < _maxHeight){
					if (j == 0){
						GenerateTile(1, 270);
					}
					else if (j < _maxWidth){
						GenerateTile(2, 0);
					}
					else{
						GenerateTile(1, 90);
					}
				}
				else{
					if (j == 0){
						GenerateTile(0, 270);
					}
					else if (j < _maxWidth){
						GenerateTile(1, 180);
					}
					else{
						GenerateTile(0, 180);
					}
				}
			}
		}
	}

	void GenerateTile(int type, int angle){

		switch(type){
			case 0:
				_index = Random.Range(0, _cornerPieces.Count);
				_angle = new Vector3(0, angle, 0);
				_pieceClone = (GameObject) Instantiate (_cornerPieces[_index], transform.position, transform.rotation);
				_pieceClone.transform.rotation = Quaternion.Euler(_angle);
				break;
			case 1:
				_index = Random.Range(0, _edgePieces.Count);
				_angle = new Vector3(0, angle, 0);
				_pieceClone = (GameObject) Instantiate (_edgePieces[_index], transform.position, transform.rotation);
				_pieceClone.transform.rotation = Quaternion.Euler(_angle);
				break;
			case 2:
				_index = Random.Range(0, _centrePieces.Count);
				var _randomAngle = Random.Range(0, 4) * 90;
				_angle = new Vector3(0, _randomAngle, 0);
				_pieceClone = (GameObject) Instantiate (_centrePieces[_index], transform.position, transform.rotation);
				_pieceClone.transform.rotation = Quaternion.Euler(_angle);
				break;
			default:
				print("Error - Generate Tile - B");
				break;
		}
		_builtPieces.Add(_pieceClone);
		_pieceClone.transform.SetParent(_parent.transform);
	}

	void MoveGenerator(int x, int y){
		int k = x * _tileSize;
		int l = y * _tileSize;
		var _newPos = new Vector3(k, 0, l);
		transform.position = _newPos;
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Space)){
			_maxHeight = Random.Range(2, 10);
			_maxWidth = Random.Range(2, 10);
			DeleteTiles();
			GenerateLevel();
		}
	}

	void DeleteTiles(){
		for (int i = 0; i < _builtPieces.Count; i++){
			Destroy(_builtPieces[i]);
		}
		_builtPieces.Clear();
	}
}
