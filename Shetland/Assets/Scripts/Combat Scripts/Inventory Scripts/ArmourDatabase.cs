using UnityEngine;
using System.Collections.Generic;

public class ArmourDatabase: MonoBehaviour{

	public List<Armour> _headDatabase = new List<Armour>();
	public List<Armour> _chestDatabase = new List<Armour>();
	public List<Armour> _legDatabase = new List<Armour>();

	void Awake(){

		_headDatabase.Add(new Armour(300, "None", 0));
		_headDatabase.Add(new Armour(301, "Wooden Helm", 2));
		_headDatabase.Add(new Armour(302, "Iron Helm", 4));
		_headDatabase.Add(new Armour(303, "Steel Helm", 6));
		_headDatabase.Add(new Armour(304, "Diamond Helm", 8));

		_chestDatabase.Add(new Armour(400, "None", 0));
		_chestDatabase.Add(new Armour(401, "Wood Plate", 4));
		_chestDatabase.Add(new Armour(402, "Iron Plate", 8));
		_chestDatabase.Add(new Armour(403, "Steel Plate", 12));
		_chestDatabase.Add(new Armour(404, "Diamond Plate", 16));

		_legDatabase.Add(new Armour(500, "None", 0));
		_legDatabase.Add(new Armour(501, "Wood Boots", 2));
		_legDatabase.Add(new Armour(502, "Iron Boots", 4));
		_legDatabase.Add(new Armour(503, "Steel Boots", 6));
		_legDatabase.Add(new Armour(504, "Diamond Boots", 8));
	}
}
