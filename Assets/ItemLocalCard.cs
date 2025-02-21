using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLocalCard : MonoBehaviour
{

    public GoogleSheetManager gsm;
    public Button LocalNextBtn;
    public bool IsOn;


    public void Start()
    {
        gsm = GameObject.Find("GoogleSheetManager").GetComponent<GoogleSheetManager>();
        LocalNextBtn = GameObject.Find("LocalNextBtn").GetComponent<Button>();
    }


    public void IsOnBtn()
    {
        if (!IsOn)
        {
            UnCheckAll();

            IsOn = true;
            Color color;
            ColorUtility.TryParseHtmlString("#FF6A00", out color);
            this.GetComponent<Image>().color = color;

            this.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().color = Color.white;
            UserData.instance.User_Local = this.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text;
            LocalNextBtn.interactable = true;
            //모든버튼 끄기
        }
        else
        {
            IsOn = false;
            this.GetComponent<Image>().color = Color.white;
            this.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().color = Color.black;
            UserData.instance.User_Local = "";
            LocalNextBtn.interactable = false;
        }



    }

    public void UnCheckAll()
    {
        GameObject mother = this.transform.parent.gameObject;

        for(int i = 0; i < mother.transform.childCount;  i++)
        {
            mother.transform.GetChild(i).GetComponent<ItemLocalCard>().IsOn = false;
            mother.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            mother.transform.GetChild(i).GetChild(1).GetComponent<TMPro.TMP_Text>().color = Color.black;
        }
        
    }
}
