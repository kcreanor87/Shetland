using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Highscores : MonoBehaviour {

	public int _score;
	public GameObject _newGame;
	public Text _scoreText;

	void Awake(){
		_newGame = GameObject.Find("NewGame");
		_score = NewGame._score;
		_scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		_scoreText.text = "Score: " + _score;
	}

	public void MainMenu(){
		Destroy(_newGame);
		SceneManager.LoadScene(0);
	}
}
