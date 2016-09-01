//Script controlling player input and statistic throughout combat

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerTurn : MonoBehaviour {
	//References scripts
	private CombatUI _combatUI;
	public CombatOver _combatOver;	
	public AuxilliaryPower _aux;
	public CalculateDamage _calcDam;
	public CameraController _cam;
	public TargetedEnemy _targetedEnemy;	
	public TurnManager _tM;
	public MuzzleFlash _muzzleFlash;
	public MoveIndicator _moveIndicator;
	public Cover _cover;
	//Navigation variables
	public NavMeshAgent _agent;
	public NavMeshObstacle _obstacle;
	public NavMeshPath _path;
	public LayerMask _mask = 1 << 9;
	//Gameobjects and transforms referenced in scripts
	public GameObject _playerTurnUI;
	public GameObject _midPoint;	
	public Transform _enemyTrans;	
	//Player and enemy Animators
	public Animator _anim;
	public Animator _enemyAnim;
	//Targeted position variable
	public Vector3 _targetPos;
	//Location player can be hit from in cover
	public Vector3 _coverDef;
	//Bools - controls current player states
	public bool _active;
	public bool _moving;
	public bool _attackToggle;
	public bool _meleeToggle;
	public static bool _underAttack;
	public bool _dead;	
	public bool _attacking;
	public bool _inCover;
	//Ints for attack costing
	public int _ap;
	public int _attackType;	
	public int _attackCost;
	public float _distance;

	void Awake () {
		//Find and allocate the referenced objects and scripts in the scene
		_muzzleFlash = gameObject.GetComponentInChildren<MuzzleFlash>();
		_midPoint = GameObject.Find("MidPoint");
		_anim = gameObject.GetComponent<Animator>();
		_agent = gameObject.GetComponent<NavMeshAgent>();
		_obstacle = gameObject.GetComponent<NavMeshObstacle>();
		_combatUI = GameObject.Find("_combatMan").GetComponent<CombatUI>();
		_calcDam = GameObject.Find("_combatMan").GetComponent<CalculateDamage>();
		_targetedEnemy = GameObject.Find("_combatMan").GetComponent<TargetedEnemy>();
		_moveIndicator =GameObject.Find("Player").GetComponent<MoveIndicator>();
		_playerTurnUI = GameObject.Find("PlayerTurn");
		_cam = GameObject.Find("_cameraParent").GetComponent<CameraController>();
		_combatOver = GameObject.Find("EndCanvas").GetComponent<CombatOver>();
		_aux = GameObject.Find("_combatMan").GetComponent<AuxilliaryPower>();
		//Determine player stats based off	
		_ap = _CombatManager._maxAP;
		_combatUI.Initialize();
		//Prepare the elements used for navigation
		_mask = ~_mask;
		_path = new NavMeshPath();
		_aux.Initialize();		
	}
	public void Initialize(){
		_agent.Resume();
		_ap = _CombatManager._maxAP;
		_active = true;		
		_playerTurnUI.SetActive(true);
		_combatUI.UpdateAP();
		_aux.EnableButtons();
	}

	void Update(){
		if (!_dead){
			if (_combatUI._paused){
				_playerTurnUI.SetActive(false);
				_moveIndicator.ActiveStatus(false, false);
				return;
			}
			RotateTowards();
			Health();
			if (!_active){
				_playerTurnUI.SetActive(false);
				return;
			}
			if (!_attackToggle && !_attacking){
				Move();
				return;
			}
			if (!_attacking) PlayerAttack();
		}
	}

	//Function for getting mouse pointer target and setting it as a destination
	void Move(){
		//If not already moving or mouse over UI element
		if (_moving || EventSystem.current.IsPointerOverGameObject()){
			EndMove();
			_combatUI.ErrorMessage(false);
			return;
		}
		_playerTurnUI.SetActive(true);
		//Cast ray to mouse point
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			if (hit.collider.tag == "Ground"){
				_targetPos = hit.point;
				_targetPos.y = 0.1f;
				_moveIndicator.ActiveStatus(false, true);
				//Call DrawPath IEnumerator to draw path
				_agent.CalculatePath(_targetPos, _path);
				_moveIndicator.DrawPath(_targetPos, _path);
				if (_distance > _ap){
					_moveIndicator.ActiveStatus(false, true);
					_combatUI.ErrorMessage(true);
					_combatUI._errorText.text = "Not enough Action Points!";
				}
				else if (_distance < 0.9f){
					_moveIndicator.ActiveStatus(false, false);
				}
				else{
					_combatUI.ErrorMessage(false);
					_moveIndicator.ActiveStatus(true, true);;
					//On mouse press, set the mouse point as target and deduct approriate AP, set moving to true
					if (Input.GetMouseButtonDown(0) && _distance > 0.5f){
						_agent.SetDestination(_targetPos);
						_moving = true;
						_anim.SetBool("Running", true);
						_ap -= Mathf.CeilToInt(_distance);
						_moveIndicator.ActiveStatus(false, false);;
						_playerTurnUI.SetActive(false);
						_combatUI.UpdateAP();
					}
				}
			}
			else if (hit.collider.tag == "Cover"){
				_targetPos = hit.collider.transform.parent.position;
				_targetPos.y = 0.1f;
				_agent.CalculatePath(_targetPos, _path);
				_moveIndicator.DrawPath(_targetPos, _path);
				_cover = hit.collider.GetComponent<Cover>();
				if (_distance > _ap){
					
					_combatUI.ErrorMessage(true);
					_moveIndicator.ActiveStatus(false, true);
				}
				else{
					_moveIndicator.ActiveStatus(true, true);
					_agent.CalculatePath(_targetPos, _path);
					_moveIndicator.DrawPath(_targetPos, _path);
					//SHOW TOOLTIP WITH NAME, BONUS + AP COST
					if (Input.GetMouseButtonDown(0)){					
						_agent.SetDestination(_targetPos);
						_moving = true;
						_anim.SetBool("Running", true);
						_moveIndicator.ActiveStatus(false, false);
						_playerTurnUI.SetActive(false);
						_ap -= Mathf.CeilToInt(_distance);
						_combatUI.UpdateAP();
					}
				}				
			}
			else{
				if (!EventSystem.current.IsPointerOverGameObject()){
					_combatUI.UpdateAP();
					_moveIndicator.ActiveStatus(false, false);
					_combatUI.ErrorMessage(false);
				}				
			}
		}
		else{
			_moveIndicator.ActiveStatus(false, false);
			_combatUI.ErrorMessage(false);
		}
	}	

	//Function to detect when at the end of movement phase and re-enable user input
	void EndMove(){
		if (Vector3.Distance(transform.position, _targetPos) < 0.5f){
			_moving = false;
			_anim.SetBool("Running", false);
			_moveIndicator.ActiveStatus(false, true);
		}
		else{
			_moveIndicator.ActiveStatus(false, false);
		}
	}

	void PlayerAttack(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			if (hit.collider.tag == "Enemy"){
				var target = hit.collider.transform;
				if (Physics.Linecast(transform.position, target.position, _mask.value)){
					_combatUI.ErrorMessage(true);
					_combatUI._errorText.text = "No Line of Sight";
				}
				else{					
					_distance = Vector3.Distance(transform.position, target.position);
					if (_meleeToggle){						
						if (_distance  < 3.0f){	
							float APCostPercent = ((float) _attackCost/_ap) * -1;		
							_combatUI._apImage.sprite = (APCostPercent < -1) ? _combatUI._apNegative : _combatUI._apPositive;
							_combatUI._apLoss.transform.localScale = (APCostPercent < -1) ? new Vector3 (-1.0f,1.0f,1.0f) : new Vector3 (APCostPercent, 1, 1);
							_combatUI.ErrorMessage(false);
							_enemyTrans = hit.collider.transform;
							_calcDam.CalculateChance();
							_targetedEnemy._target = target.position;
							_targetedEnemy._targetText = "" + target.name + "\n" + _calcDam._chanceToHit + " % To Hit";
							_targetedEnemy.EnableTarget();
							if (Input.GetMouseButtonDown(0) && _ap >= _attackCost){
								_targetPos = target.position;
								_enemyAnim = hit.collider.GetComponentInChildren<Animator>();
								_attackType = 2;								
								StartCoroutine(Attack());
								
							}
						}
						else {
							_combatUI.ErrorMessage(false);
							_combatUI._errorText.text = "Out of Range!";
						}
					}
					else{				
						float APCostPercent = ((float) _attackCost/_ap) * -1;		
						_combatUI._apLoss.transform.localScale = (APCostPercent > -1) ? new Vector3 (APCostPercent, 1, 1) : new Vector3 (-1.0f,1.0f,1.0f);
						_combatUI.ErrorMessage(false);
						_enemyTrans = hit.collider.transform;
						_calcDam._rangeModifier = (_distance > _CombatManager._equipRanged._range) ? -30 : 0;
						_calcDam.CalculateChance();
						_targetedEnemy._target = hit.collider.transform.position;
						_targetedEnemy._targetText = "" + hit.collider.name + "\n" + _calcDam._chanceToHit + " % To Hit";
						_targetedEnemy.EnableTarget();
						if (Input.GetMouseButtonDown(0) && _ap >= _attackCost){
							_targetPos = target.position;
							_enemyAnim = hit.collider.GetComponentInChildren<Animator>();							
							_attackType = 1;
							StartCoroutine(Attack());
						}
					}
				}
			}
			else if (hit.collider.tag == "Cover"){

			}
			else{
				_combatUI.ErrorMessage(false);
				if (!EventSystem.current.IsPointerOverGameObject()){
					_combatUI._apLoss.transform.localScale = new Vector3(0, 1.0f, 1.0f);
				}				
				_targetedEnemy.DisableTarget();
			}
		}
		if (Input.GetMouseButton(1)){
			_combatUI.ErrorMessage(false);
			_attackToggle = false;
			_combatUI.DeactivateButtons(false);
			_targetedEnemy.DisableTarget();
		}
	}
	//Funciton to call when attack is submitted
	IEnumerator Attack(){
		//Disable targetting UI
		_targetedEnemy.DisableTarget();
		//Get midpoint info and move Camera to location
		var x = transform.position.x + (_targetPos.x - transform.position.x)/2;
		var z = transform.position.z + (_targetPos.z - transform.position.z)/2;
		_midPoint.transform.position = new Vector3(x, 0, z);
		_cam._playerTarget = _targetPos;
		_cam.EnableShotCam();
		//Deduct AP and minimize/reset UI
		_ap -= _attackCost;		
		_combatUI.UpdateAP();		
		_combatUI.DeactivateButtons(false);		
		_attackToggle = false;
		_attacking = true;
		//Allow time for the camera to move
		yield return new WaitForSeconds(0.4f);		
		//Check what kind of attack was submitted
		switch (_attackType){
			case 0:
				print("Error - no attack type selected");
				break;
			case 1:
				print("Peow!");
				_anim.SetBool("Attack", true);
				_muzzleFlash._active = true;
				break;
			case 2:
				print("Chop!");
				_anim.SetBool("Attack", true);
				break;
			default:
				print("Error - no attack type selected");
				break;
		}				
		yield return new WaitForSeconds(0.75f);		
		_calcDam.Damage();
		_enemyAnim.SetBool("Hit", true);		
		yield return new WaitForSeconds(0.75f);		
		_anim.SetBool("Attack", false);
		_enemyAnim.SetBool("Hit", false);
		//Wait for animations to finish && re-enable input
		yield return new WaitForSeconds(0.5f);
		_attacking = false;	
	}

	void Rotate(Transform target){
		Vector3 targetDir = transform.position - target.position;
		targetDir.y = 0;
		float step = 15.0f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(target.forward, targetDir, step, 0.0F);
		target.transform.rotation = Quaternion.LookRotation(newDir);
	}

	void RotateTowards(){		
		if (_attacking || _underAttack){
			Vector3 targetDir = _targetPos - transform.position;
			targetDir.y = 0;
			float step = 15.0f * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}
	public void Health(){
		if (_CombatManager._currentHealth <= 0 && !_dead){
			_anim.SetBool("Dead", true);
			_dead = true;
			_combatOver._playerWin = false;
			StartCoroutine(ChangeCanvas());
		}
	}

	IEnumerator ChangeCanvas(){
		yield return new WaitForSeconds(3.0f);
		_combatOver.EnableEndCanvas();
	}

	public void EnableCanvas(){
		_playerTurnUI.SetActive(true);
	}
}
