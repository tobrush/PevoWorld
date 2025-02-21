using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ClickManager : MonoBehaviour
{

    public RectTransform mailPanel;
    public RectTransform shopPanel;
    public RectTransform invenPanel;
    public RectTransform outPanel;
    public RectTransform NoticePanel;
    public RectTransform ConfigPanel;
    public RectTransform FriendsPanel;


    private GameObject MenuCancleBtn;
    private GameObject CancleSideBtn;

    public GameObject GetAllBtn;
    public GameObject InvenCancleBtn;
    public GameObject MailCancleBtn;
    public GameObject ShopCancleBtn;
    public GameObject OutCancleBtn;
    public GameObject NoticeCancleBtn;
    public GameObject ConfigCancleBtn;
    public GameObject FriendsCancleBtn;

    public Sprite pevoMark;
    public Sprite ringMark;

    public Animator CancleAnim;

    public RectTransform pevoBtn;

    public RectTransform CharacterPaenl1;
    public RectTransform CharacterPaenl2;
    public RectTransform CharacterPaenl3;

    public RectTransform CharacterPaenl4;
    public RectTransform CharacterPaenl5;
    public RectTransform CharacterPaenl6;



    public GameObject Cancle;

    public RectTransform FirstSlot;

    public GameObject Slot01;
    public GameObject Slot02;
    public GameObject Slot03;

    public GameObject Slot04;
    public GameObject Slot05;
    public GameObject Slot06;

    private Vector3 firstVector;
    private Vector3 SelfVector;
    
    public Animator anim;
    public bool ChaOpen;
    public bool BtnOpen;

    public Animator anim_Back;
    public float alpha;

    public int nowPage;

    private void Update()
    {
        /*
        if (ChaOpen)
        {
            Image[] children;
            Color newColor;

            if (FirstSlot.gameObject == Slot01)
            {
                children = CharacterPaenl1.GetComponentsInChildren<Image>();
            }
            else if (FirstSlot.gameObject == Slot02)
            {
                children = CharacterPaenl2.GetComponentsInChildren<Image>();
            }
            else
            {
                children = CharacterPaenl3.GetComponentsInChildren<Image>();
            }
            
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        if (MailOpen)
        {
            Image[] children;
            Color newColor;

            children = mailPanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;
                
            }
        }
        if (ShopOpen)
        {
            Image[] children;
            Color newColor;

            children = shopPanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        if (InvenOpen)
        {
            Image[] children;
            Color newColor;

            children = invenPanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        if (OutOpen)
        {
            Image[] children;
            Color newColor;

            children = outPanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        if (NoticeOpen)
        {
            Image[] children;
            Color newColor;

            children = NoticePanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        if (ConfigOpen)
        {
            Image[] children;
            Color newColor;

            children = ConfigPanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        if (FriendsOpen)
        {
            Image[] children;
            Color newColor;

            children = FriendsPanel.GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;

            }
        }
        */
    }







    public void CancleAnimation_ring()
    {
        CancleAnim.GetComponent<Image>().sprite = ringMark;
        CancleAnim.SetTrigger("Cancle");
    }

    public void CancleAnimation_pevo()
    {
        CancleAnim.GetComponent<Image>().sprite = pevoMark;
        CancleAnim.SetTrigger("Cancle");
    }



    public void MenuBtn()
    {
        if (!BtnOpen)
        {
            pevoBtn.localScale = new Vector3(0f, 0f, 0f);
            ButtonOpen();
            BtnOpen = true;
        }
        else
        {
            pevoBtn.localScale = new Vector3(1f, 1f, 1f);
            ButtonClose();
            BtnOpen = false;
        }

    }

    public void ButtonOpen()
    {
        anim.SetBool("MenuBtn", true);
        anim_Back.SetBool("MenuBtn", true);
    }
    public void ButtonClose()
    {
        anim.SetBool("MenuBtn", false);
        anim_Back.SetBool("MenuBtn", false);
    }

    /*
      // OnMenu
      public void OnMenuMe(GameObject menuMe)
      {
          alpha = 0;
          menuMe.SetActive(true);
          if (menuMe.name == "MailboxPanel")
          {
              MailOpen = true;
              CancleSideBtn = GetAllBtn;
              MenuCancleBtn = MailCancleBtn;
          }
          else if (menuMe.name == "ShopPanel")
          {
              ShopOpen = true;
              CancleSideBtn = null;
              MenuCancleBtn = ShopCancleBtn;
          }
          else if (menuMe.name == "InvenPanel")
          {
              InvenOpen = true;
              CancleSideBtn = null;
              MenuCancleBtn = InvenCancleBtn;
          }
          else if (menuMe.name == "NoticePanel")
          {
              NoticeOpen = true;
              CancleSideBtn = null;
              MenuCancleBtn = NoticeCancleBtn;
          }
          else if (menuMe.name == "ConfigPanel")
          {
              ConfigOpen = true;
              CancleSideBtn = null;
              MenuCancleBtn = ConfigCancleBtn;
          }
          else if (menuMe.name == "FriendsPanel")
          {
              FriendsOpen = true;
              CancleSideBtn = null;
              MenuCancleBtn = FriendsCancleBtn;
          }
          else
          {
              OutOpen = true;
              CancleSideBtn = null;
              MenuCancleBtn = OutCancleBtn;
          }


          if (CancleSideBtn != null)
          {
              CancleSideBtn.GetComponent<Button>().interactable = false;
          }
          MenuCancleBtn.GetComponent<Button>().interactable = false;

          StartCoroutine(MenuMe(menuMe.GetComponent<RectTransform>()));
      }
      IEnumerator MenuMe(RectTransform menuPaenl)
      {
          Color color = menuPaenl.GetComponent<Image>().color;

          for (float t = 0f; t <= 1f; t += Time.fixedDeltaTime)
          {
              if (CancleSideBtn != null)
              {
                  CancleSideBtn.SetActive(true);
              }

              MenuCancleBtn.SetActive(true);

              menuPaenl.offsetMin = Vector3.Lerp(menuPaenl.offsetMin, new Vector2(menuPaenl.offsetMin.x, 0), t);
              menuPaenl.offsetMax = Vector3.Lerp(menuPaenl.offsetMax, new Vector2(menuPaenl.offsetMax.x, 0), t);
              alpha = Mathf.Lerp(0, 1, t * 2);
              color.a = Mathf.Lerp(0, 1, t * 2);
              menuPaenl.GetComponent<Image>().color = color;

              if (CancleSideBtn != null)
              {
                  CancleSideBtn.GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), t * 3 - 1.5f);
              }
              MenuCancleBtn.GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), t * 3 - 1.5f);

              //menuPaenl.transform.GetChild().GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), t * 3 - 1.5f);

              yield return null;
          }


          //Time.timeScale = 0;

          if (CancleSideBtn != null)
          {
              CancleSideBtn.GetComponent<Button>().interactable = true;
          }
          MenuCancleBtn.GetComponent<Button>().interactable = true;
          MailOpen = false;
          ShopOpen = false;
          InvenOpen = false;
          OutOpen = false;
          ConfigOpen = false;
          NoticeOpen = false;
          FriendsOpen = false;
      }

      public void OnMenuMeClose(GameObject menuMe)
      {
          Time.timeScale = 1;

          if (menuMe.name == "MailboxPanel")
          {
              MailOpen = false;
          }
          else if (menuMe.name == "ShopPanel")
          {
              ShopOpen = false;
          }
          else if (menuMe.name == "InvenPanel")
          {
              InvenOpen = false;
          }
          else if (menuMe.name == "ConfigPanel")
          {
              ConfigOpen = false;
          }
          else if (menuMe.name == "NoticePanel")
          {
              NoticeOpen = false;
          }
          else if (menuMe.name == "FriendsPanel")
          {
              FriendsOpen = false;
          }
          else
          {
              OutOpen = false;
          }

          StartCoroutine(CloseMenuPanel(menuMe.GetComponent<RectTransform>()));

      }


      IEnumerator CloseMenuPanel(RectTransform menuPaenl)
      {
          for (float t = 0f; t <= 1f; t += Time.fixedDeltaTime)
          {
              menuPaenl.offsetMin = Vector3.Lerp(menuPaenl.offsetMin, new Vector2(menuPaenl.offsetMin.x, -3000), t);
              menuPaenl.offsetMax = Vector3.Lerp(menuPaenl.offsetMax, new Vector2(menuPaenl.offsetMax.x, -3000), t);
              Color color = menuPaenl.GetComponent<Image>().color;
              color.a = Mathf.Lerp(1, 0, t * 2);
              menuPaenl.GetComponent<Image>().color = color;
              alpha = Mathf.Lerp(1, 0, t * 2);
              if (CancleSideBtn != null)
              {
                  CancleSideBtn.GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), t * 3);
              }
              MenuCancleBtn.GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), t * 3);
              yield return null;
          }
          if (CancleSideBtn != null)
          {
              CancleSideBtn.SetActive(false);
          }
          menuPaenl.gameObject.SetActive(false);
          MenuCancleBtn.SetActive(false);
      }
      */

    // OnClick Character

    public void OnClickMe(GameObject itsMe)
    {
        StartCoroutine(ClickMe(itsMe));
    }
    
    IEnumerator ClickMe(GameObject itsMe)
    {

        
        if (itsMe.GetComponent<RectTransform>() != FirstSlot)
        {
            Slot01.GetComponent<Button>().interactable = false;
            Slot02.GetComponent<Button>().interactable = false;
            Slot03.GetComponent<Button>().interactable = false;
            if(Slot04 != null)
            {
                Slot04.GetComponent<Button>().interactable = false;
            }
            if (Slot05 !=null)
            {
                Slot05.GetComponent<Button>().interactable = false;
            }
            if (Slot06 != null)
            {
                Slot06.GetComponent<Button>().interactable = false;
            }

            firstVector = FirstSlot.localPosition;
            SelfVector = itsMe.GetComponent<RectTransform>().localPosition;
            //itsMe.GetComponent<RectTransform>().localPosition = FirsetSlot.localPosition;
            for (float t = 0f; t <= 0.7f; t += Time.fixedDeltaTime)
            {
                itsMe.GetComponent<RectTransform>().localPosition = Vector3.Lerp(itsMe.GetComponent<RectTransform>().localPosition, firstVector, t);
                itsMe.GetComponent<RectTransform>().localScale = Vector3.Lerp(itsMe.GetComponent<RectTransform>().localScale, new Vector3(2f, 2f, 2f), t);
                FirstSlot.localPosition = Vector3.Lerp(FirstSlot.localPosition, SelfVector, t);
                FirstSlot.localScale = Vector3.Lerp(FirstSlot.localScale, new Vector3(1f, 1f, 1f), t);

                yield return null;

            }

            FirstSlot = itsMe.gameObject.GetComponent<RectTransform>(); // 
            Slot01.GetComponent<Button>().interactable = true;
            Slot02.GetComponent<Button>().interactable = true;
            Slot03.GetComponent<Button>().interactable = true;
            if (Slot04 != null)
            {
                Slot04.GetComponent<Button>().interactable = true;
            }
            if (Slot05 != null)
            {
                Slot05.GetComponent<Button>().interactable = true;
            }
            if (Slot06 != null)
            {
                Slot06.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            
            //alpha = 0;
            //ChaOpen = true;
            //Cancle.GetComponent<Button>().interactable = false;

            if (itsMe == Slot01)
            {
                nowPage = 1;
                
                itsMe.GetComponent<Button>().interactable = false;
                CharacterPaenl1.gameObject.SetActive(true);
                CharacterPaenl1.DOAnchorPos(Vector2.zero, 0.3f);
                //StartCoroutine(OpenCharacterPanel(CharacterPaenl1));
            }
            else if (itsMe == Slot02)
            {
                nowPage = 2;

                itsMe.GetComponent<Button>().interactable = false;
                CharacterPaenl2.gameObject.SetActive(true);
                CharacterPaenl2.DOAnchorPos(Vector2.zero, 0.3f);
                //StartCoroutine(OpenCharacterPanel(CharacterPaenl2));
            }
            else if (itsMe == Slot03)
            {
                nowPage = 3;

                itsMe.GetComponent<Button>().interactable = false;
                CharacterPaenl3.gameObject.SetActive(true);
                CharacterPaenl3.DOAnchorPos(Vector2.zero, 0.3f);
                //StartCoroutine(OpenCharacterPanel(CharacterPaenl3));
            }
            else if (itsMe == Slot04)
            {
                nowPage = 4;

                itsMe.GetComponent<Button>().interactable = false;
                CharacterPaenl4.gameObject.SetActive(true);
                CharacterPaenl4.DOAnchorPos(Vector2.zero, 0.3f);
               
            }
            else if (itsMe == Slot05)
            {
                nowPage = 5;

                itsMe.GetComponent<Button>().interactable = false;
                CharacterPaenl5.gameObject.SetActive(true);
                CharacterPaenl5.DOAnchorPos(Vector2.zero, 0.3f);
               
            }
            else if (itsMe == Slot06)
            {
                nowPage = 6;

                itsMe.GetComponent<Button>().interactable = false;
                CharacterPaenl6.gameObject.SetActive(true);
                CharacterPaenl6.DOAnchorPos(Vector2.zero, 0.3f);
              
            }

        }
    }

    public void OpenMenu(RectTransform ThisDogPanel)
    {
        ThisDogPanel.gameObject.SetActive(true);
        ThisDogPanel.DOAnchorPos(Vector2.zero, 0.3f);
    }


    public void CloseMenu(RectTransform ThisDogPanel)
    {
        //ThisDogPanel.DOAnchorPos(new Vector2(2000, 0), 0.3f).OnComplete(() => {
            ThisDogPanel.gameObject.SetActive(false);
       // });
    }





    public void CloseMe(RectTransform ThisDogPanel)
    {

        ThisDogPanel.DOAnchorPos(new Vector2(-2000, 0), 0.3f).OnComplete(() => {
            ThisDogPanel.gameObject.SetActive(false);
        });

        if (ThisDogPanel == CharacterPaenl1)
        {
            Slot01.GetComponent<Button>().interactable = true;
        }
        else if (ThisDogPanel == CharacterPaenl2)
        {
            Slot02.GetComponent<Button>().interactable = true;
        }
        else if(ThisDogPanel == CharacterPaenl3)
        {
            Slot03.GetComponent<Button>().interactable = true;
        }
        else if (ThisDogPanel == CharacterPaenl4)
        {
            Slot04.GetComponent<Button>().interactable = true;
        }
        else if (ThisDogPanel == CharacterPaenl5)
        {
            Slot05.GetComponent<Button>().interactable = true;
        }
        else if (ThisDogPanel == CharacterPaenl6)
        {
            Slot06.GetComponent<Button>().interactable = true;
        }


    }
    void Com(RectTransform thisPanel)
    {
        thisPanel.gameObject.SetActive(false);
    }



    IEnumerator OpenCharacterPanel(RectTransform CharacterPaenl)
    {
        CharacterPaenl.gameObject.SetActive(true);
        Color color = CharacterPaenl.GetComponent<Image>().color;

        for (float t = 0f; t <= 1f; t += Time.fixedDeltaTime)
        {
            Cancle.SetActive(true);
            CharacterPaenl.offsetMin = Vector3.Lerp(CharacterPaenl.offsetMin, new Vector2(CharacterPaenl.offsetMin.x, 0), t);
            CharacterPaenl.offsetMax = Vector3.Lerp(CharacterPaenl.offsetMax, new Vector2(CharacterPaenl.offsetMax.x, 0), t);
            alpha = Mathf.Lerp(0, 1, t * 2);
            color.a = Mathf.Lerp(0, 1, t * 2);
            CharacterPaenl.GetComponent<Image>().color = color;

            Cancle.GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), t * 3 - 1.5f);

            yield return null;
        }
        //Time.timeScale = 0;
        Cancle.GetComponent<Button>().interactable = true;
        ChaOpen = false;
    }

    public void OnCloseCharacterPanel()
    {
        Time.timeScale = 1;
        if (FirstSlot.gameObject == Slot01)
        {
            StartCoroutine(CloseCharacterPanel(CharacterPaenl1)); 
        }
        else if (FirstSlot.gameObject == Slot02)
        {
            StartCoroutine(CloseCharacterPanel(CharacterPaenl2));
        }
        else if (FirstSlot.gameObject == Slot03)
        {
            StartCoroutine(CloseCharacterPanel(CharacterPaenl3));
        }
        else if (FirstSlot.gameObject == Slot04)
        {
            StartCoroutine(CloseCharacterPanel(CharacterPaenl4));
        }
        else if (FirstSlot.gameObject == Slot05)
        {
            StartCoroutine(CloseCharacterPanel(CharacterPaenl5));
        }
        else if (FirstSlot.gameObject == Slot06)
        {
            StartCoroutine(CloseCharacterPanel(CharacterPaenl6));
        }


        if (!BtnOpen)
        {
            CancleAnim.GetComponent<Image>().sprite = pevoMark;
        }
        else
        {
            CancleAnim.GetComponent<Image>().sprite = ringMark;
        }
        CancleAnim.SetTrigger("Cancle");

       
    }
   
    IEnumerator CloseCharacterPanel(RectTransform CharacterPaenl)
    {
        ChaOpen = false;
        for (float t = 0f; t <= 1f; t += Time.fixedDeltaTime)
        {
            CharacterPaenl.offsetMin = Vector3.Lerp(CharacterPaenl.offsetMin, new Vector2(CharacterPaenl.offsetMin.x, -3000), t);
            CharacterPaenl.offsetMax = Vector3.Lerp(CharacterPaenl.offsetMax, new Vector2(CharacterPaenl.offsetMax.x, -3000), t);
            Color color = CharacterPaenl.GetComponent<Image>().color;
            color.a = Mathf.Lerp(1, 0, t * 2);
            CharacterPaenl.GetComponent<Image>().color = color;
            alpha = Mathf.Lerp(1, 0, t * 2);
            Cancle.GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), t*3);
            yield return null;
        }
        FirstSlot.GetComponent<Button>().interactable = true;
        CharacterPaenl.gameObject.SetActive(false);
        Cancle.SetActive(false);

    }

}
