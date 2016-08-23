using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void ResumeGame(){

		SceneManager.LoadScene(1);
		NewGame._newGame = false;
	}

	public void StartNewGame(){
		NewGame._newGame = true;
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene(1);		
	}

	public void ExitGame(){
		Application.Quit();
	}
}
