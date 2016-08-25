using UnityEngine;

public class PlayerSight : MonoBehaviour {

	public BoxCollider _col;
	public DayTimer _dayTimer;
	public Vector3 _daySize;
	public Vector3 _nightSize;
	public EndGame _endGame;

	void Start(){
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
		_endGame = GameObject.Find("HarbourCanvas").GetComponent<EndGame>();
		_col = gameObject.GetComponent<BoxCollider>();
		ChangeSightSize();
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "FOW"){
			var script = col.gameObject.GetComponent<FOW>();
			script._active = true;
			script.Seen();
		}
		else if (col.gameObject.tag == "Factory"){
			var script = col.gameObject.GetComponent<Factories>();
			script._seen = true;
		}
		else if (col.gameObject.tag == "Town"){
			var script = col.gameObject.GetComponent<TownManager>();
			script._seen = true;
		}
		_endGame._seen |= (col.gameObject.tag == "Harbour");
	}

	public void ChangeSightSize(){
		_col.size = (_dayTimer._hours < 12) ? _daySize : _nightSize;		
	}
}
