using UnityEngine;
using System.Collections.Generic;

public class WeaponDatabase: MonoBehaviour{

	public List<Weapon> _meleeDatabase = new List<Weapon>();
	public List<Weapon> _rangedDatabase = new List<Weapon>();

	void Awake(){

		_meleeDatabase.Add(new Weapon(100, "Wooden Sword", 0.7f, 15, 1, 0));
		_meleeDatabase.Add(new Weapon(101, "Iron Sword", 0.8f, 25, 1, 0));
		_meleeDatabase.Add(new Weapon(102, "Steel Sword", 0.9f, 40, 1, 0));
		_meleeDatabase.Add(new Weapon(103, "Diamond Sword", 1.0f, 60, 1, 0));

		_rangedDatabase.Add(new Weapon(200, "Broken rifle", 0.9f, 85, 1, 20));
		_rangedDatabase.Add(new Weapon(201, "Rifle", 1.0f, 145, 1, 20));
		_rangedDatabase.Add(new Weapon(202, "Long rifle", 1.1f, 205, 1, 40));
		_rangedDatabase.Add(new Weapon(203, "Plasma rifle", 10f, 1000, 1, 30));
	}
}
