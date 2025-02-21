using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherUserDataManager : MonoBehaviour
{

    public Text CharacterUserName;
    public Text CharacterDogName;
    
    public Text TitleUserName;
    public Text TitleDogName;

    public Text DogNameText;
    public Text DogStyleText;
    public Text DisplayInfor;
    public bool Male;

    public Button EditBtn;

    void Start()
    {
       
        GetOtherUserData();
    }

    public void GetOtherUserData()
    {
        CharacterUserName.text = OtherUserData.instance.OhterUser_Name +"님의";
        CharacterDogName.text = OtherUserData.instance.OhterDog1_Name;

        TitleUserName.text = OtherUserData.instance.OhterUser_Name;
        TitleDogName.text = OtherUserData.instance.OhterDog1_Name;


        DogNameText.text = OtherUserData.instance.OhterDog1_Name;
        DogStyleText.text = OtherUserData.instance.OhterDog1_Style + "<color=#88CDFF>♂</color>"; // 임시남자
        DisplayInfor.text = "<size=25>지역</size>" + "\n" + OtherUserData.instance.OhterUser_Local + "\n" + "<size=25>출생일(나이) </size>" + "\n" + OtherUserData.instance.OhterDog1_Age + "\n" + "<size=25>몸무게</size>" + "\n" + OtherUserData.instance.OhterDog1_Weight + "\n" + "<size=25>중성화 여부</size>" + "\n" + "O";

        EditBtn.gameObject.SetActive(false);
    }

}
