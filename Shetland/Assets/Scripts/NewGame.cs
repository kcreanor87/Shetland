using UnityEngine;

public class NewGame : MonoBehaviour {

	public static bool _newGame;
	public static int _score;
	
	void Start(){
		DontDestroyOnLoad(gameObject);
	}
}
