using UnityEngine;
using UnityEngine.UI;

public class Cover : MonoBehaviour {

	public int _bonus;
	public int _hp;
	public EnemyScript _enemy;
	public PlayerTurn _pT;
	public bool _occupied;
	public Vector3 _defPoint;
	public Text _defText;
	public Animator _anim;
	public Canvas _coverCanvas;

	void Start(){
		_pT = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_anim = gameObject.GetComponent<Animator>();
		_coverCanvas = transform.parent.FindChild("Canvas").GetComponent<Canvas>();
		_coverCanvas.enabled = false;
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Player"){
			_CombatManager._armourRating += _bonus;
			_pT._inCover = true;
			_pT._coverDef = _defPoint;
			_occupied = true;
			_coverCanvas.enabled = true;
		}
		else if (col.tag == "Enemy"){
			_enemy = col.GetComponent<EnemyScript>();
			_enemy._def += _bonus;
			_occupied = true;
			_coverCanvas.enabled = true;
		}
	}

	void OnTriggerExit(Collider col){		
		if (col.tag == "Player"){
			_CombatManager._armourRating -= _bonus;
			_occupied = false;
			_pT._inCover = true;
			_coverCanvas.enabled = false;
		}
		else if (col.tag == "Enemy"){
			_enemy._def -= _bonus;
			_occupied = false;
			_coverCanvas.enabled = false;
		}
	}

	public void BeenHit(){
		if (_hp <= 0){
			_anim.SetBool("Broken", true);
		}
	}

	public void OnMouseOver(){
		_coverCanvas.enabled = (_pT._active && !_pT._moving && !_pT._attacking);
	}

	public void OnMouseExit(){
		_coverCanvas.enabled = _occupied;
	}
}
