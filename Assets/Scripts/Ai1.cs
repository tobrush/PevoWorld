using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai1 : MonoBehaviour
{
    public GameObject[] playerChacater;

    private NavMeshAgent agent;

    public float radius;

    public float randomTime;

    public int randomStop;

    public Animator anim;

    public TouchManager touchManager;

    public Canvas canvas;

    private void Start()
    {
        playerChacater[int.Parse(OtherUserData.instance.OhterDog1_Character)-2].SetActive(true);
        anim = this.transform.GetChild(int.Parse(OtherUserData.instance.OhterDog1_Character) +2).GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine("DOGAI");
    }
 

    private void Update()
    {
        canvas.transform.LookAt(Camera.main.gameObject.transform);

        if (touchManager.handOn)
        {
            agent.SetDestination(transform.position);
            anim.SetBool("Walk", false);
        }

            if (agent.desiredVelocity.magnitude <= 0.1f)
        {
            anim.SetBool("Walk", false);
        }
    }
    IEnumerator DOGAI()
    {
        if (!touchManager.handOn)
        {

            if (!agent.hasPath)
            {
                //GetPoint.Instance.GetRandomPoint(transform, radius);

                randomStop = Random.Range(1, 10);
                if (randomStop >= 5)
                {
                    anim.SetBool("Walk", true);
                    agent.SetDestination(GetPoint.Instance.GetRandomPoint());
                    //agent.SetDestination(GetPoint.Instance.GetRandomPoint(transform, radius));

                }
                else
                {
                    agent.SetDestination(transform.position);
                    yield return new WaitForSeconds(3.0f);
                    anim.SetBool("Walk", false);
                }

            }


        }

        randomTime = Random.Range(0.5f, 1.0f);

        yield return new WaitForSeconds(randomTime);


        StartCoroutine("DOGAI");
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

#endif
}