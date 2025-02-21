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
                //outputArea.text = request.downloadHandler.text; // ���°� ������ ���
                string data = request.downloadHandler.text;
                Food food = JsonConvert.DeserializeObject<Food>(data);
                //outputArea.text = Food.resultMsg;
            }

            else
            {
                //outputArea.text = request.downloadHandler.text; // �ǰ� �Է��� ���

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
                    
                    instance.transform.GetChild(1).GetComponent<TMP_Text>().text = "��ǰ�� : " + food.result_diet[i].name;
                    instance.transform.GetChild(2).GetComponent<TMP_Text>().text = food.result_diet[i].me + " cal";

                    if (food.result_diet[i].title == "��Ʈ���� �ǰ���") // ��
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title + "��";
                    }
                    else if (food.result_diet[i].title == "�����Ľ������̾�") // Ƽ
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title + "Ƽ";
                    }
                    else if (food.result_diet[i].title == "��Ʈ���ҽ� �׷�") // ������
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title + "������";
                    }
                    else if (food.result_diet[i].title == "���� ���̾𽺴�") //�̾�Ʈ
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title + "�̾�Ʈ";
                    }
                    else if (food.result_diet[i].title == "���� ������ũ��") //�� ���̾�Ʈ
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title + "�� ���̾�Ʈ";
                    }
                    else if (food.result_diet[i].title == "�����Ľ� ������") //��
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title + "��";
                    }
                    else
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = "�귣��� : " + food.result_diet[i].title;
                    }
                   
                }
                
            }

        }
    }
}



