using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject FPVision;
    public GameObject currentDestination;

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
        UpdateDebugVisionAreas();
        currentDestination.transform.position = agent.destination;
        if (Vector3.Distance(transform.position, currentDestination.transform.position) < 0.3f) currentDestination.SetActive(false);
        else currentDestination.SetActive(true);

        if (!player.GetComponent<PlayerMovement>().pauseMenu.activeSelf)
        {
            switch (aiType)
            {
                case AIType.None: break;
                case AIType.Normal:
                    //if distance to player <= visionDistance
                    if (Vector3.Distance(player.transform.position, transform.position) <= visionDistance)
                    {
                        agent.destination = player.transform.position;
                    }
                    else //if player is far
                    {
                        Vector3 closestFollowPoint = followPoints[0];
                        for (int i = 0; i < followPoints.Count; i++)
                        {
                            //if iterating FP closer than `closestFollowPoint`
                            //and iterating FP is not too close, then clstsFP = iterFP
                            float iterFPDist = Vector3.Distance(transform.position, followPoints[i]);
                            float closestFPDist = Vector3.Distance(transform.position, closestFollowPoint);
                            if (iterFPDist < closestFPDist && iterFPDist >= minDistanceToFollowPoint) closestFollowPoint = followPoints[i];
                        }
                        agent.destination = closestFollowPoint;
                    }
                    break;
            }
        }
    }

    public void UpdateDebugVisionAreas()
    {
        if (FPVision != null)
        {
            FPVision.transform.localScale = new Vector3(visionDistance, 0.1f, visionDistance);
        }
    }

    public enum AIType
    {
        None, Normal
    }
}
