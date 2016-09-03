using UnityEngine;
using UnityEngine.UI;

public class WorldMap_Icons : MonoBehaviour {

	public string _name;
	public GameObject _badge;
	public Text _badgeText;
	void Start(){
		_badge = transform.FindChild("Badge").gameObject;
		_badgeText = _badge.GetComponentInChildren<Text>();
		_badge.SetActive(false);
	}

	public void MouseOver(){
		_badgeText.text = _name;
		_badge.SetActive(true);
		transform.SetAsLastSibling();
	}

	public void MouseExit(){
		_badge.SetActive(false);
	}
}
