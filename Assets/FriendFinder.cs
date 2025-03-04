using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class FriendFinder : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbxPj95nFXmEU4xcnW29aXR61ZCmmLAPUdrUkmylLePnPV5eY6Eu56y0hAoXS_DGqydK/exec";
    public GoogleData GD;

	public FriendsManager FM;

	public TMP_Dropdown SearchKind;

	public Transform SearchList;

	public TMP_InputField SearchInput;
	string SearchText;

	public TMP_Text SearchStyle;
	public TMP_Text SearchDetail;

	public Transform SearchFriendList;
	public GameObject Item_SearchFriend;
	public GameObject Item_Empty;
	public GameObject Item_RecommandBar;

	public int SearchListCountNumber;
	public Button SearchBtn;
	public GameObject SearchLoading;

    public void Start()
    {
		FM = this.gameObject.GetComponent<FriendsManager>();
		StartCoroutine(RecommendFriend());
    }


	public void RestRecommand()
    {
		for (int i = 2; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}
		SearchListCountNumber = 0;
		StartCoroutine(RecommendFriend());
	}

    IEnumerator RecommendFriend()
	{
		SearchInput.text = "";

		SearchStyle.text = "같은 견종 친구추천";
		SearchDetail.text = UserData.instance.Dog1_Style;

		SearchText = UserData.instance.Dog1_Style.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "searchToDogStyle");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
		//
		
		yield return new WaitUntil(() => SearchListCountNumber > 2);

		for (int i = 12; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}

		GameObject barCard = Instantiate(Item_RecommandBar, SearchFriendList);
		barCard.transform.GetChild(0).GetComponent<TMP_Text>().text = "같은 동네 친구추천";
		barCard.transform.GetChild(2).GetComponent<TMP_Text>().text = UserData.instance.User_Local;

		SearchText = UserData.instance.User_Local.Trim();

		form = new WWWForm();
		form.AddField("order", "searchToLocalAddress");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
	
		yield return new WaitUntil(() => SearchListCountNumber > 12);

		for (int i = 23; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}
	}

	public void SearchFriend()
    {
		SearchBtn.gameObject.SetActive(false);
		SearchLoading.SetActive(true);
		SearchStyle.transform.parent.gameObject.SetActive(false);



		if (SearchKind.value == 0)
        {
			SearchToNickName();
			SearchStyle.text = "닉네임 검색 결과";
			SearchDetail.text = SearchInput.text;
		}
		else if(SearchKind.value == 1)
        {
			searchToDogName();
			SearchStyle.text = "강아지명 검색 결과";
			SearchDetail.text = SearchInput.text;
		}
		else if (SearchKind.value == 2)
		{
			searchToDogStyle();
			SearchStyle.text = "견종 검색 결과";
			SearchDetail.text = SearchInput.text;
		}
		else if (SearchKind.value == 3)
		{
			searchToLocalAddress();
			SearchStyle.text = "주소 검색 결과";
			SearchDetail.text = SearchInput.text;
		}
		else
		{
			searchToPlayfabID();
			SearchStyle.text = "ID 검색 결과";
			SearchDetail.text = SearchInput.text;
		}
	
	}


	public void SearchToNickName()
	{
		
		for (int i = 2; i < SearchFriendList.childCount; i++)
		{
				Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}


		SearchText = SearchInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "searchToNickName");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
	}

	public void searchToPlayfabID()
	{
		for (int i = 2; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}

		SearchText = SearchInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "searchToPlayfabID");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
	}

	public void searchToLocalAddress()
	{
		for (int i = 2; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}

		SearchText = SearchInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "searchToLocalAddress");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
	}
	public void searchToDogStyle()
	{
		for (int i = 2; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}

		SearchText = SearchInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "searchToDogStyle");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
	}
	public void searchToDogName()
	{
		for (int i = 2; i < SearchFriendList.childCount; i++)
		{
			Destroy(SearchFriendList.transform.GetChild(i).gameObject);
		}

		SearchText = SearchInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "searchToDogName");
		form.AddField("searchText", SearchText);

		StartCoroutine(Post(form));
	}



	IEnumerator Post(WWWForm form)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // ???????????using?????? ?????????????????
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("???? ?????? ????????.");
		}
	}
	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;

		//print(json);

		SearchBtn.gameObject.SetActive(true);
		SearchLoading.SetActive(false);
		

		GD = JsonConvert.DeserializeObject<GoogleData>(json);
		//GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.order == "searchToNickName")
		{
			SearchStyle.transform.parent.gameObject.SetActive(true);

			if (GD.searchData.Count == 0)
			{
				GameObject EmptyCard = Instantiate(Item_Empty, SearchFriendList);
			}
			for (int i = 0; i < GD.searchData.Count; i++)
			{
				GameObject itemCard = Instantiate(Item_SearchFriend, SearchFriendList);
				
				itemCard.transform.GetChild(0).GetComponent<TMP_Text>().text = GD.searchData[i][0];
				itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = "ID : " + GD.searchData[i][1];
				itemCard.GetComponent<SearchFriend>().PlayfabID = GD.searchData[i][1];

				if (GD.searchData[i][1] == UserData.instance.User_MyID)
                {
					Destroy(itemCard.gameObject);
					//print("Me");
					//itemCard.transform.GetChild(2).gameObject.SetActive(false);
					//itemCard.transform.GetChild(3).gameObject.SetActive(true);
					//?????????????????? ????????.
				}
				
				for (int z = 0; z < FM._friends.Count; z++)
                {//???????? ????????
					
					if (GD.searchData[i][1] == FM._friends[z].FriendPlayFabId)
                    {
						
						if (FM._friends[z].Tags[0] == "confirmed")
                        {
							
							itemCard.GetComponent<SearchFriend>().Relationship = 1;
                        }

					}

				}
				
			}


			SearchListCountNumber = SearchFriendList.childCount;
			return;

		}
		if (GD.order == "searchToPlayfabID")
		{

			SearchStyle.transform.parent.gameObject.SetActive(true);

			if (GD.searchData.Count == 0)
			{
				GameObject EmptyCard = Instantiate(Item_Empty, SearchFriendList);
			}
			for (int i = 0; i < GD.searchData.Count; i++)
			{
				GameObject itemCard = Instantiate(Item_SearchFriend, SearchFriendList);

				itemCard.transform.GetChild(0).GetComponent<TMP_Text>().text = GD.searchData[i][0];
				itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = "ID : " + GD.searchData[i][1];
				itemCard.GetComponent<SearchFriend>().PlayfabID = GD.searchData[i][1];

				if (GD.searchData[i][1] == UserData.instance.User_MyID)
				{
					Destroy(itemCard.gameObject);
					//print("Me");
					//itemCard.transform.GetChild(2).gameObject.SetActive(false);
					//itemCard.transform.GetChild(3).gameObject.SetActive(true);
					//?????????????????? ????????.
				}

				for (int z = 0; z < FM._friends.Count; z++)
				{//???????? ????????

					if (GD.searchData[i][1] == FM._friends[z].FriendPlayFabId)
					{

						if (FM._friends[z].Tags[0] == "confirmed")
						{

							itemCard.GetComponent<SearchFriend>().Relationship = 1;
						}

					}

				}
			}
			SearchListCountNumber = SearchFriendList.childCount;
			return;
		}

		if (GD.order == "searchToLocalAddress")
		{

			SearchStyle.transform.parent.gameObject.SetActive(true);

			if (GD.searchData.Count == 0)
			{
				GameObject EmptyCard = Instantiate(Item_Empty, SearchFriendList);
			}
			for (int i = 0; i < GD.searchData.Count; i++)
			{
				GameObject itemCard = Instantiate(Item_SearchFriend, SearchFriendList);

				itemCard.transform.GetChild(0).GetComponent<TMP_Text>().text = GD.searchData[i][0];
				itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = "ID : " + GD.searchData[i][1];
				itemCard.GetComponent<SearchFriend>().PlayfabID = GD.searchData[i][1];
				if (GD.searchData[i][1] == UserData.instance.User_MyID)
				{
					Destroy(itemCard.gameObject);
					//print("Me");
					//itemCard.transform.GetChild(2).gameObject.SetActive(false);
					//itemCard.transform.GetChild(3).gameObject.SetActive(true);

				}

				for (int z = 0; z < FM._friends.Count; z++)
				{//???????? ????????

					if (GD.searchData[i][1] == FM._friends[z].FriendPlayFabId)
					{

						if (FM._friends[z].Tags[0] == "confirmed")
						{

							itemCard.GetComponent<SearchFriend>().Relationship = 1;
						}

					}

				}
			}
			SearchListCountNumber = SearchFriendList.childCount;
			return;
		}

		if (GD.order == "searchToDogStyle")
		{
			SearchStyle.transform.parent.gameObject.SetActive(true);

			if(GD.searchData.Count == 0)
            {
				GameObject EmptyCard = Instantiate(Item_Empty, SearchFriendList);
			}

			for (int i = 0; i < GD.searchData.Count; i++)
			{
				GameObject itemCard = Instantiate(Item_SearchFriend, SearchFriendList);

				itemCard.transform.GetChild(0).GetComponent<TMP_Text>().text = GD.searchData[i][0];
				itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = "ID : " + GD.searchData[i][1];
				itemCard.GetComponent<SearchFriend>().PlayfabID = GD.searchData[i][1];

				if (GD.searchData[i][1] == UserData.instance.User_MyID)
				{
					Destroy(itemCard.gameObject);
					//print("Me");
					//itemCard.transform.GetChild(2).gameObject.SetActive(false);
					//itemCard.transform.GetChild(3).gameObject.SetActive(true);
					//?????????????????? ????????.
				}

				for (int z = 0; z < FM._friends.Count; z++)
				{//???????? ????????

					if (GD.searchData[i][1] == FM._friends[z].FriendPlayFabId)
					{

						if (FM._friends[z].Tags[0] == "confirmed")
						{
							//itemCard.transform.GetChild(2).gameObject.SetActive(false);
							itemCard.transform.GetChild(4).gameObject.SetActive(true);
							itemCard.GetComponent<SearchFriend>().Relationship = 1;
						}
                        else 
						{
							//itemCard.transform.GetChild(2).gameObject.SetActive(false);
							itemCard.transform.GetChild(5).gameObject.SetActive(true);
						}

					}

				}
			}
			SearchListCountNumber = SearchFriendList.childCount;
			return;
		}

		if (GD.order == "searchToDogName")
		{
			SearchStyle.transform.parent.gameObject.SetActive(true);

			if (GD.searchData.Count == 0)
			{
				GameObject EmptyCard = Instantiate(Item_Empty, SearchFriendList);
			}
			for (int i = 0; i < GD.searchData.Count; i++)
			{
				GameObject itemCard = Instantiate(Item_SearchFriend, SearchFriendList);

				itemCard.transform.GetChild(0).GetComponent<TMP_Text>().text = GD.searchData[i][0];
				itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = "ID : " + GD.searchData[i][1];
				itemCard.GetComponent<SearchFriend>().PlayfabID = GD.searchData[i][1];
				if (GD.searchData[i][1] == UserData.instance.User_MyID)
				{
					Destroy(itemCard.gameObject);
					//print("Me");
					//itemCard.transform.GetChild(2).gameObject.SetActive(false);
					//itemCard.transform.GetChild(3).gameObject.SetActive(true);

				}

				for (int z = 0; z < FM._friends.Count; z++)
				{//???????? ????????

					if (GD.searchData[i][1] == FM._friends[z].FriendPlayFabId)
					{

						if (FM._friends[z].Tags[0] == "confirmed")
						{

							itemCard.GetComponent<SearchFriend>().Relationship = 1;
						}

					}

				}
			}
			SearchListCountNumber = SearchFriendList.childCount;
			return;
		}

	}

}
