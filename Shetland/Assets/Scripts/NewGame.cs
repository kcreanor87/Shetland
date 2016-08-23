using UnityEngine;

public class NewGame : MonoBehaviour {

	public static bool _newGame;
	
	void Start(){
		DontDestroyOnLoad(gameObject);
	}
}
