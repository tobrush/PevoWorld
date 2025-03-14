using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System;


public class PevoDogApiManager : MonoBehaviour
{
    public TMP_InputField Serch_pet_id;
    public Button NextBtn, PetRgtBtn;

    public PlayfabGuestLoginManager PGLM;

    public TMP_Text Card_PevoNumber;
    public TMP_Text Card_dogName;
    public TMP_Text Card_dogStyle;
    public TMP_Text Card_dogAge;
    public TMP_Text Card_dogSex;
    public TMP_Text Card_dogWeight;
    public TMP_Text Card_dogAdopted;
    public TMP_Text Card_dogRgtNo;

    public void NotEmptyInput()
    {
        if (Serch_pet_id.text.Length < 3 || Serch_pet_id.text.Length > 10 || Serch_pet_id.text == "")
        {
            NextBtn.interactable = false;

        }
        else
        {
            NextBtn.interactable = true;
        }
            
    }


    public void DogData()
    {
        NextBtn.transform.GetChild(0).gameObject.SetActive(false);
        NextBtn.transform.GetChild(1).gameObject.SetActive(true);
        NextBtn.interactable = false;

        StartCoroutine(DogData_Coroutine());
    }

    IEnumerator DogData_Coroutine()
    {
        yield return new WaitForSeconds(1.0f);

        string url = "https://api.pevo.care/v4/pevo_world/pet_info";
        WWWForm form = new WWWForm();
        form.AddField("pet_id", Serch_pet_id.text);
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                NextBtn.transform.GetChild(0).gameObject.SetActive(true);
                NextBtn.transform.GetChild(1).gameObject.SetActive(false);
                NextBtn.interactable = true;

                //string data = request.downloadHandler.text;
                //Dog dog = JsonConvert.DeserializeObject<Dog>(data);
                
            }

            else
            {

                NextBtn.transform.GetChild(0).gameObject.SetActive(true);
                NextBtn.transform.GetChild(1).gameObject.SetActive(false);
                NextBtn.interactable = true;

                string data = request.downloadHandler.text;

                Dog dog = JsonConvert.DeserializeObject<Dog>(data);
                dog.pet_id = Serch_pet_id.text;

                UserData.instance.Dog1_PevoNumber = Serch_pet_id.text;
                UserData.instance.Dog1_Name = dog.pet_name;
                UserData.instance.Dog1_Style = dog.breed_name;
                UserData.instance.Dog1_Age = dog.birth;
                UserData.instance.Dog1_Sex = dog.gender;
                UserData.instance.Dog1_Neuter = dog.is_neutered;
                UserData.instance.Dog1_Weight = dog.weight;
                UserData.instance.Dog1_Adopted = dog.adopted_date;
                UserData.instance.Dog1_PetRgtNo = dog.petRgtNo;

                Card_PevoNumber.text = Serch_pet_id.text;
                Card_dogName.text = dog.pet_name;
                Card_dogStyle.text = dog.breed_name;
                Card_dogRgtNo.text = dog.petRgtNo;

                if (dog.diet_name != null)
                {
                    UserData.instance.Dog1_basicFood_cal = dog.diet_me;
                    UserData.instance.Dog1_basicFood_Brand = dog.diet_title;
                    UserData.instance.Dog1_basicFood_Name = dog.diet_name;
                }
                

                if (UserData.instance.Dog1_Name != "")
                {

                    if (UserData.instance.Dog1_Sex == "M")
                    {
                        if (UserData.instance.Dog1_Neuter == "T")
                        {
                            Card_dogSex.text = "남아 / O";
                        }
                        else
                        {
                            Card_dogSex.text = "남아 / X";
                        }

                    }
                    else
                    {
                        if (UserData.instance.Dog1_Neuter == "T")
                        {
                            Card_dogSex.text = "여아 / O";
                        }
                        else
                        {
                            Card_dogSex.text = "여아 / X";
                        }
                    }

                   
                    if (UserData.instance.Dog1_PetRgtNo == "")
                    {
                        PetRgtBtn.gameObject.SetActive(true);
                    }
                    else
                    {
                        PetRgtBtn.gameObject.SetActive(false);
                    }


                    DateTime BirthDate = DateTime.Parse(UserData.instance.Dog1_Age);
                    int BirthYear = BirthDate.Year;
                    int BirthMonth = BirthDate.Month;
                    int BirthDay = BirthDate.Day;

                    DateTime nowDate = DateTime.Now;
                    int NowYear = nowDate.Year;
                    int NowMonth = nowDate.Month;
                    int NowDay = nowDate.Day;

                    int AgeYear;
                    int AgeMonth;
                    int AgeDay;

                    if (NowDay >= BirthDay)
                    {
                        AgeDay = NowDay - BirthDay;
                    }
                    else
                    {
                        NowMonth -= 1;
                        AgeDay = DateTime.DaysInMonth(NowYear,NowMonth) + NowDay - BirthDay;

                        if(AgeDay > 15)
                        {
                            NowMonth += 1;
                        }
                    }


                    if (NowMonth >= BirthMonth)
                    {
                        AgeMonth = NowMonth - BirthMonth;
                    }
                    else
                    {
                        NowYear -= 1;
                        AgeMonth = 12 + NowMonth - BirthMonth;
                    }

                    AgeYear = NowYear - BirthYear;




                    DateTime AdoptedDate = DateTime.Parse(UserData.instance.Dog1_Adopted);
                    int AdoptedYear = AdoptedDate.Year;
                    int AdoptedMonth = AdoptedDate.Month;
                    int AdoptedDay = AdoptedDate.Day;

                    
                    Card_dogAge.text = BirthYear + "년 " + BirthMonth + "월 " + BirthDay + "일" + "\n" + "(" + AgeYear +"살 " + AgeMonth + "개월)";

                    Card_dogWeight.text = UserData.instance.Dog1_Weight + "kg";
                    Card_dogAdopted.text = AdoptedYear + "년 " + AdoptedMonth + "월 " + AdoptedDay + "일";
                }
                else
                {
                    //Error
                }

                PGLM.PetCardOpen();
                
            }
            request.Dispose();
        }

    }

}
