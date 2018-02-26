using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	private InputField inputText;
	static public Text resultText;

	void Start(){
		inputText = GameObject.FindGameObjectWithTag ("InputTextField").GetComponent<InputField>();
		resultText = GameObject.FindGameObjectWithTag("ResultTextField").GetComponent<Text>();
	}

	public void calcButton(){
		
		string str = inputText.text;
		Calc.Expretion expr = new Calc.Expretion (str);

		if (expr.checkExpression()) 
			resultText.text = "Result: " + expr.calculate ().ToString ();

	}
		
}