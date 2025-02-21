using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class OtherUserData : MonoBehaviour
{

    public Image OhterPlayerPhoto, OhterDog1Photo, OhterDog2Photo, OhterDog3Photo;

    public static OtherUserData instance;

    [Header("Other User Infor")]
    public string OhterUser_ID; // 다른 유저 아이디
    public string OhterUser_Name; // 유저이름
    public string OhterUser_Local; // 지역
    public string OhterUser_Hello; // 인사말
    public int OhterUser_LikeCount; // 좋아요 수
    public int OhterUser_FriendsCount; // 친구 수
    public int OhterUser_DogCount; // 강아지 수
    public string OhterUser_SaveTime; // 최종접속시간 및 저장시간



    // Dog1

    [Header("Other Dog1 Infor")]
    public string OhterDog1_PevoNumber; // 페보번호
    public string OhterDog1_PetRgtNo;
    public string OhterDog1_Character; // 캐릭터
    public string OhterDog1_Name; // 개이름
    public string OhterDog1_Style; // 견종
    public string OhterDog1_Age; // 개나이
    public string OhterDog1_Adopted;
    public string OhterDog1_Weight; // 개몸무게
    public string OhterDog1_Sex; // 성별
    public string OhterDog1_Neuter; // 중성화
    public string OhterDog1_Food; // 푸드 Json
    public string OhterDog1_Place; // 장소 Json


    [Header("Other Dog1 Condition")]
    public float OhterDog1_Health; // 건강
    public float OhterDog1_Hungry; // 배고픔
    public float OhterDog1_Tirsty; // 목마름
    public float OhterDog1_Clean; // 청결
    public float OhterDog1_Happy; // 행복

    [Header("Other Dog1 Stats")]
    public int OhterDog1_Level;
    public int OhterDog1_Exp;
    public int OhterDog1_Speed;
    public int OhterDog1_Power;
    public int OhterDog1_Stamina;
    public int OhterDog1_Sense;
    public int OhterDog1_Guts;
    public int OhterDog1_Lux;

    [Header("Dog1 Item Equip")]
    public string OhterDog1_item_Head;
    public string OhterDog1_item_Eye;
    public string OhterDog1_item_Neck;
    public string OhterDog1_item_Body;

    // Dog2

    [Header("Other Dog2 Infor")]
    public string OhterDog2_PevoNumber; // 페보번호
    public string OhterDog2_PetRgtNo;
    public string OhterDog2_Character; // 캐릭터
    public string OhterDog2_Name; // 개이름
    public string OhterDog2_Style; // 견종
    public string OhterDog2_Age; // 개나이
    public string OhterDog2_Adopted;
    public string OhterDog2_Weight; // 개몸무게
    public string OhterDog2_Sex; // 성별
    public string OhterDog2_Neuter; // 중성화
    public string OhterDog2_Food; // 푸드 Json
    public string OhterDog2_Place; // 장소 Json

    [Header("Other Dog2 Condition")]
    public float OhterDog2_Health; // 건강
    public float OhterDog2_Hungry; // 배고픔
    public float OhterDog2_Tirsty; // 목마름
    public float OhterDog2_Clean; // 청결
    public float OhterDog2_Happy; // 행복

    [Header("Other Dog2 Stats")]
    public int OhterDog2_Level;
    public int OhterDog2_Exp;
    public int OhterDog2_Speed;
    public int OhterDog2_Power;
    public int OhterDog2_Stamina;
    public int OhterDog2_Sense;
    public int OhterDog2_Guts;
    public int OhterDog2_Lux;

    [Header("Other Dog2 Item Equip")]
    public string OhterDog2_item_Head;
    public string OhterDog2_item_Eye;
    public string OhterDog2_item_Neck;
    public string OhterDog2_item_Body;


    // Dog3

    [Header("Other Dog3 Infor")]
    public string OhterDog3_PevoNumber; // 페보번호
    public string OhterDog3_PetRgtNo;
    public string OhterDog3_Character; // 캐릭터
    public string OhterDog3_Name; // 개이름
    public string OhterDog3_Style; // 견종
    public string OhterDog3_Age; // 개나이
    public string OhterDog3_Adopted;
    public string OhterDog3_Weight; // 개몸무게
    public string OhterDog3_Sex; // 성별
    public string OhterDog3_Neuter; // 중성화
    public string OhterDog3_Food; // 푸드 Json
    public string OhterDog3_Place; // 장소 Json

    [Header("Other Dog3 Condition")]
    public float OhterDog3_Health; // 건강
    public float OhterDog3_Hungry; // 배고픔
    public float OhterDog3_Tirsty; // 목마름
    public float OhterDog3_Clean; // 청결
    public float OhterDog3_Happy; // 행복

    [Header("Other Dog1 Stats")]
    public int OhterDog3_Level;
    public int OhterDog3_Exp;
    public int OhterDog3_StatPoint;
    public int OhterDog3_Speed;
    public int OhterDog3_Power;
    public int OhterDog3_Stamina;
    public int OhterDog3_Sense;
    public int OhterDog3_Guts;
    public int OhterDog3_Lux;

    [Header("Other Dog3 Item Equip")]
    public string OhterDog3_item_Head;
    public string OhterDog3_item_Eye;
    public string OhterDog3_item_Neck;
    public string OhterDog3_item_Body;



    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(this.gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
    }

}
