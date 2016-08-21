using UnityEngine;
using System.Collections.Generic;

public class _manager : MonoBehaviour {

	public static List <int> _resources = new List <int>();
	public static int _obols = 1000;
	public static List <string> _resourceNames = new List<string>();

	public static List <int> _factoryOuput = new List<int>();

	void Start(){
		_resourceNames.Add("Wood");
		_resourceNames.Add("Stone");
		_resourceNames.Add("Iron");
		_resourceNames.Add("Coal");
		_resourceNames.Add("Steel");
		_resourceNames.Add("Diamonds");

		_factoryOuput.Add(0);
		_factoryOuput.Add(0);
		_factoryOuput.Add(0);
		_factoryOuput.Add(0);

		_resources.Add(100);
		_resources.Add(100);
		_resources.Add(100);
		_resources.Add(100);
		_resources.Add(100);
		_resources.Add(100);
	}
}
