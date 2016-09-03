using UnityEngine;
using UnityEngine.UI;

public class AuxilliaryPower : MonoBehaviour {

	private CombatUI _combatUI;
	private PlayerTurn _pT;
	private Tooltip _tooltip;
	private Button _auxButton;
	public float _armourBonus = 1.0f;
	public float _aimBonus = 1.0f;
	public float _damageBonus = 1.0f;
	private int _healAmount = 50;

	public int _auxHealth;
	public int _auxArmour;
	public int _auxAim;
	public int _auxDam;

	public string _auxName;
	public string _auxDesc;
	public int _auxAp;

	private Sprite _icon_0;
	private Sprite _icon_1;
	private Sprite _icon_2;
	private SpriteState _state;

	public Sprite _heal_0, _heal_1, _heal_2;
	public Sprite _guard_0, _guard_1, _guard_2;
	public Sprite _aim_0, _aim_1, _aim_2;
	public Sprite _frenzy_0, _frenzy_1, _frenzy_2;

	public Button _skillsButton;

	void Awake(){		
		_combatUI = gameObject.GetComponent<CombatUI>();
		_tooltip = gameObject.GetComponent<Tooltip>();
		_pT = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_skillsButton = GameObject.Find("Skills").GetComponent<Button>();
		_auxButton = GameObject.Find("AuxilliaryPower").GetComponent<Button>();
		Initialize();
	}

	public void Initialize(){
		SetIcons();
		SetButton();
		SetSkill();
	}

	public void ActivatePower(){
		switch (_CombatManager._auxPower){
			case 0:
			Heal();
			break;
			case 1:
			Guard();
			break;
			case 2:
			KeenEye();
			break;
			case 3:
			Frenzy();
			break;
		}
		_skillsButton.interactable = false;
		_auxButton.interactable = false;
		_combatUI.UpdateAP();
		_tooltip.DisableTooltip();
	}

	void Heal(){
		if (_pT._ap >= _auxAp){
			_CombatManager._currentHealth += _healAmount;
			_pT._ap -= _auxAp; 
			if (_CombatManager._currentHealth > _CombatManager._maxHealth) {
				_CombatManager._currentHealth = _CombatManager._maxHealth;
			}		
			_combatUI.UpdateHealth();
		}		
	}
	void Guard(){
		if (_pT._ap >= _auxAp){
			_armourBonus = 1.2f;
			_pT._ap -= _auxAp;
		}		
	}
	void KeenEye(){
		if (_pT._ap >= _auxAp){
			_aimBonus = 1.2f;
			_pT._ap -= _auxAp;
		}
	}
	void Frenzy(){
		if (_pT._ap >= _auxAp){
			_damageBonus = 1.2f;
			_armourBonus = 0.6f;
			_aimBonus = 0.9f;
			_pT._ap -= _auxAp;
		}		
	}

	public void EnableButtons(){
		_skillsButton.interactable = true;
		_auxButton.interactable = true;
		_aimBonus = 1.0f;
		_damageBonus = 1.0f;
		_armourBonus = 1.0f;
	}
	void SetIcons(){
		switch (_CombatManager._auxPower){
			case 0:
			_icon_0 = _heal_0;
			_icon_1 = _heal_1;
			_icon_2 = _heal_2;
			break;
			case 1:
			_icon_0 = _guard_0;
			_icon_1 = _guard_1;
			_icon_2 = _guard_2;
			break;
			case 2:
			_icon_0 = _aim_0;
			_icon_1 = _aim_1;
			_icon_2 = _aim_2;
			break;
			case 3:
			_icon_0 = _frenzy_0;
			_icon_1 = _frenzy_1;
			_icon_2 = _frenzy_2;
			break;

		}
	}

	void SetButton(){
		_auxButton.image.sprite = _icon_0;
		_state.pressedSprite = _icon_1;
		_state.disabledSprite = _icon_1;
		_state.highlightedSprite = _icon_2;
		_auxButton.spriteState = _state;
	}

	void SetSkill(){
		_auxHealth = 0;
		_auxArmour = 0;
		_auxAim = 0;
		_auxDam = 0;
		switch (_CombatManager._auxPower){
			case 0:
			_auxHealth = 100;
			_auxName = "Heal";
			_auxDesc = "Heal 100 points of damage. \n One use per round";
			_auxAp = 8;
			break;
			case 1:
			_auxArmour = 15;
			_auxName = "Guard";
			_auxDesc = "Receiving a bonus to armour\nuntil the start of your next turn";
			_auxAp = 12;
			break;
			case 2:
			_auxAim = 10;
			_auxName = "Keen Eye";
			_auxDesc = "Receive a bonus to hit \n until the end of the round";
			_auxAp = 8;
			break;
			//Melee Only - damage bonus
			case 3:
			_auxDam = 5;
			_auxName = "Frenzy";
			_auxDesc = "Lower accuracy & armour\n to receive damage boost";
			_auxAp = 12;
			break;
		}
	}
}
