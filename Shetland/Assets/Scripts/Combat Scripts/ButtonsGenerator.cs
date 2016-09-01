using UnityEngine;

public class ButtonsGenerator : MonoBehaviour {

	public int _type;
	public int _activeList;
	public WeaponDatabase _weaponDatabase;
	public ArmourDatabase _armourDatabase;
	public GameObject _itemList;
	public GenerateStats _generator;

	void Awake(){
		_generator = GameObject.Find("CharCreate").GetComponent<GenerateStats>();
		_itemList = GameObject.Find("ItemSelect");
		_weaponDatabase = GameObject.Find("_CombatManager").GetComponent<WeaponDatabase>();
		_armourDatabase = GameObject.Find("_CombatManager").GetComponent<ArmourDatabase>();
	}

	public void SelectItem(){
		_itemList.SetActive(false);
		switch(_activeList){
			case 0:
			_CombatManager._equipMelee = _weaponDatabase._meleeDatabase[_type];
			break;
			case 1:
			_CombatManager._equipRanged = _weaponDatabase._rangedDatabase[_type];
			break;
			case 2:
			_CombatManager._headSlot = _armourDatabase._headDatabase[_type];
			break;
			case 3:
			_CombatManager._chestSlot = _armourDatabase._chestDatabase[_type];
			break;
			case 4:
			_CombatManager._legSlot = _armourDatabase._legDatabase[_type];
			break;
			default:
			Debug.Log("Error - SelectItem function");
			break;
		}
		_CombatManager.CalculateStats();
		_generator.UpdateStats();
	}
}
