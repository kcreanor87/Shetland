[System.Serializable]
public class Character {

	public int _str;
	public int _dex;
	public int _vit;
	public int _melee;
	public int _ranged;
	public int _chest;
	public int _head;
	public int _legs;
	public string _name;

	public Character(int str, int dex, int vit, string name, int melee, int ranged, int chest, int head, int legs){
		_str = str;
		_dex = dex;
		_vit = vit;
		_name = name;
		_melee = melee;
		_ranged = ranged;
		_chest = chest;
		_head = head;
		_legs = legs;
	}
}
