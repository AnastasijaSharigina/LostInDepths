using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    NavMeshAgent agent;
    public Camera Camera_debug;
    [Tooltip("0 = player; 1 = low health hiding spot; 1 =< patrol points")]
    public GameObject [] targets;
    public float searchRadius = 1;
    public float searchTime = 1;
    public float patrolChangeTime = 5;
    public float MaxHealth = 100;
    public float Health ;
    public float LowHealthThreshold = 20;
    Vector3 randomOfSet ;
    public int currentTarget = 1;
    void ReRandom()
    {
        randomOfSet = UnityEngine.Random.insideUnitSphere * searchRadius ;
        Invoke("ReRandom", searchTime);
    }
    Ray ray;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ReRandom();
        Health = MaxHealth;
       
    }
    double nextUpdateSecond = 0;RaycastHit hit;
    void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0))
        {
             
            if (Physics.Raycast(Camera_debug.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                targets[0].transform.position = hit.point;
            }
            Debug.Log(hit.point);
        }
        if (targets[0] != null)
        {
            
            if (Health > LowHealthThreshold)
            {
                 ray = new Ray (transform.position,targets[0].transform.position-transform.position+new Vector3(0f,1f,0f)+(UnityEngine.Random.insideUnitSphere*0.2f));
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity)&&(hit.collider != null && hit.collider.name == "player colider" || hit.collider.name == " XR Origin (XR Rig)"))
                {

                    

                        currentTarget = 0;
                        nextUpdateSecond = Math.Floor(Time.time) + 1;
                    
                }
                else
                {


                    if (nextUpdateSecond == Math.Floor(Time.time))
                    {

                        nextUpdateSecond = patrolChangeTime + Math.Floor(Time.time);
                        if (currentTarget == targets.Length - 1)
                        {
                            currentTarget = 1;
                            Debug.Log(" reset " + Time.time + " " + Math.Floor(Time.time) + " " + nextUpdateSecond + " " + targets.Length);
                        }
                        else if (currentTarget < targets.Length)
                        {
                            currentTarget++;
                            Debug.Log("iterate " + Time.time + " " + Math.Floor(Time.time) + " " + nextUpdateSecond + " " +currentTarget);
                        }
                    }

                }
                
                agent.destination = targets[currentTarget].transform.position + randomOfSet;
            }
            else if (Health < LowHealthThreshold)
            {
                agent.destination = targets[1].transform.position + randomOfSet;
            }
        }
    }
    public void Take(float damage)
    {
        Health -= damage;
    }
    
}