[System.Serializable]
public class Weapon {

	public int _id;
	public string _name;
	public float _attRating;
	public int _dam;
	public int _minStr;
	public int _range;


	public Weapon(int id, string name, float att, int dam, int minStr, int range){
		_id = id;
		_name = name;
		_attRating = att;
		_dam = dam;
		_minStr = minStr;
		_range = range;
	}
}
