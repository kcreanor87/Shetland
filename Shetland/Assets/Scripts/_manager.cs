using UnityEngine;
using System.Collections.Generic;

public class _manager : MonoBehaviour {

	public static List <int> _resources = new List <int>();
	public static int _obols = 10;
	public static List <string> _resourceNames = new List<string>();

	public static List <int> _factoryOuput = new List<int>();

	public static float _buyModifier = 1.2f;
	public static float _sellModifier = 0.8f;

	public float _helloTest;

	void Start(){
		_resourceNames.Add("Wood");
		_resourceNames.Add("Stone");
		_resourceNames.Add("Iron");
		_resourceNames.Add("Coal");

		_factoryOuput.Add(0);
		_factoryOuput.Add(0);
		_factoryOuput.Add(0);
		_factoryOuput.Add(0);

		_resources.Add(1);
		_resources.Add(0);
		_resources.Add(0);
		_resources.Add(0);

		_helloTest = 1;
	}	
}
