using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

public class Censor : MonoBehaviour {

	private IList<string> CensoredWords;
	private IList<string> OKWords;

	void Awake() {
		TextAsset badWordsRaw = Resources.Load("profanity-blacklist") as TextAsset;
		TextAsset goodWordsRaw = Resources.Load("profanity-whitelist") as TextAsset;
		CensoredWords = new List<string>(Regex.Split(badWordsRaw.ToString(), "\r\n|\r|\n"));
		OKWords = new List<string>(Regex.Split(goodWordsRaw.ToString(), "\r\n|\r|\n"));
	}
		
	//known problem: fails for any words like "tacokitty" (cok). Just add them to the whitelist.
	//known problem: fails for words like "assass" because it is the combination of two real words as + sass. If this is an issue, remove "sass" from the whitelist.
	public string CensorText(string text)  
	{
		text = checkThisText (text); // first if the word outright contains bad words, filter those.


		text = checkNumbers(checkForLeet(text)); // Next look at the possibility of numbers
				
		return checkForReverses(text); // Recheck the word in revers.
	} 

	private string checkThisText(string text) {

		foreach (string censoredWord in CensoredWords) {
			if (text.ToLower ().Contains (censoredWord)) {

				//Debug.Log ("found " + censoredWord);
			
				//determine if the word contains something on the whitelist
				List<string> goodWordsFound = new List<string> ();
				foreach (string OKWord in OKWords) {
					if (text.Contains (OKWord)) {
						goodWordsFound.Add (OKWord);
						//Debug.Log ("ok word: " + OKWord);
					}
				}
			
				//the above filter could be circumvented with words like GrassyAss, so filter the remainder recursively
				if (goodWordsFound.Count > 0) {
					string possiblyFiltered;
					string reconstructed = text;
					foreach (string word in goodWordsFound) { 
						int whereToPutItBack = reconstructed.IndexOf (word);
						//Debug.Log ("index where to put back " + whereToPutItBack);
						possiblyFiltered = CensorText (text.Replace (word, ""));
						//Debug.Log ("possibly filtered " + possiblyFiltered);
						reconstructed = possiblyFiltered.Insert (whereToPutItBack, word);
					}
					text = reconstructed;
					continue;
				} else { //otherwise star it out.
					string stars = "";
					for (int i = 0; i < censoredWord.Length; i++) {
						stars += "*";
					}
					text = Regex.Replace(text,censoredWord,stars,RegexOptions.IgnoreCase);
				}
			}

		}
		return text;
	}

	private string checkNumbers (string input)
	{
		//There are three possibilities. 
		// 1) They put random numbers inside of a bad word to try and break it up.
		// 2) They used leet / 1337 to substitute numbers for letters. 
		// 3) They just put honest numbers at the end of a word or something. The initial case already looks for this. 
		return checkForInterspersedNumbers(checkForLeet(input));
	}

	private string checkForInterspersedNumbers (string input)
	{
		string withoutNumbers = Regex.Replace(input, "[0-9]", "");
		string withoutNumbersChecked = checkThisText(withoutNumbers);
		if (withoutNumbersChecked.Contains ("*")) {
			return withoutNumbersChecked;
		}
		else {
			return input;
		}
	}

	private string checkForLeet (string input)
	{
		string leet = input.Replace ("0", "o").Replace ("1", "i").Replace ("3", "e")
			.Replace ("4", "a").Replace ("5", "s").Replace ("7", "t")
			.Replace ("8", "b").Replace ("9", "g"); //This replaces 13457890 with ieastbgo;
		string leet2 = input.Replace ("0", "o").Replace ("1", "l").Replace ("3", "e")
			.Replace ("4", "a").Replace ("5", "s").Replace ("7", "t")
			.Replace ("8", "b").Replace ("9", "g"); //This replaces 13457890 with leastbgo;
		string leetChecked = checkThisText (leet);
		string leetChecked2 = checkThisText (leet2);
		if (leetChecked.Contains ("*")) {
			return leetChecked;
		} else if (leetChecked2.Contains ("*")) {
			return leetChecked2;
		} else {
			return input;
		}
	}

	private string checkForReverses(string input)
	{
		string reverseChecked = checkThisText (Reverse (input));

		reverseChecked = checkNumbers(reverseChecked); // also check for number issues. 
			
		return checkThisText(Reverse(reverseChecked)); // now after re-reversing it let's just check one last time. 
	}

	private string Reverse( string s )
	{
		char[] charArray = s.ToCharArray();
		System.Array.Reverse( charArray );
		return new string( charArray );
	}
}