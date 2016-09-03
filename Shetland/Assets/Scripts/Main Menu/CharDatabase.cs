using UnityEngine;
using System.Collections.Generic;

public class CharDatabase : MonoBehaviour {

	public List <Character> _chars = new List <Character>();

	// Use this for initialization
	void Awake(){
		_chars.Add(new Character(3, 6, 6, "Richmond", 1, 1, 0, 0, 0));
		_chars.Add(new Character(4, 4, 4, "Liz", 0, 0, 2, 2, 2));
		_chars.Add(new Character(7, 2, 6, "Gunther", 3, 0, 0, 0, 0));
	}
}
