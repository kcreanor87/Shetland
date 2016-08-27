using UnityEngine;
using UnityEngine.UI;

public class WM_UI : MonoBehaviour {

	public static Text _woodTxt, _stoneTxt, _ironTxt, _coalTxt, _steelTxt, _diamondTxt, _obolTxt;
	public static GameObject _resourcePrompt;
	public static Text _promptText;
	public static ResourceGen _resourceScript;
	public static GameObject _mapToggle;

	// Use this for initialization
	void Start () {
		_mapToggle = GameObject.Find("MapToggle");
		_resourcePrompt = GameObject.Find("ResourcePrompt");
		_promptText = GameObject.Find("PromptText").GetComponent<Text>();
		_resourcePrompt.SetActive(false);
		_woodTxt = GameObject.Find("Wood").GetComponent<Text>();
		_stoneTxt = GameObject.Find("Stone").GetComponent<Text>();
		_ironTxt = GameObject.Find("Iron").GetComponent<Text>();
		_coalTxt = GameObject.Find("Coal").GetComponent<Text>();
		_obolTxt = GameObject.Find("Obols").GetComponent<Text>();
		_steelTxt = GameObject.Find("Steel").GetComponent<Text>();
		_diamondTxt = GameObject.Find("Diamonds").GetComponent<Text>();
		UpdateUI();
	}
	
	public static void UpdateUI(){
		_woodTxt.text = "Wood: " + _manager._resources[0] + "(+" + _manager._factoryOuput[0]+ ")";
		_stoneTxt.text = "Stone: " + _manager._resources[1] + "(+" + _manager._factoryOuput[1]+ ")";
		_ironTxt.text = "Iron: " + _manager._resources[2] + "(+" + _manager._factoryOuput[2] + ")";
		_coalTxt.text = "Coal: " + _manager._resources[3] + "(+" + _manager._factoryOuput[3] + ")";
		_steelTxt.text = "Steel: " + _manager._resources[4];
		_diamondTxt.text = "Diamonds: " + _manager._resources[5];
		_obolTxt.text = "Obols: " + _manager._obols;
	}

	public static void ResourcePrompt(){
		_mapToggle.SetActive(false);
		_resourceScript.AddResource();
		_resourcePrompt.SetActive(true);
		_promptText.text = "+" + _resourceScript._amount + " " + _manager._resourceNames[_resourceScript._type];
		Time.timeScale = 0.0f;
		UpdateUI();
	}

	public void ClosePrompt(){
		_mapToggle.SetActive(true);
		_resourcePrompt.SetActive(false);
		Time.timeScale = 1.0f;
	}
}
