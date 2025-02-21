using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class RaceMan
{
    public int index = 0;
    public int speed;
    public int power;
    public int stamina;
    public int sense;
    public int intelligence;
    public int grit;

}


public enum RunState { Ready, Starting ,  Maximum , Exhaust, LastSpurt, GoalIn }

// Ready = 게임시작전 카운다운후 Starting으로 변경
// Starting = 예열단계. 지정된 가속도로 최고속도에 도달할때까지 달림 = > 최고속도도달했다면 Maximum 으로 변경
// Maximum = 최대속도로 달리는중. 스테미나 소모가 모두 끝날때까지 달리고 Exhaust로 변경
// Exhaust = 구간에서 골인 직전 300m 구간에서 LastSpurt로 변경
// LastSpurt = 근성파라미터에 영향받아 마지막골인지점까지 속도유지
// GoalIn= 이후 세레모니 애니메이션



public class RaceTest : MonoBehaviour
{
    public RunState runState; 

    public Text ReadyNumber;
    bool GameStart = false;

    public int rank;

    public List<RaceMan> RaceManList;

    public GameObject[] RaceManObj;

    public float Player1_speed;
    public float Player2_speed;
    public float Player3_speed;
    public float Player4_speed;
    public float Player5_speed;


    [SerializeField] private AnimationCurve Player1_SpeedCurve;
    [SerializeField] private AnimationCurve Player2_SpeedCurve;
    [SerializeField] private AnimationCurve Player3_SpeedCurve;
    [SerializeField] private AnimationCurve Player4_SpeedCurve;
    [SerializeField] private AnimationCurve Player5_SpeedCurve;

    public float raceTime;
    [SerializeField] private float period = 2f;

    private Vector3 dir = Vector3.right;

    public void Awake()
    {
        rank = 1;

        // player 1
        Player1_SpeedCurve.AddKey(new Keyframe(0.3f - (RaceManList[0].sense * 0.002f), (RaceManList[0].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });


        Player1_SpeedCurve.AddKey(new Keyframe((RaceManList[0].stamina * 0.002f)+ 0.4f , (RaceManList[0].speed * 0.005f )+ 0.5f)
        {
          inTangent = 0,
          outTangent = 0,
          tangentMode = 0
        });
        Player1_SpeedCurve.AddKey(new Keyframe(0.8f, (RaceManList[0].speed * 0.005f) + 0.3f + (RaceManList[0].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player1_SpeedCurve.MoveKey(4, new Keyframe(0.8f, (RaceManList[0].speed * 0.005f) + 0.3f + (RaceManList[0].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });

        // player 2
        Player2_SpeedCurve.AddKey(new Keyframe(0.3f - (RaceManList[1].sense * 0.002f), (RaceManList[1].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });


        Player2_SpeedCurve.AddKey(new Keyframe((RaceManList[1].stamina * 0.002f) + 0.4f, (RaceManList[1].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player2_SpeedCurve.AddKey(new Keyframe(0.8f, (RaceManList[1].speed * 0.005f) + 0.3f + (RaceManList[1].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player2_SpeedCurve.MoveKey(4, new Keyframe(0.8f, (RaceManList[1].speed * 0.005f) + 0.3f + (RaceManList[1].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });

        // player 3
        Player3_SpeedCurve.AddKey(new Keyframe(0.3f - (RaceManList[2].sense * 0.002f), (RaceManList[2].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });


        Player3_SpeedCurve.AddKey(new Keyframe((RaceManList[2].stamina * 0.002f) + 0.4f, (RaceManList[2].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player3_SpeedCurve.AddKey(new Keyframe(0.8f, (RaceManList[2].speed * 0.005f) + 0.3f + (RaceManList[2].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player3_SpeedCurve.MoveKey(4, new Keyframe(0.8f, (RaceManList[2].speed * 0.005f) + 0.3f + (RaceManList[2].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });

        // player 4
        Player4_SpeedCurve.AddKey(new Keyframe(0.3f - (RaceManList[3].sense * 0.002f), (RaceManList[3].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });


        Player4_SpeedCurve.AddKey(new Keyframe((RaceManList[3].stamina * 0.002f) + 0.4f, (RaceManList[3].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player4_SpeedCurve.AddKey(new Keyframe(0.8f, (RaceManList[3].speed * 0.005f) + 0.3f + (RaceManList[3].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player4_SpeedCurve.MoveKey(4, new Keyframe(0.8f, (RaceManList[3].speed * 0.005f) + 0.3f + (RaceManList[3].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });

        // player 5
        Player5_SpeedCurve.AddKey(new Keyframe(0.3f - (RaceManList[4].sense * 0.002f), (RaceManList[4].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });


        Player5_SpeedCurve.AddKey(new Keyframe((RaceManList[4].stamina * 0.002f) + 0.4f, (RaceManList[4].speed * 0.005f) + 0.5f)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        Player5_SpeedCurve.AddKey(new Keyframe(0.8f, (RaceManList[4].speed * 0.005f) + 0.3f + (RaceManList[4].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });

        Player5_SpeedCurve.MoveKey(4, new Keyframe(0.8f, (RaceManList[4].speed * 0.005f) + 0.3f + (RaceManList[4].intelligence * 0.002f))
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });


        /*
        Player5_SpeedCurve.MoveKey(4, new Keyframe(1, (RaceManList[4].speed * 0.005f) + 0.3f + (RaceManList[4].intelligence * 0.002f) + ((1 - (RaceManList[4].speed * 0.005f) + 0.3f + (RaceManList[4].intelligence * 0.002f)) * 0.01f) * RaceManList[4].grit)
        {
            inTangent = 0,
            outTangent = 0,
            tangentMode = 0
        });
        */

        //TO DO : 스텟에 맞게 커브 조정
        //AnimationCurve.MoveKey(keyIndex, newKey)
        //AnimationCurve curve;
        // Keyframe boxed = curve.keys[i];
        //curve.MoveKey(0, boxed);
    }
    public void Start()
    { 
        StartCoroutine(GameReady());
    }

    IEnumerator GameReady()
    {
        ReadyNumber.gameObject.SetActive(true);
        ReadyNumber.text = "3";
        yield return new WaitForSeconds(1);
        ReadyNumber.text = "2";
        yield return new WaitForSeconds(1);
        ReadyNumber.text = "1";
        yield return new WaitForSeconds(1);
        ReadyNumber.text = "Race!";
        yield return new WaitForSeconds(1);
        ReadyNumber.gameObject.SetActive(false);
        GameStart = true;
    }

    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Goal!");
        if(col.gameObject == RaceManObj[0])
        {
            GoalMan(RaceManList[0]);
        }
        if (col.gameObject == RaceManObj[1])
        {
            GoalMan(RaceManList[1]);
        }
        if (col.gameObject == RaceManObj[2])
        {
            GoalMan(RaceManList[2]);
        }
        if (col.gameObject == RaceManObj[3])
        {
            GoalMan(RaceManList[3]);
        }
        if (col.gameObject == RaceManObj[4])
        {
            GoalMan(RaceManList[4]);
        }
    }

    public void FixedUpdate()
    {
        if(GameStart)
        {
            // Start Line
            Debug.DrawLine(Vector3.zero, new Vector3(0, 0, 10), Color.red);
            // Goal Line
            Debug.DrawLine(new Vector3(10, 0, 0), new Vector3(10, 0, 10), Color.red);


            raceTime += Time.deltaTime;
            
            
            //if (curTime >= period)
            //{
            //    curTime -= curTime; // 시간제한
            //}



            Player1_speed = Player1_SpeedCurve.Evaluate(raceTime / 10);
            Player2_speed = Player2_SpeedCurve.Evaluate(raceTime / 10);
            Player3_speed = Player3_SpeedCurve.Evaluate(raceTime / 10);
            Player4_speed = Player4_SpeedCurve.Evaluate(raceTime / 10);
            Player5_speed = Player5_SpeedCurve.Evaluate(raceTime / 10);

           
            

            if(RaceManList[0].index == 0)
            {
                RaceManObj[0].GetComponent<Rigidbody>().velocity = (dir * Player1_speed);
            }
            else
            {
                RaceManObj[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if (RaceManList[1].index == 0)
            {
                RaceManObj[1].GetComponent<Rigidbody>().velocity = (dir * Player2_speed);
            }
            else
            {
                RaceManObj[1].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if (RaceManList[2].index == 0)
            {
                RaceManObj[2].GetComponent<Rigidbody>().velocity = (dir * Player3_speed);
            }
            else
            {
                RaceManObj[2].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if (RaceManList[3].index == 0)
            {
                RaceManObj[3].GetComponent<Rigidbody>().velocity = (dir * Player4_speed );
            }
            else
            {
                RaceManObj[3].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if (RaceManList[4].index == 0)
            {
                RaceManObj[4].GetComponent<Rigidbody>().velocity = (dir * Player5_speed);
            }
            else
            {
                RaceManObj[4].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

        }

    }

    public void GoalMan(RaceMan raceman)
    {
        raceman.index = rank;
        rank = rank + 1;
    }
}




