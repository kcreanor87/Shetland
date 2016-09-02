using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CombatUI : MonoBehaviour {

	private PlayerTurn _playerTurn;
	private Tooltip _toolTip;
	private CalculateDamage _calc;
	public AuxilliaryPower _auxPower;
	public MoveIndicator _moveIndicator;
	public Animator _anim;
	private Button _attackAbutton;
	private Button _attackBbutton;
	private Button _attackCbutton;
	public GameObject _player;
	public GameObject _pauseMenu;
	public GameObject _errorGO;
	public Text _errorText;
	public Image _apImage;
	public Sprite _apPositive;
	public Sprite _apNegative;
	public Sprite _moveEnabled, _moveDisabled;
	public Sprite _attackEnabled, _attackDisabled;
	public Sprite _weaponSwitchSprite;
	public Sprite _attackASprite_0, _attackASprite_1;
	public Sprite _attackBSprite_0, _attackBSprite_1;
	public Sprite _attackCSprite_0, _attackCSprite_1;
	private Button _moveButton;
	private Button _toggleAttackButton;

	private GameObject _hpBar;
	private RectTransform _hpRect;
	private RectTransform _hpBack;
	private RectTransform _apRect;
	private RectTransform _apBack;
	private RectTransform _apLossRect;
	private GameObject _apBar;
	public GameObject _apLoss;
	public GameObject _hpap;
	private Text _moveCost;
	public Text _hpapText;

	public bool _paused;

	public string _activeAction = "Regular Shot";

    public void Initialize(){
    	_errorGO = GameObject.Find("ErrorGO");		
    	_errorText = GameObject.Find("losText").GetComponent<Text>();
		_player = GameObject.Find("Player");
		_playerTurn = _player.GetComponent<PlayerTurn>();		
		_apBar = GameObject.Find("APbar");
		_apRect = _apBar.GetComponent<RectTransform>();
		_apBack = GameObject.Find("APback").GetComponent<RectTransform>();
		_hpBar = GameObject.Find("HPbar");
		_hpRect = _hpBar.GetComponent<RectTransform>();
		_hpBack = GameObject.Find("HPback").GetComponent<RectTransform>();
		_apLoss = GameObject.Find("APLoss");
		_apLossRect = _apLoss.GetComponent<RectTransform>();
		_apImage = _apLoss.GetComponent<Image>();
		_moveIndicator = GameObject.Find("Player").GetComponent<MoveIndicator>();
		_toolTip = gameObject.GetComponent<Tooltip>();
		_moveCost = GameObject.Find("MoveCost").GetComponent<Text>();
		_moveButton = GameObject.Find("ToggleMove").GetComponent<Button>();
		_toggleAttackButton = GameObject.Find("ToggleAttack").GetComponent<Button>();
		_attackAbutton = GameObject.Find("AttackTypeA").GetComponent<Button>();
		_attackBbutton = GameObject.Find("AttackTypeB").GetComponent<Button>();
		_attackCbutton = GameObject.Find("AttackTypeC").GetComponent<Button>();
		_calc = GameObject.Find("_combatMan").GetComponent<CalculateDamage>();
		_auxPower = GameObject.Find("_combatMan").GetComponent<AuxilliaryPower>();
		_hpap = GameObject.Find("HP/AP");
		_hpapText = GameObject.Find("HP/AP Text").GetComponent<Text>();
		_hpap.SetActive(false);
		_pauseMenu = GameObject.Find("PauseMenu");
		_errorGO.SetActive(false);
		_pauseMenu.SetActive(false);
		DeactivateButtons(false);
		UpdateHealth();
	}

	void Update(){
		if (!_playerTurn._moving && !_paused){
			TextOverMouse();
		}
		PauseMenu();	
	}

	public void ButtonToggle(int i){
		_anim = _player.GetComponent<Animator>();
		_toolTip.DisableTooltip();
		//Switch statement controls what happens on each button press
		switch (i){
			//"Move" Button - on press, disable attack options, set sprite to red
			case 0:
            _playerTurn._attackToggle = false;
            _moveButton.image.sprite = _moveEnabled;
            DeactivateButtons(false);
            break;
            //"Attack" button - disable move sprite, open attack options, set sprite to red
			case 1:			
			_playerTurn._attackToggle = !_playerTurn._attackToggle;
			_moveButton.image.sprite = _moveDisabled;
			AttackButtonSprite(1);
			DeactivateButtons(_playerTurn._attackToggle);
			_playerTurn._attackCost = 8;
			break;
			//"Switch Weapons" button - disable attack options and toggle player weapons
            case 2:
            _playerTurn._attackToggle = false;          		               
            _playerTurn._meleeToggle = !_playerTurn._meleeToggle;
			_anim.SetBool("WeaponB", _playerTurn._meleeToggle);				
			DeactivateButtons(false);
			_playerTurn._ap -= 8;
			UpdateAP();
			break;
			//First Attack option - set AP cost to low, chance to hit to low
			case 3:
			_activeAction = "Quick Shot";
			_playerTurn._attackCost = 5;
			AttackButtonSprite(0);	
			_calc._toHitModifier = -20;	
			break;
			//second Attack option - (default) regular attack stats
			case 4:
			_activeAction = "Regular Shot";
			_playerTurn._attackCost = 8;
			AttackButtonSprite(1);
			_calc._toHitModifier = 0;
			break;
			//Third Attack option - high AP cost, increase to hit chance
			case 5:
			_activeAction = "Aimed Shot";
			_playerTurn._attackCost = 12;
			AttackButtonSprite(2);
			_calc._toHitModifier = 20;
			break;
			case 6:
			break;
			default:
			print("Error - Button Toggle");
			break;
		}
    }
    //Void to control the active status of the three attack type buttons
	public void DeactivateButtons(bool active){
		_attackAbutton.gameObject.SetActive(active);
		_attackBbutton.gameObject.SetActive(active);
		_attackCbutton.gameObject.SetActive(active);
		_playerTurn._attackToggle = active;
		if (!active){
			_moveButton.image.sprite = _moveEnabled;
			_toggleAttackButton.image.sprite = _attackDisabled;
		}
		else{
			_toggleAttackButton.image.sprite = _attackEnabled;
		}
	}
	//Void to control the three attack option sprites
	void AttackButtonSprite(int i){
		//Reset all sprites
		_attackAbutton.image.sprite = _attackASprite_0;
		_attackBbutton.image.sprite = _attackBSprite_0;
		_attackCbutton.image.sprite = _attackCSprite_0;
		//Activate the relevant sprite
		switch (i){
			case 0:
			_attackAbutton.image.sprite = _attackASprite_1;
			break;
			case 1:
			_attackBbutton.image.sprite = _attackBSprite_1;
			//Reset Attack type - Used when toggling attack for the 1st time each turn
			_activeAction = "Regular Shot";
			_playerTurn._attackCost = 8;
			_calc._toHitModifier = 0;
			break;
			case 2:
			_attackCbutton.image.sprite = _attackCSprite_1;
			break;
		}
	}
	//Retreat function, return to previous town, day++ but prices stay the same
	public void Retreat(){
		SceneManager.LoadScene(0);
	}

	public void UpdateAP(){
		_apBack.sizeDelta = new Vector2(_CombatManager._maxAP*10, 29);
		_apRect.sizeDelta = new Vector2(_CombatManager._maxAP*5-27, 29);
		_apLossRect.sizeDelta = new Vector2(_apRect.sizeDelta.x, 29);
		_apLoss.transform.localPosition = new Vector2(_CombatManager._maxAP*5-27, 0);
		_apLoss.transform.localScale = new Vector3 (0, 1, 1);
		float i = (float)_playerTurn._ap/_CombatManager._maxAP;
		_apBar.transform.localScale = new Vector3(i, 1.0f, 1.0f);
	}

	public void UpdateHealth(){
		_hpBack.sizeDelta = new Vector2(_CombatManager._vit*280, 29);
		_hpRect.sizeDelta = new Vector2((_CombatManager._vit*143)-25, 29);
		float i = (float)_CombatManager._currentHealth/_CombatManager._maxHealth;
		_hpBar.transform.localScale = (i > 0) ? new Vector3(i, 1.0f, 1.0f) : new Vector3(0.0f, 1.0f, 1.0f);
	}
	public void HP_AP(bool active){
		_hpap.SetActive(active);
		_hpapText.text = "" + _CombatManager._currentHealth + " / " + _CombatManager._maxHealth + "\n" + _playerTurn._ap + " / " + _CombatManager._maxAP;
	}

	void TextOverMouse(){
		_moveCost.enabled = _moveIndicator._moveInd.activeInHierarchy;
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		y = y + 60;
		_moveCost.transform.position = new Vector2(x, y);
		var distance = Mathf.CeilToInt(_moveIndicator._distance);
		_moveCost.text = "" + distance + " AP";
		_moveCost.color = (distance > _playerTurn._ap) ? Color.red : Color.white;
		if (!EventSystem.current.IsPointerOverGameObject()){
			float APCost = ((float) distance/_playerTurn._ap) * -1;		
			_apImage.sprite = (APCost < -1) ? _apNegative : _apPositive;
			_apLoss.transform.localScale = (APCost < -1) ? new Vector3 (-1.0f,1.0f,1.0f) : new Vector3 (APCost, 1, 1);
		}				
	}
	void PauseMenu(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (Time.timeScale < 1.0f){
				ResumeGame();
			}
			else{
				Time.timeScale = 0.0f;
				_pauseMenu.SetActive(true);
				_paused = true;
			}
		}
	}

	public void ErrorMessage(bool active){
		_errorGO.SetActive(active);
	}

	public void ResumeGame(){
		Time.timeScale = 1.0f;
		_pauseMenu.SetActive(false);
		_paused = false;
	}

	public void ToMainMenu(){
		SceneManager.LoadScene(0);
	}	

	public void QuitToDesktop(){
		Application.Quit();
	}	
}