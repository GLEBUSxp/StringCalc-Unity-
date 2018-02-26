using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour {

	//Операция
	abstract class Operation{
		public abstract float Eval ();
	}

	//Число
	class Number:Operation{
		private float value;
		public Number (float f) {
			value = f;
		}
		public override float Eval(){
			return value;
		}
	}

	//Один операнд
	abstract class Unary:Operation{
		protected Operation one;
		public Unary(Operation op){
			one=op;
		}
	}

	//Унарный минус
	class Negative:Unary{
		public Negative (Operation n): base(n){}
		public override float Eval(){
			return -one.Eval ();
		}
	}

	//Два операнда
	abstract class Binary:Operation{
		protected Operation left, right;
		public Binary(Operation l, Operation r){
			left = l; right = r;
		}
	}
		
	//Сложение
	class Summation : Binary{
		public Summation(Operation l, Operation r) : base(l, r){}
		public override float Eval(){
			return left.Eval () + right.Eval();
		}
	}

	//Вычитание
	class Subtraction:Binary{
		public Subtraction(Operation l, Operation r) : base(l, r){}
		public override float Eval(){
			return left.Eval () - right.Eval();
		}
	}

	//Деление
	class Divide:Binary{
		public Divide(Operation l, Operation r):base(l, r){}
		public override float Eval(){
			if (right.Eval () == 0.0f) {
				ErrorExeption.customExeption ("Error: Attempt to divide by 0");
				throw new ArgumentException ("Attempt to divide by 0");
			}
			return (right.Eval () != 0.0f) ? (left.Eval()/right.Eval ()):float.MaxValue;
		}
	}

	//Умножение
	class Multiple:Binary{
		public Multiple(Operation l, Operation r):base(l, r){}
		public override float Eval(){
			return left.Eval () * right.Eval ();
		}
	}

	public class Expretion{
		private string sourceStr;
		private int position;

		public Expretion (string str){
			sourceStr = str;
		}

		public float calculate(){
			position = 0;
			Operation root = parseLow ();
			return (root != null) ? root.Eval () : 0.0f;
		}

		//Выражения с низким приоритетом (сложение, вычитание)
		private Operation parseLow(){
			Operation result = parseMedium ();
			while (true) {
				if (Match ('+')) 
					result = new Summation (result, parseMedium ());
				else 
					if (Match ('-')) 
						result = new Subtraction (result, parseMedium ());
				else return result;
			}
		}

		//Выражения со средним приоритетом (умножение, деление)
		private Operation parseMedium(){
			Operation result = parseHight ();
			while (true) {
				if (Match ('*'))
					result = new Multiple (result, parseHight ());
				else if (Match ('/'))
					result = new Divide (result, parseHight ());
				else
					return result;
			}
		}

		//Выражения с высоким приоритетом (унарный минус, скобки, число)
		private Operation parseHight(){
			Operation result = null;
			if (Match ('-'))
				result = new Negative (parseNumber());
			else if (Match ('(')) {
				result = parseLow ();
				if (!Match (')'))
					throw new ArgumentException("Missing ')'");
			}
			else 
				result = parseNumber ();
			return result;
		}

		//Парсинг числа
		private Operation parseNumber(){
			Operation result = null;
			float val = 0.0f;
			int start = position;
			while (position < sourceStr.Length && (char.IsDigit (sourceStr [position]) || sourceStr [position] == '.' || sourceStr [position] == 'e'))
				++position;

			try{
				val = float.Parse(sourceStr.Substring(start, position - start));
			} catch{
				ErrorExeption.customExeption("Can't parse '" + sourceStr.Substring(start));
				throw new ArgumentException ("Can't parse '" + sourceStr.Substring(start));
			}
			result = new Number (val);
			return result;
		}

		//Поиск символа в строке
		private bool Match (char c){
			if (position >= sourceStr.Length)
				return false;

			//Пропуск пробелов
			while (sourceStr [position] == ' ') {
				++position;
			}

			//Искомый символ найден
			if (sourceStr [position] == c) {
				++position; 
				return true;
			} else
				return false;
		}


		//проверка выражения
		public bool checkExpression(){
			string enabledChars = "0123456789+-/*.()";

			for (int i=0; i<sourceStr.Length; i++){
				if(!enabledChars.Contains(sourceStr[i].ToString())){
					ErrorExeption.checkExeption(sourceStr[i], i);
					return false;
				}
			}
			return checkBrackets(sourceStr);
		}

		//Проверка корректности скобок
		private bool checkBrackets (string sourceStr){

			Stack<int> posOfBrackets = new Stack<int>();
			Stack<char> brackets = new Stack<char>();

			for (int i = 0; i < sourceStr.Length; i++) {
				if (sourceStr [i] == '('){
					brackets.Push ('(');
					posOfBrackets.Push (i);
				}
				if(sourceStr[i] == ')'){
					try{
						brackets.Pop ();	
						posOfBrackets.Pop ();
					} catch{
						ErrorExeption.checkExeption(sourceStr[i], i);
						return false;
					}
				}
			}
			if (brackets.Count == 0)
				return checkOperations(sourceStr);
			else {
				ErrorExeption.checkExeption(brackets.Pop(), posOfBrackets.Pop ());
				return false;
			}

		}

		//Проверка операций
		private bool checkOperations(string sourceStr){
			string charsOfOperation = "+-/*.";
			for (int i=0; i<sourceStr.Length; i++){
				if(charsOfOperation.Contains(sourceStr[i].ToString()) && charsOfOperation.Contains(sourceStr[i+1].ToString())){
					ErrorExeption.checkExeption(sourceStr[i], i);
					return false;
				}
			}
			return true;
		}


	}
}