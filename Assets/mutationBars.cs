using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mutationBars : MonoBehaviour {
	
	public Image amoxBar;
	public Image methBar;
	public Image vancBar;
	public Image carbBar;
	public Image lineBar;
	public Image rifaBar;
	public Image isonBar;
	private __app appScript;
	
	void Start () {
		appScript = GameObject.Find("__app").GetComponent<__app>();
		appScript.giveMutationBarsScript(this);
	}

	public void updateBars() {
		amoxBar.fillAmount = __app.mutationChances["amox"];
		methBar.fillAmount = __app.mutationChances["meth"];
		vancBar.fillAmount = __app.mutationChances["vanc"];
		carbBar.fillAmount = __app.mutationChances["carb"];
		lineBar.fillAmount = __app.mutationChances["line"];
		rifaBar.fillAmount = __app.mutationChances["rifa"];
		isonBar.fillAmount = __app.mutationChances["ison"];
	}
}
