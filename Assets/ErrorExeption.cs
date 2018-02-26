using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorExeption : MonoBehaviour {
	
	static public void checkExeption(char c, int pos){
		pos++;
		Main.resultText.text = "Unexpected: '" + c + "' on position: " + pos;
	}

	static public void customExeption(string exep) {
		Main.resultText.text = exep;
	}

}