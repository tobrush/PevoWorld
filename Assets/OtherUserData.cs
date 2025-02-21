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
    public string OhterUser_ID; // �ٸ� ���� ���̵�
    public string OhterUser_Name; // �����̸�
    public string OhterUser_Local; // ����
    public string OhterUser_Hello; // �λ縻
    public int OhterUser_LikeCount; // ���ƿ� ��
    public int OhterUser_FriendsCount; // ģ�� ��
    public int OhterUser_DogCount; // ������ ��
    public string OhterUser_SaveTime; // �������ӽð� �� ����ð�



    // Dog1

    [Header("Other Dog1 Infor")]
    public string OhterDog1_PevoNumber; // �亸��ȣ
    public string OhterDog1_PetRgtNo;
    public string OhterDog1_Character; // ĳ����
    public string OhterDog1_Name; // ���̸�
    public string OhterDog1_Style; // ����
    public string OhterDog1_Age; // ������
    public string OhterDog1_Adopted;
    public string OhterDog1_Weight; // ��������
    public string OhterDog1_Sex; // ����
    public string OhterDog1_Neuter; // �߼�ȭ
    public string OhterDog1_Food; // Ǫ�� Json
    public string OhterDog1_Place; // ��� Json


    [Header("Other Dog1 Condition")]
    public float OhterDog1_Health; // �ǰ�
    public float OhterDog1_Hungry; // �����
    public float OhterDog1_Tirsty; // �񸶸�
    public float OhterDog1_Clean; // û��
    public float OhterDog1_Happy; // �ູ

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
    public string OhterDog2_PevoNumber; // �亸��ȣ
    public string OhterDog2_PetRgtNo;
    public string OhterDog2_Character; // ĳ����
    public string OhterDog2_Name; // ���̸�
    public string OhterDog2_Style; // ����
    public string OhterDog2_Age; // ������
    public string OhterDog2_Adopted;
    public string OhterDog2_Weight; // ��������
    public string OhterDog2_Sex; // ����
    public string OhterDog2_Neuter; // �߼�ȭ
    public string OhterDog2_Food; // Ǫ�� Json
    public string OhterDog2_Place; // ��� Json

    [Header("Other Dog2 Condition")]
    public float OhterDog2_Health; // �ǰ�
    public float OhterDog2_Hungry; // �����
    public float OhterDog2_Tirsty; // �񸶸�
    public float OhterDog2_Clean; // û��
    public float OhterDog2_Happy; // �ູ

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
    public string OhterDog3_PevoNumber; // �亸��ȣ
    public string OhterDog3_PetRgtNo;
    public string OhterDog3_Character; // ĳ����
    public string OhterDog3_Name; // ���̸�
    public string OhterDog3_Style; // ����
    public string OhterDog3_Age; // ������
    public string OhterDog3_Adopted;
    public string OhterDog3_Weight; // ��������
    public string OhterDog3_Sex; // ����
    public string OhterDog3_Neuter; // �߼�ȭ
    public string OhterDog3_Food; // Ǫ�� Json
    public string OhterDog3_Place; // ��� Json

    [Header("Other Dog3 Condition")]
    public float OhterDog3_Health; // �ǰ�
    public float OhterDog3_Hungry; // �����
    public float OhterDog3_Tirsty; // �񸶸�
    public float OhterDog3_Clean; // û��
    public float OhterDog3_Happy; // �ູ

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
        if (instance == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������
        {
            instance = this; //���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(this.gameObject); //OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            if (instance != this) //instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ�
                Destroy(this.gameObject); //�� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ����
        }
    }

}
