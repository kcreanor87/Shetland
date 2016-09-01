using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyWM : MonoBehaviour {

	public NavMeshAgent _agent;
	public PlayerControls_WM _player;
	public SpawnPoint _thisSpawn;	
	public bool _spotted;
	public SaveGame _saveGame;

	void Start(){		
		_saveGame = GameObject.Find("Loader").GetComponent<SaveGame>();
		_agent = gameObject.GetComponent<NavMeshAgent>();
		_player = GameObject.Find("Player").GetComponent<PlayerControls_WM>();

	}

	public void ChasePlayer(){
		print("Player Spotted");
		_spotted = true;
		_agent.enabled = true;
		StartCoroutine(ChaseLoop(0.5f));
	}

	public IEnumerator ChaseLoop(float looptime){
		while (_spotted){
			_agent.SetDestination(_player.transform.position);
			yield return new WaitForSeconds(looptime);
		}		
	}

	public void AtPlayer(){
		_spotted = false;
		_agent.SetDestination(transform.position);
		_player._agent.SetDestination(_player.transform.position);
		_player.enabled = false;
		_thisSpawn._fought = true;
		print("Combat!");
		_saveGame.Save();
		SceneManager.LoadScene("Combat");
	}
}
