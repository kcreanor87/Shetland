[System.Serializable]
public class BuildingCost {

	public string _name;
	public int _woodCost;
	public int _stoneCost;
	public int _ironCost;
	public int _coalCost;

	public void PriceCost(string name, int wood, int stone, int iron, int coal){
		_name = name;
		_woodCost = wood;
		_stoneCost = stone;
		_ironCost = iron;
		_coalCost = coal;
	}
}
