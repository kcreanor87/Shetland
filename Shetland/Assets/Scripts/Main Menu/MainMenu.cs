using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject _promptGO;
	public GameObject _introButtonts;

	void Start(){
		_promptGO = GameObject.Find("NewGamePrompt");
		_introButtonts = GameObject.Find("IntroButtons");
		_promptGO.SetActive(false);
	}

	public void ResumeGame(){
		SceneManager.LoadScene("World Map");
		NewGame._newGame = false;
	}

	public void NewGamePrompt(){
		_promptGO.SetActive(true);
		_introButtonts.SetActive(false);
	}

	public void CancelNewGame(){
		_promptGO.SetActive(false);
		_introButtonts.SetActive(true);
	}

	public void StartNewGame(){
		NewGame._newGame = true;
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene("CharCreation");		
	}

	public void ExitGame(){
		Application.Quit();
	}
}
