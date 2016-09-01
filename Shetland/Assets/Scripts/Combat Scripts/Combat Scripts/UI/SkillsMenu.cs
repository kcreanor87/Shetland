using UnityEngine;
using UnityEngine.UI;

public class SkillsMenu : MonoBehaviour {

	public CombatUI _combatUI;
	public AuxilliaryPower _aux;
	public PlayerTurn _pT;
	public GameObject _skillsMenu;
	public RectTransform _menuRect;
	public GameObject _playerMenu;
	public bool _menuOpen;
	public GameObject _skillPrefab;

	public Button _button;
	public Text _name;
	public Text _desc;
	public Text _ap;
	public string _skillName;
	public string _skillDesc;
	public int _skillAP;

	private Sprite _icon_0;
	private Sprite _icon_1;
	private Sprite _icon_2;

	public Sprite _heal_0, _heal_1, _heal_2;
	public Sprite _guard_0, _guard_1, _guard_2;
	public Sprite _aim_0, _aim_1, _aim_2;
	public Sprite _frenzy_0, _frenzy_1, _frenzy_2;

	void Start(){
		_combatUI = gameObject.GetComponent<CombatUI>();
		_aux = gameObject.GetComponent<AuxilliaryPower>();
		_pT = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_playerMenu = GameObject.Find("PlayerTurn");
		_skillsMenu = GameObject.Find("SkillsMenu");
		_menuRect = _skillsMenu.GetComponent<RectTransform>();
		SpawnMenu();
		_skillsMenu.SetActive(false);
	}

	void SpawnMenu(){
		float height = (_CombatManager._skills.Count+1) * 125.0f;
		for (int i = 0; i < _CombatManager._skills.Count; i++){
			var _clone = (GameObject) Instantiate(_skillPrefab, transform.position, transform.rotation);	
			_clone.transform.SetParent(_skillsMenu.transform, false);
			var _rect = _clone.GetComponent<RectTransform>();
			var _pos = (i+1) * 125.0f;
			_rect.anchoredPosition = new Vector3(0, -_pos,0);
			SetIcon(i, _clone);
			SetText(i, _clone);
		}
		_menuRect.sizeDelta = new Vector2(500, height);
		float _yPos = -700.0f + (_CombatManager._skills.Count*125.0f);
		_menuRect.anchoredPosition = new Vector2(0, _yPos);
	}

	public void OpenMenu(){
		_combatUI._paused = true;
		_skillsMenu.SetActive(true);		
	}

	public void CloseMenu(){
		_skillsMenu.SetActive(false);
		_pT.EnableCanvas();
		_menuOpen = false;
		_combatUI._paused = false;
	}

	void SetIcon(int i, GameObject clone){
		_button = clone.GetComponentInChildren<Button>();
		var _state = new SpriteState();
		switch (i){
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
		_button.image.sprite = _icon_0;
		_state.pressedSprite = _icon_1;
		_state.disabledSprite = _icon_1;
		_state.highlightedSprite = _icon_2;
		_button.spriteState = _state;
		_button.onClick.AddListener(delegate {SetButton(i); });
	}

	void SetText(int i, GameObject clone){
		switch (i){
			case 0:
			_skillName = "Heal";
			_skillDesc = "Immediately heal yourself \n for 100 points of damage";
			_skillAP = 8;
			break;
			case 1:
			_skillName = "Guard";
			_skillDesc = "Receive a bonus to armour. \n until the start of your next turn";
			_skillAP = 12;
			break;
			case 2:
			_skillName = "Keen Eye";
			_skillDesc = "Receive a small bonus to accuracy \n until the end of the round";
			_skillAP = 8;
			break;
			case 3:
			_skillName = "Frenzy";
			_skillDesc = "Lower accuracy & armour\n to receive damage boost";
			_skillAP = 12;
			break;
		}
		_name = clone.transform.FindChild("SkillName").GetComponent<Text>();
		_desc = clone.transform.FindChild("SkillDesc").GetComponent<Text>();
		_ap = clone.transform.FindChild("SkillAP").GetComponent<Text>();
		_name.text = _skillName;
		_desc.text = _skillDesc;
		_ap.text = "" + _skillAP;
	}

	public void SetButton(int i){
		_CombatManager._auxPower = i;
		CloseMenu();
		_aux.Initialize();
	}
}
