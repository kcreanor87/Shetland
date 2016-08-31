using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMP : MonoBehaviour {

	public void LoadLevel(int level){
		NewGame._newGame = false;
		SceneManager.LoadScene(level);
	}
}
