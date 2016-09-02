using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour {

	public CharDatabase _database;
	public WeaponDatabase _weaponDb;
	public ArmourDatabase _armourDb;
	public GameObject _characterList;
	public int _charIndex;
	public Button _moveRight, _moveLeft;
	public Text _name, _str, _dex, _vit;

	void Start(){
		_moveRight = GameObject.Find("MoveRight").GetComponent<Button>();
		_moveLeft = GameObject.Find("MoveLeft").GetComponent<Button>();
		_moveLeft.interactable = false;
		_characterList = GameObject.Find("CharacterList");
		_database = gameObject.GetComponent<CharDatabase>();
		_weaponDb = GameObject.Find("_manager").GetComponent<WeaponDatabase>();
		_armourDb = GameObject.Find("_manager").GetComponent<ArmourDatabase>();
		_name = GameObject.Find("Name").GetComponent<Text>();
		_str = GameObject.Find("StrText").GetComponent<Text>();
		_dex = GameObject.Find("DexText").GetComponent<Text>();
		_vit = GameObject.Find("VitText").GetComponent<Text>();		
		SetStats(0);
	}
	public void ScrollRight(){
		if (_charIndex < (_database._chars.Count -1)){
			_charIndex++;			
			SetStats(_charIndex);
			Scroll(false);
		}
	}

	public void ScrollLeft(){
		if (_charIndex > 0){
			_charIndex--;
			SetStats(_charIndex);
			Scroll(true);
		}		
	}

	public void SetStats(int index){
		_CombatManager._str = _database._chars[index]._str;
		_CombatManager._dex = _database._chars[index]._dex;
		_CombatManager._vit = _database._chars[index]._vit;
		_CombatManager._equipMelee = _weaponDb._meleeDatabase[_database._chars[index]._melee];
		_CombatManager._equipRanged = _weaponDb._rangedDatabase[_database._chars[index]._ranged];
		_CombatManager._headSlot = _armourDb._headDatabase[_database._chars[index]._head];
		_CombatManager._chestSlot = _armourDb._chestDatabase[_database._chars[index]._chest];
		_CombatManager._legSlot = _armourDb._legDatabase[_database._chars[index]._legs];

		_name.text = _database._chars[index]._name;
		_str.text = "Str: " + _database._chars[index]._str;
		_dex.text = "Dex: " + _database._chars[index]._dex;
		_vit.text = "Vit: " + _database._chars[index]._vit;
	}

	void Scroll(bool right){
		var move = right? 500 : -500;
		var posX = _characterList.transform.localPosition.x + move;
		var Pos = new Vector3(posX, 0, 0);
		_characterList.transform.localPosition = Pos;
		_moveLeft.interactable = (_charIndex > 0);
		_moveRight.interactable = (_charIndex < (_database._chars.Count-1));
	}
	public void StartGame(){
		SceneManager.LoadScene("World Map");
	}
}
