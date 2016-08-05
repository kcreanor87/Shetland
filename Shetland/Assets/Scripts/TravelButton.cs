using UnityEngine;

public class TravelButton : MonoBehaviour {

	public Vector3 _destination;
	public Transform _player;
	public TownCanvas _townCanvas;
	public NavMeshAgent _playerNav;
	public Transform _cam;
	public int _cost;
	public int _time;
	public DayTimer _dayTimer;

	void Start(){
		_cam = GameObject.Find("CameraParent").GetComponent<Transform>();
		_townCanvas = GameObject.Find("TownCanvas").GetComponent<TownCanvas>();
		_player = GameObject.Find("Player").GetComponent<Transform>();
		_playerNav = GameObject.Find("Player").GetComponent<NavMeshAgent>();
		_dayTimer = GameObject.Find("Timer").GetComponent<DayTimer>();
	}

	public void Travel(){
		_playerNav.Stop();
		_playerNav.enabled = false;
		_player.position = _destination;
		_cam.position = _destination;		
		_playerNav.enabled = true;
		_playerNav.SetDestination(_destination);
		_playerNav.Resume();
		_townCanvas.CloseBuilding(3);
		_townCanvas.CloseCanvas();
		_manager._obols -= _cost;
		_dayTimer.AdvanceTime(_time);
	}
}
