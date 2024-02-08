using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public AIType aiType = AIType.Normal;
    public NavMeshAgent agent;
    public Transform player;
    public GameObject[] followPointsGO;
    public List<Vector3> followPoints;
    public float visionDistance = 10f;
    public float minDistanceToFollowPoint = 2.5f;
    public GameObject currentDestination;
    public float visionLosingTimer = 3f;
    bool playerVisible = false;
    int currentFPIndex = 0;

    private void Start()
    {
        //get aifollowpoints
        followPointsGO = GameObject.FindGameObjectsWithTag("AIFollowPoint");
        for(int i = 0; i < followPointsGO.Length; i++)
            followPoints.Add(followPointsGO[i].transform.position);
        //get navmeshagent
        agent = GetComponent<NavMeshAgent>();
        //get player
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        currentDestination.transform.position = agent.destination;
        if (Vector3.Distance(transform.position, currentDestination.transform.position) < 0.3f) currentDestination.SetActive(false);
        else currentDestination.SetActive(true);

        if (!player.GetComponent<PlayerMovement>().pauseMenu.activeSelf)
        {
            switch (aiType)
            {
                case AIType.None: break;
                case AIType.Normal:
                    if (playerVisible)
                    {
                        agent.destination = player.transform.position;
                    }
                    else
                    {
                        DoFollowPoints();
                    }
                    break;
            }
        }
    }

    private void DoFollowPoints()
    {
        float distToCurrent = Vector3.Distance(transform.position, followPoints[currentFPIndex]);
        if(distToCurrent < 1f)
        {
            if(currentFPIndex == followPoints.Count - 1)
            {
                currentFPIndex = 0;
            }
            else
            {
                currentFPIndex++;
            }
        }
        agent.destination = followPoints[currentFPIndex];
    }
    public enum AIType
    {
        None, Normal
    }
}
