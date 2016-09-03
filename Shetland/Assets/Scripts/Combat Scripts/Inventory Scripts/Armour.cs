[System.Serializable]
public class Armour {

	public int _id;
	public string _name;
	public int _armourBonus;

	public Armour(int id, string name, int armBonus){
		_id = id;
		_name = name;
		_armourBonus = armBonus;
	}
}
