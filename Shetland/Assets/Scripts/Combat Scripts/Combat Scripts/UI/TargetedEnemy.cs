using UnityEngine;
using UnityEngine.UI;

public class TargetedEnemy : MonoBehaviour {

	public Sprite _targetLeft;
	public Sprite _targetRight;
	public string _targetText;

	public GameObject _targetGO;
	public RectTransform _targetRect;
	public Vector3 _target;
	public Image _targetImage;
	public Text _targetTextRight;
	public Text _targetTextLeft;

	// Use this for initialization
	void Start () {

		_targetGO = GameObject.Find("TargetedEnemy");
		_targetRect = _targetGO.GetComponent<RectTransform>();
		_targetImage = _targetGO.GetComponent<Image>();
		_targetTextLeft = _targetGO.transform.FindChild("TargetTextLeft").GetComponent<Text>();
		_targetTextRight = _targetGO.transform.FindChild("TargetTextRight").GetComponent<Text>();	
		DisableTarget();
	}
	
	public void EnableTarget(){
		_targetGO.SetActive(true);
		var _targetHead = new Vector3(_target.x, 2.0f, _target.z);
		var _pos = Camera.main.WorldToScreenPoint(_targetHead);
		_targetRect.position = _pos;
		if (Input.mousePosition.x >= Screen.width/2){
			_targetImage.sprite = _targetLeft;
			_targetTextLeft.enabled = true;
			_targetTextRight.enabled = false;
			UpdateText();
		}
		else{
			_targetImage.sprite = _targetRight;
			_targetTextLeft.enabled = false;
			_targetTextRight.enabled = true;
			UpdateText();
		}
	}

	public void DisableTarget(){
		_targetGO.SetActive(false);
	}

	void UpdateText(){
			_targetTextLeft.text = _targetText;
			_targetTextRight.text = _targetText;
	}
}
