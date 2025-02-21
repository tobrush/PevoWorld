using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System;


public class result_diet
{
    public string seq { get; set; }
    public string title { get; set; }
    public string name { get; set; }
    public string me { get; set; }

}

    public class Food
{
    public string resultCode { get; set; }
    public string resultMsg { get; set; }
    public List<result_diet> result_diet { get; set; }
}

public class FoodSearchApiManager : MonoBehaviour
{
    public GameObject SearchFoodCard;
    public Transform CardSpawnList;
    public TMP_InputField SearchNameField;

    public void FoodData()
    {
        StartCoroutine(FoodData_Coroutine());
    }

    IEnumerator FoodData_Coroutine()
    {
        string uri = "https://api.pevo.care/v4/pevo_world/pet_diet";
        WWWForm form = new WWWForm();
        form.AddField("diet_name", SearchNameField.text);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //outputArea.text = request.downloadHandler.text; // 없는걸 가져온 경우
                string data = request.downloadHandler.text;
                Food food = JsonConvert.DeserializeObject<Food>(data);
                //outputArea.text = Food.resultMsg;
            }

            else
            {
                //outputArea.text = request.downloadHandler.text; // 옳게 입력한 경우

                string data = request.downloadHandler.text;
                Food food = JsonConvert.DeserializeObject<Food>(data);


                for (int i = 0; i < CardSpawnList.childCount; i++)
                { 
                    Destroy(CardSpawnList.GetChild(i).gameObject); 
                }
                CardSpawnList.DetachChildren();
  



                for (int i = 0; i < food.result_diet.Count; i++)
                {
                    GameObject instance = Instantiate(SearchFoodCard, CardSpawnList);
                    
                    instance.transform.GetChild(1).GetComponent<TMP_Text>().text = "제품명 : " + food.result_diet[i].name;
                    instance.transform.GetChild(2).GetComponent<TMP_Text>().text = food.result_diet[i].me + " cal";

                    if (food.result_diet[i].title == "뉴트리나 건강백") // 서
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title + "서";
                    }
                    else if (food.result_diet[i].title == "네이쳐스버라이어") // 티
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title + "티";
                    }
                    else if (food.result_diet[i].title == "뉴트리소스 그레") // 인프리
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title + "인프리";
                    }
                    else if (food.result_diet[i].title == "힐스 사이언스다") //이어트
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title + "이어트";
                    }
                    else if (food.result_diet[i].title == "힐스 프리스크립") //션 다이어트
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title + "션 다이어트";
                    }
                    else if (food.result_diet[i].title == "네이쳐스 프로텍") //션
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title + "션";
                    }
                    else
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "브랜드명 : " + food.result_diet[i].title;
                    }
                   
                }
                
            }

        }
    }
}



