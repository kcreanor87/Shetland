using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	public AuxilliaryPower _auxPower;
	public CombatUI _combatUI;
		
	public GameObject _toolTipGO;
	public RectTransform _tooltipRect;
	public PlayerTurn _pT;

	public Text _toolTipName;
	public Text _toolTipDesc;
	public Text _toolTipAP;

	public Vector3 _upperPos;
	public Vector3 _lowerPos;
	public Vector3 _retreatPos;

	// Use this for initialization
	void Start () {
		_auxPower = gameObject.GetComponent<AuxilliaryPower>();
		_combatUI = gameObject.GetComponent<CombatUI>();
		_pT = GameObject.Find("Player").GetComponent<PlayerTurn>();
		_toolTipName = GameObject.Find("TooltipName").GetComponent<Text>();
		_toolTipDesc = GameObject.Find("TooltipDesc").GetComponent<Text>();
		_toolTipAP = GameObject.Find("TooltipAP").GetComponent<Text>();
		_toolTipGO = GameObject.Find("TooltipGO");
		_tooltipRect = _toolTipGO.GetComponent<RectTransform>();
		_tooltipRect.anchoredPosition = _lowerPos;
		_toolTipGO.SetActive(false);
	}
	
	// Update is called once per frame
	public void EnableToolTip(int i){
		_toolTipGO.SetActive(true);
		_tooltipRect.anchoredPosition = _pT._attackToggle ? _upperPos : _lowerPos;
		UpdateAP(0);
		switch(i){
			//Mouse is over Retreat Button
			case 0:
			_toolTipName.text = "Retreat";
			_toolTipDesc.text = "Run away from combat and \n return to the previous town";
			_toolTipAP.text = null;
			_tooltipRect.anchoredPosition = _retreatPos;
			break;
			case 1:
			_toolTipName.text = "Move";
			_toolTipDesc.text = "Click to move to location";
			_toolTipAP.text = null;
			break;
			//Mouse is over Auxilliary Button
			case 2:
			_toolTipName.text = _auxPower._auxName;
			_toolTipDesc.text = _auxPower._auxDesc;
			_toolTipAP.text = "" + _auxPower._auxAp + " AP";
			UpdateAP(_auxPower._auxAp);
			break;
			case 3:
			_toolTipName.text = "Attack";
			_toolTipDesc.text = "Open Attack Menu";
			_toolTipAP.text = null;
			break;
			case 4:
			_toolTipName.text = "Switch Weapons";
			_toolTipDesc.text = "Click to switch between \nmelee and ranged weapon";
			_toolTipAP.text = "8 AP";
			UpdateAP(8);
			break;
			case 5:
			_toolTipName.text = "Delayed Attack";
			_toolTipDesc.text = "(In Development)";
			_toolTipAP.text = null;
			break;
			case 6:
			_toolTipName.text = "Next Turn";
			_toolTipDesc.text = "End the turn";
			_toolTipAP.text = null;
			break;
			case 7:
			_toolTipName.text = "Snap Shot";
			_toolTipDesc.text = "-20 % Accuracy";
			_toolTipAP.text = "5 AP";
			UpdateAP(5);
			break;
			case 8:
			_toolTipName.text = "Regular Shot";
			_toolTipDesc.text = "No aim penalty";
			_toolTipAP.text = "8 AP";
			UpdateAP(8);
			break;
			case 9:
			_toolTipName.text = "Aimed Shot";
			_toolTipDesc.text = "+ 20% Accuracy";
			_toolTipAP.text = "12 AP";
			UpdateAP(12);			
			break;
			case 10:
			_toolTipName.text = "Skills Menu";
			_toolTipDesc.text = "Select the active skill";
			_toolTipAP.text = null;
			break;

		}
	}
	public void DisableTooltip(){
		_toolTipGO.SetActive(false);
		_tooltipRect.anchoredPosition = _lowerPos;
		_combatUI._apLoss.transform.localScale = new Vector3 (0f,1.0f,1.0f);
	}

	void UpdateAP(int cost){
		float APCost = ((float) cost/_pT._ap) * -1;	
		_combatUI._apImage.sprite = (APCost < -1) ? _combatUI._apNegative : _combatUI._apPositive;
		_combatUI._apLoss.transform.localScale = (APCost <= -1.0f) ? new Vector3 (-1.0f,1.0f,1.0f) : new Vector3 (APCost, 1, 1);
	}	
}
