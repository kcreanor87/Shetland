//For use at the start of a new game to allow
//the player to allocate stat points

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GenerateStats : MonoBehaviour {

	//Cache of points to allocate to stats
	public int _points = 10;

	//Access the text elements of the UI
	private Text _strText;
	private Text _dexText;
	private Text _vitText;
	private Text _lvlText;
	private Text _pointsText;
	private Text _itemText;
	private Text _meleeDR;
	private Text _rangedDR;
	private Text _armour;
	public Text _equippedMelee;
	public Text _equippedRanged;
	public Text _equippedHead;
	public Text _equippedChest;
	public Text _equippedLegs;

	private GameObject _itemList;

	public GameObject _itemButton;

	public List<GameObject> _buttonGOs = new List<GameObject>();

	public int _activeList;

	public WeaponDatabase _weaponDatabase;
	public ArmourDatabase _armourDatabase;

	void Start(){

		//Initialize the UI
		_strText = GameObject.Find("StrVar").GetComponent<Text>();
		_dexText = GameObject.Find("DexVar").GetComponent<Text>();
		_vitText = GameObject.Find("VitVar").GetComponent<Text>();
		_lvlText = GameObject.Find("LevelVar").GetComponent<Text>();
		_pointsText = GameObject.Find("PointsVar").GetComponent<Text>();
		_meleeDR = GameObject.Find("MeleeDRVar").GetComponent<Text>();
		_rangedDR = GameObject.Find("RangedDRVar").GetComponent<Text>();
		_armour = GameObject.Find("ArmourRatingVar").GetComponent<Text>();
		_equippedMelee = GameObject.Find("MeleeButtonText").GetComponent<Text>();
		_equippedRanged = GameObject.Find("RangedButtonText").GetComponent<Text>();
		_equippedHead = GameObject.Find("HeadButtonText").GetComponent<Text>();
		_equippedChest = GameObject.Find("ChestButtonText").GetComponent<Text>();
		_equippedLegs = GameObject.Find("LegsButtonText").GetComponent<Text>();
		//_chaText = GameObject.Find("ChaText").GetComponent<Text>();
		_weaponDatabase = GameObject.Find("_CombatManager").GetComponent<WeaponDatabase>();
		_armourDatabase = GameObject.Find("_CombatManager").GetComponent<ArmourDatabase>();
		UpdateStats();
		_itemList = GameObject.Find("ItemSelect");
		_itemList.SetActive(false);
	}

	//UI Button method to increase and decrease stat. i is declared in the inspector
	public void IncreaseStat(int i){
		if (_points > 0){
			switch (i){
				case 0:
					if (_CombatManager._str < 10){
						_CombatManager._str++;
						_points--;
					}					
					break;
				case 1:
					if (_CombatManager._dex < 10){
						_CombatManager._dex++;
						_points--;
					}					
					break;
				case 2:
					if (_CombatManager._vit < 10){
						_CombatManager._vit++;
						_points--;
					}
					break;
				default:
					print("Error - _CombatManager.IncreaseStat()");
					break;
			}
		}
		_CombatManager.CalculateStats();
		UpdateStats();
	}

	//UI Button method to decrease stats. i is declared in the inspector
	public void DecreaseStat(int i){
		switch (i){
			case 0:
				if (_CombatManager._str > 1){
					_CombatManager._str--;
					_points++;
				}
				break;
			case 1:
				if (_CombatManager._dex > 1){
					_CombatManager._dex--;
					_points++;
				}
				break;
			case 2:
				if (_CombatManager._vit > 1){
					_CombatManager._vit--;
					_points++;
				}
				break;
			default:
				print("Error - DecreaseStat()");
				break;
		}
		_CombatManager.CalculateStats();
		UpdateStats();
	}

	public void ChangeLevel(int i){
		if (i == 0){
			_CombatManager._playerLevel++;
			_points++;
		}
		else{
			if (_points > 0 && _CombatManager._playerLevel > 1){
				_CombatManager._playerLevel--;
				_points--;
			}
			else{
				//Display fault text
			}
		}
		_CombatManager.CalculateStats();
		UpdateStats();
	}

	//Method to update the UI to match value changes
	public void UpdateStats(){
		_strText.text = _CombatManager._str.ToString();
		_dexText.text = _CombatManager._dex.ToString();
		_vitText.text = _CombatManager._vit.ToString();
		_lvlText.text = _CombatManager._playerLevel.ToString();
		_pointsText.text = _points.ToString();
		_meleeDR.text = _CombatManager._meleeDam.ToString();
		_rangedDR.text = _CombatManager._rangedDam.ToString();
		_armour.text = _CombatManager._armourRating.ToString();
		//_chaText.text = _CombatManager._cha.ToString();
		_equippedMelee.text = _CombatManager._equipMelee._name;
		_equippedRanged.text = _CombatManager._equipRanged._name;
		_equippedHead.text = _CombatManager._headSlot._name;
		_equippedChest.text = _CombatManager._chestSlot._name;
		_equippedLegs.text = _CombatManager._legSlot._name;
		for (int i = 0; i < _buttonGOs.Count; i++){
			Destroy(_buttonGOs[i]);
		}
	}

	public void OpenList(int i){
		_activeList = i;
		if (!_itemList.activeInHierarchy){
			_itemList.SetActive(true);
			switch (i){
				case 0:
				for (int j = 0; j <_weaponDatabase._meleeDatabase.Count; j++){
					GenerateButton(j);
					_itemText.text = _weaponDatabase._meleeDatabase[j]._name;
				}
				break;
				case 1:
				for (int j = 0; j <_weaponDatabase._rangedDatabase.Count; j++){
					GenerateButton(j);
					_itemText.text = _weaponDatabase._rangedDatabase[j]._name;
				}
				break;
				case 2:
				for (int j = 0; j <_armourDatabase._headDatabase.Count; j++){
					GenerateButton(j);
					_itemText.text = _armourDatabase._headDatabase[j]._name;
				}
				break;
				case 3:
				for (int j = 0; j <_armourDatabase._chestDatabase.Count; j++){
					GenerateButton(j);
					_itemText.text = _armourDatabase._chestDatabase[j]._name;
				}
				break;
				case 4:
				for (int j = 0; j <_armourDatabase._legDatabase.Count; j++){
					GenerateButton(j);
					_itemText.text = _armourDatabase._legDatabase[j]._name;
				}
				break;
				default:
				Debug.Log("Error - OpenList function");
				break;
			}
		}
	}

	void GenerateButton(int i){
		var k = (i*55)+5;
		var buttonClone = (GameObject)Instantiate(_itemButton, new Vector3(0, -k, 0), Quaternion.identity);
		buttonClone.transform.SetParent(_itemList.transform, false);
		ButtonsGenerator _gen = buttonClone.GetComponent<ButtonsGenerator>();
		_gen._type = i;
		_gen._activeList = _activeList;
		_itemText = buttonClone.GetComponentInChildren<Text>();
		_buttonGOs.Add(buttonClone);
	}

	//Specifically for the 'Start Game' button, only allow progression
	//if there are no points left to spend.
	public void StartGame(){
		if (_points == 0){
			SceneManager.LoadScene("Combat");
		}
		else{
			print("You still have points remaining");
		}
	}
}
