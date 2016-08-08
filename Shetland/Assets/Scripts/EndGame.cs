using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public int _obolReq;
	public Canvas _harbourCanvas;

	void Start(){
		_harbourCanvas = GameObject.Find("HarbourCanvas").GetComponent<Canvas>();
		_harbourCanvas.enabled = false;
	}

	public void OpenCanvas(){
		_harbourCanvas.enabled = true;
	}

}
