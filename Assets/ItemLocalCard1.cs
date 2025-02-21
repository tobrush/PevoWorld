using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLocalCard1 : MonoBehaviour
{
    public Button LocalNextBtn;
    public bool IsOn;


    public void Start()
    {
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
            UserData.instance.editDummyData = this.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text;
            LocalNextBtn.interactable = true;
            //???????? ????
        }
        else
        {
            IsOn = false;
            this.GetComponent<Image>().color = Color.white;
            this.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().color = Color.black;
            UserData.instance.editDummyData = "";
            LocalNextBtn.interactable = false;
        }



    }

    public void UnCheckAll()
    {
        GameObject mother = this.transform.parent.gameObject;

        print(mother.name + " : childCount :  " + mother.transform.childCount);

        for(int i = 0; i < mother.transform.childCount;  i++)
        {
            mother.transform.GetChild(i).GetComponent<ItemLocalCard1>().IsOn = false;
            mother.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            mother.transform.GetChild(i).GetChild(1).GetComponent<TMPro.TMP_Text>().color = Color.black;
        }
        
    }
}
