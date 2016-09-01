using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

	public Transform _target;
	public Text _text;
	public int _damage;
	public float _timer = 1.5f;

	public bool _active;

	public CalculateDamage _calcDam;

	// Use this for initialization
	void Awake() {
		_calcDam = GameObject.Find("_CombatManager").GetComponent<CalculateDamage>();
		_text = gameObject.GetComponent<Text>();
		_text.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		IsActive();
	}

	public void EnableOnScreen(){
		_active = true;
		var _pos = new Vector3(_target.position.x, 4.5f, _target.position.z);
		transform.position = (Camera.main.WorldToScreenPoint(_pos));
		_damage = _calcDam._totalDamage;
		if (_damage > 0){
			_text.text = "" + _damage;
		}
		else if (_damage < 0){
			_text.text = "Miss";
		}
		else{
			_text.text = "0";
		}
		_timer = 1.5f;
	}

	void IsActive(){
		if (_active){
			_text.enabled = true;
			_timer -= Time.deltaTime;
			if (_timer <= 0){
				_active = false;
				_text.enabled = false;
			}
		}
	}
}
