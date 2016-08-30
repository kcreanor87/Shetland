using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TEMP : MonoBehaviour {

	public void LoadLevel(int level){
		SceneManager.LoadScene(level);
	}
}
