using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


[System.Serializable]
public class GoogleData
{
	public string order, result, msg, value;
	public List<List<string>> localData;
	public List<List<string>> searchData;

}

public class GoogleSheetManager : MonoBehaviour
{

	const string URL = "https://script.google.com/macros/s/AKfycbytD52hrz0NZFjRXXimm-rCPasTlvZ_c4wlcTURJwuV5sHLBTRqNzot4IFsn5WTVKY_/exec";
	public GoogleData GD;

	public TMP_InputField IDInput, AddressInput;
	string id, pass, address;

	public TMP_Text NameCheck;
	public Button NextBtn, LocalNextBtn;
	public Image O_image, X_image;

	public Transform LocalList;
	public GameObject itemLocalCard, tooManyResult;

	public PlayfabGuestLoginManager PGLM;

	public GameObject NickNameError;

	


	public void GetLocal()
	{
		LocalNextBtn.interactable = false;
		LocalNextBtn.transform.GetChild(0).gameObject.SetActive(false);
		LocalNextBtn.transform.GetChild(1).gameObject.SetActive(true);
		//?????? ????????
		Transform[] childList = LocalList.GetComponentsInChildren<Transform>();

		if (childList != null)
		{
			for (int i = 1; i < childList.Length; i++)
			{
				if (childList[i] != transform)
				{
					Destroy(childList[i].gameObject);
				}
			}
		}


		address = AddressInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "localSearch");
		form.AddField("address", address);

		StartCoroutine(Post(form));
	}



	bool SetIDPass()
	{
		id = IDInput.text.Trim();
		//pass = PassInput.text.Trim();
		pass = UserData.instance.User_MyID;

		if (id == "" || pass == "")
		{
			return false;
		}
		else if (id.Length < 3 || id.Length > 10)
		{
			return false;
		}
		else
		{
			return true;
		}
	}


	public void EmptyResult()
	{
		NextBtn.transform.GetChild(0).gameObject.SetActive(true);
		NextBtn.transform.GetChild(1).gameObject.SetActive(false);
		NameCheck.text = "";
		NextBtn.interactable = false;
		IDInput.text = Regex.Replace(IDInput.text, @"[^0-9a-zA-Z??-?R]", "");
		O_image.gameObject.SetActive(false);
		X_image.gameObject.SetActive(false);
	}

	public void IDCheck()
	{
		NextBtn.transform.GetChild(0).gameObject.SetActive(false);
		NextBtn.transform.GetChild(1).gameObject.SetActive(true);

		if (!SetIDPass())
		{
			if (id.Length < 3 || id.Length > 10 || id == "")
			{
				NameCheck.color = Color.gray;
				NameCheck.text = "???????? 3???? ????, 10???? ?????????? ??????.";
				//print("???????? 3???? ????, 10???? ?????????? ??????.");
				NextBtn.interactable = false;
				NextBtn.transform.GetChild(0).gameObject.SetActive(true);
				NextBtn.transform.GetChild(1).gameObject.SetActive(false);
				O_image.gameObject.SetActive(false);
				X_image.gameObject.SetActive(true);
			}

			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("order", "idCheck");
		form.AddField("id", id);

		StartCoroutine(Post(form));
	}


	

	public void Register()
	{
		if (!SetIDPass())
		{
			if (id.Length < 3 || id.Length > 10 || id == "")
			{
				print("???????? 3???? ????, 10???? ?????????? ??????.");
			}

			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("order", "register");
		form.AddField("id", id);
		form.AddField("pass", pass); // pass = Playfab ID
		form.AddField("local", UserData.instance.User_Local); // petid
		form.AddField("petid", UserData.instance.Dog1_PevoNumber); // petid
		form.AddField("dogstyle", UserData.instance.Dog1_Style); // petid
		form.AddField("dog1name", UserData.instance.Dog1_Name); // petid

		StartCoroutine(Post(form));
	}


	public void Login()
	{
		if (!SetIDPass())
		{
			print("?????? ???? ?????????? ????????????");
			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("order", "login");
		form.AddField("id", id);
		form.AddField("pass", pass);

		StartCoroutine(Post(form));
	}


	void OnApplicationQuit()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "logout");

		StartCoroutine(Post(form));
	}


	public void SetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "setValue");
		//form.AddField("value", ValueInput.text);

		StartCoroutine(Post(form));
	}


	public void GetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "getValue");

		StartCoroutine(Post(form));
	}




	IEnumerator Post(WWWForm form)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // ?????? using?? ????????
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("???? ?????? ????????.");
		}
	}


	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;

		print(json);

		GD = JsonConvert.DeserializeObject<GoogleData>(json);
		//GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.result == "Found")
		{
			LocalNextBtn.transform.GetChild(0).gameObject.SetActive(true);
			LocalNextBtn.transform.GetChild(1).gameObject.SetActive(false);

			if (GD.localData.Count < 200)
            {
				
				//print(GD.localData.Count);
				for (int i = 0; i < GD.localData.Count; i++)
				{
					GameObject itemCard = Instantiate(itemLocalCard, LocalList);
					string ResultAddress = GD.localData[i][0] + " " + GD.localData[i][1] + " " + GD.localData[i][2] + " " + GD.localData[i][3] + " " + GD.localData[i][4];

					itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = ResultAddress;

				}
			}else
            {
				GameObject itemCard = Instantiate(tooManyResult, LocalList);
			}
			

			return;
		}


		if (GD.order == "register")
		{
			if (GD.result == "ERROR")
			{
				NickNameError.SetActive(true);
				//???? ???????? DB?????? ???????????? ???? ??????????
				//TODO : ???????? ???????????? ?????????? ???????? ???????????????? ????.

			}
			else
			{
				PGLM.MakeNewSlot00();
				//????????????
			}
		}

		if (GD.order == "idCheck")
		{
			if (GD.result == "ERROR")
			{
				//print(GD.order + "?? ?????? ?? ????????. ???? ?????? : " + GD.msg);
				NameCheck.color = new Color(255f / 255f, 62f / 255f, 62f / 255f);
				NameCheck.text = GD.msg;
				NextBtn.interactable = false;
				NextBtn.transform.GetChild(0).gameObject.SetActive(true);
				NextBtn.transform.GetChild(1).gameObject.SetActive(false);
				O_image.gameObject.SetActive(false);
				X_image.gameObject.SetActive(true);
				return;
			}
			else
			{
				//print(GD.order + "?? ????????????. ?????? : " + GD.msg);
				NameCheck.color = new Color(73f / 255f, 115f / 255f, 207f / 255f);
				NameCheck.text = GD.msg;
				NextBtn.transform.GetChild(0).gameObject.SetActive(true);
				NextBtn.transform.GetChild(1).gameObject.SetActive(false);
				NextBtn.interactable = true;
				O_image.gameObject.SetActive(true);
				X_image.gameObject.SetActive(false);
				UserData.instance.User_Name = IDInput.text;
			}
		}


		/*
		if (GD.order == "getValue")
		{
			ValueInput.text = GD.value;
		}
		*/
	}

	public void NickNameEdit()
	{
		NickNameError.SetActive(false);
		PGLM.CharacterChoiceClose();
		PGLM.PetCardClose();
		PGLM.PevoAppClose();
		PGLM.PevoCheckClose();
		PGLM.LocalCheckClose();
		EmptyResult();
	}

	public void DownloadPevoApp()
	{
		string AppUrl;
#if UNITY_EDITOR
		AppUrl = "https://play.google.com/store/apps/details?id=com.ddcares.pevo";
#elif UNITY_IPHONE
        AppUrl = "https://apps.apple.com/kr/app/%ED%8E%98%EB%B3%B4/id1502216647";
#elif UNITY_ANDROID
        AppUrl = "https://play.google.com/store/apps/details?id=com.ddcares.pevo";
#endif
		Application.OpenURL(AppUrl);
	}


	public void LnikPetRgtNo()
	{
		string PetRgtNoUrl = "https://cbh.bemypet.kr/";
		Application.OpenURL(PetRgtNoUrl);
	}
}
