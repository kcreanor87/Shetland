using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public TurnManager _tM;
	public PlayerTurn _pT;
	public MuzzleFlash _muzzleFlash;
	public CalculateDamage _calcDam;	
	public CombatStats _stats;	
	public CameraController _cam;
	
	public NavMeshAgent _agent;
	public NavMeshObstacle _obstacle;
	public LayerMask _mask = 1 << 9;

	public int _actionsRemaining;
	public int _maxHP;
	public int _currentHP;
	public int _def;

	public float _moveSpeed;
	public float _range;
	public float _dodge;
	public float _distance;
	public float _distanceTravelled;

	public Animator _anim;
	public Animator _playerAnim;

	public bool _attacking;
	public bool _rotating;
	public bool _dead;	
	public bool _active;
	
	public Renderer _renderer;	
	public GameObject _midPoint;
	public RectTransform _hpRect;
	public GameObject _hpBarGO;
	public Text _damageText;
	public Transform _player;
	public Transform _meshTrans;


	void Start(){
		_hpBarGO = transform.Find("HealthBar/Background").gameObject;
		_damageText = transform.Find("HealthBar/DamageText").GetComponent<Text>();
		_damageText.enabled = false;
		_hpRect = transform.Find("HealthBar/Background/HP").GetComponent<RectTransform>();
		_hpBarGO.SetActive(false);
		_muzzleFlash = gameObject.GetComponentInChildren<MuzzleFlash>();
		_anim = gameObject.GetComponentInChildren<Animator>();
		_mask = ~_mask;
		_midPoint = GameObject.Find("MidPoint");
		_cam = GameObject.Find("_cameraParent").GetComponent<CameraController>();
		_tM = GameObject.Find("_combatMan").GetComponent<TurnManager>();
		_calcDam = GameObject.Find("_combatMan").GetComponent<CalculateDamage>();
		_player = GameObject.Find("Player").GetComponent<Transform>();
		_pT = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_playerAnim = _player.GetComponent<Animator>();
		_agent = transform.GetComponent<NavMeshAgent>();
		_obstacle = transform.GetComponent<NavMeshObstacle>();
		transform.LookAt(_player);
		_currentHP = _maxHP;
		_meshTrans = gameObject.transform.FindChild("EnemyMesh").GetComponent<Transform>();
		_renderer = _meshTrans.FindChild("Body").GetComponent<Renderer>();
	}

	void Update(){
		if (!_tM._waitingForSeconds && _active){
			if (!_dead){
				RotateTowards();
				if(_range > 0){					
					EnemyRangedTurn();
				}
				else{
					EnemyMeleeTurn();
				}
			}
			else{
				NextTurn();
			}
		}
	}
	public void Initialize(){
		_actionsRemaining = 2;
		_range = _tM._activeCombatant._range;
		_moveSpeed = _tM._activeCombatant._moveSpeed;
		_active = true;
		_agent.Resume();
		if (_range < 1){
			_anim.SetBool("WeaponB", true);
		}
	}

	void EnemyMeleeTurn(){
		_distance = Vector3.Distance(transform.position, _player.position);
		if (_actionsRemaining > 1){
			if (_distance > (_moveSpeed*4) || _distanceTravelled > 0){
				//move 40.0f but stop, end turn
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
				_distanceTravelled += Time.deltaTime*_moveSpeed;
				if (_distanceTravelled > (_moveSpeed*4)){
					_agent.SetDestination(transform.position);
					_anim.SetBool("Running", false);
					_tM.NextTurn();
					_distanceTravelled = 0;
					print("Double move - not at player");
				}
			}
			else if (_distance > (_moveSpeed*2)){
				//move to player and stop, end turn
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
				_actionsRemaining -= 2;
				print("Double move - at player");
			}
			else if (_distance > 2.5f){
				//move once, attack, end turn
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
				print("Single move");
				_actionsRemaining--;
			}
			else{
				_agent.SetDestination(transform.position);
				_anim.SetBool("Running", false);
				EnemyAttack(false);
			}
		}
		else if (_actionsRemaining == 1){
			if (_distance <= 2.5f){
				_agent.SetDestination(transform.position);
				_anim.SetBool("Running", false);
				EnemyAttack(false);
			}
		}
		else{
			if (_distance <= 3.0f){
				_agent.SetDestination(transform.position);
				_anim.SetBool("Running", false);
				NextTurn();
			}
		}
	}

	void EnemyRangedTurn(){
		_distance = Vector3.Distance(transform.position, _player.position);
		if (_actionsRemaining > 1){
			if (_distance > (_moveSpeed*2)+_range || _distanceTravelled > 0){
				//move but stop short, end turn
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
				_distanceTravelled += Time.deltaTime*_moveSpeed;
				if (_distanceTravelled > (_moveSpeed*6)){
					_agent.SetDestination(transform.position);
					_anim.SetBool("Running", false);
					NextTurn();
					_distanceTravelled = 0;
					print("Double move - still not in range");
				}
				else if (_distance <= (_range - 5)){
					_agent.SetDestination(transform.position);
					_anim.SetBool("Running", false);
					NextTurn();
					_distanceTravelled = 0;
					print("Double move - within range");
				}
			}
			else if (_distance > (_range)){
				//Move Towards player until within range, then shoot
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
				_actionsRemaining--;
				print("Moving and Shoot");
			}
			else{
				if (!Physics.Linecast(transform.position, _player.position, _mask.value)){
					//Shoot Twice
					_agent.SetDestination(transform.position);
					_anim.SetBool("Running", false);
					EnemyAttack(true);
				}
				else{
					_agent.SetDestination(_player.position);
					_anim.SetBool("Running", true);
					_actionsRemaining--;
					print("Can't See Player");
				}
			}
		}
		else if (_actionsRemaining == 1){
			if (_distance <= (_range) && !Physics.Linecast(transform.position, _player.position, _mask.value)){
				_agent.SetDestination(transform.position);
				_anim.SetBool("Running", false);
				EnemyAttack(true);
			}
			else if (_distance > _range){
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
			}
			else if (Physics.Linecast(transform.position, _player.position, _mask.value)){
				_agent.SetDestination(_player.position);
				_anim.SetBool("Running", true);
				print("Moving to see player");
			}
			else{
				_agent.SetDestination(transform.position);
				_anim.SetBool("Running", false);
				EnemyAttack(true);
				print("Player in sight");
			}
		}
		else{
			NextTurn();
		}
	}

	void EnemyAttack(bool ranged){
		if (ranged && !_attacking){
			print("Ranged Attack");
			StartCoroutine(Attack(true));
		}
		else if (!ranged && !_attacking){
			print("Melee Attack");
			StartCoroutine(Attack(false));
		}
	}

	IEnumerator Attack(bool ranged){
		_attacking = true;
		_muzzleFlash._active |= (_range > 1);
		_pT._targetPos = transform.position;
		PlayerTurn._underAttack = true;
		_rotating = true;
		_anim.SetBool("Attack", true);
		var x = transform.position.x + (_player.position.x - transform.position.x)/2;
		var z = transform.position.z + (_player.position.z - transform.position.z)/2;
		_midPoint.transform.position = new Vector3(x, 0, z);
		if (ranged) _cam.EnableShotCam();
		yield return new WaitForSeconds(0.88f);
		_calcDam.CalculateChance();
		_calcDam.Damage();
		_playerAnim.SetBool("Hit", true);
		yield return new WaitForSeconds(0.32f);
		_anim.SetBool("Attack", false);
		_playerAnim.SetBool("Hit", false);
		yield return new WaitForSeconds(1.0f);
		_attacking = false;
		_rotating = false;
		PlayerTurn._underAttack = false;
		_actionsRemaining--;
	}

	void RotateTowards(){
		if (_rotating){
			Vector3 targetDir = _player.position - transform.position;
			float step = (15.0f * Time.deltaTime);
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	void NextTurn(){
		if (!_attacking){
			_tM.NextTurn();
		}
	}

	public void BeenHit(){
		_damageText.enabled = true;
		if (_calcDam._totalDamage > 0){
			_damageText.text = "-" + _calcDam._totalDamage;
			_hpBarGO.SetActive(true);
		}
		else{
			_damageText.text = "Miss";
		}
		StartCoroutine(DisableText());
	}

	IEnumerator DisableText(){
		yield return new WaitForSeconds(1.5f);
		_damageText.enabled = false;
	}

	public void Health(){
		float health = (float) _currentHP/_maxHP;
		_hpRect.sizeDelta = new Vector2(680*health, 7);
		if (_currentHP <= 0){
			_anim.SetBool("Dead", true);
			_tM._activeEnemies--;
			_dead = true;
			gameObject.tag = "Untagged";
			_hpBarGO.SetActive(false);
		}
	}
	public void OnMouseOver(){
		if (_pT._attackToggle){
			if(!_dead){
				_renderer.material.EnableKeyword("_EMISSION");
				const float floor = 0.8f;
				float emission = Mathf.PingPong(Time.time*0.4f, 1.0f - floor);
				Color baseColor = Color.red;
				Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
				_renderer.material.SetColor("_EmissionColor", finalColor);
			}			
		}	
		else{
			_renderer.material.SetColor("_EmissionColor", Color.black);
		}	
	}

	public void OnMouseExit(){ 
		_renderer.material.SetColor("_EmissionColor", Color.black);
	}
}
