using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mail_Item : MonoBehaviour
{
    public MailBoxManager MBM;

    public int Sort;
    public string To;
    public string ToID;
    public int Index;
    public string MailName;
    public string MailDesc;
    public string ItemID;
    public string ItemAmount;
    public string GetItem;
    public string ExpireTime;

    public void Start()
    {
        MBM = GameObject.Find("MailBoxManager").GetComponent<MailBoxManager>();
    }

    public void ReadMail()
    {
        if (ItemID == "PC")
        {
            MBM.MailInItemImage.sprite = MBM.itemSprite[0];
        }
        if (ItemID == "PG")
        {
            MBM.MailInItemImage.sprite = MBM.itemSprite[1];
        }
        
       
        MBM.OpenBox.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text = To;
       MBM.OpenBox.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = ExpireTime.Substring(0,10);
       MBM.OpenBox.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = MailName;
       MBM.OpenBox.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>().text = MailDesc;
   
        if (Sort == 0) // �������� ǥ�Ե�.
        {
          MBM.OpenBox.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.GetComponent<TMP_Text>().text = ItemID; // �������̸�
            MBM.OpenBox.transform.GetChild(0).GetChild(3).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "X "+ItemAmount; // ����
            MBM.OpenBox.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            MBM.OpenBox.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
        }
        else // �������� ǥ�Ե������� �޽���
        {
            MBM.OpenBox.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            MBM.OpenBox.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        }
        MBM.index = this.Index;

        if(GetItem == "0") // �����ۼ��ɿ���
        {
            MBM.NewGet();
            
        }
        else
        {
           MBM.AlreadyGet();
        }

        MBM.OpenBox.SetActive(true);
    }
}
