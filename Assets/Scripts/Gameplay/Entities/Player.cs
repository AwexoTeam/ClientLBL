using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Player : MonoBehaviour
{
    [Header("Gameplay Info")]
    public NavMeshAgent agent;

    public Vector3 targetPosition;
    private Vector3 lastTargetPosition;
    
    public bool needUpdate = false;

    [Header("Base Info")]
    public int characterID;
    public string displayName;

    private bool hasInit;

    private bool isLocalPlayer
    {
        get { return this == PlayerManager.instance.localPlayer; }
    }
    
    private void Update()
    {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    PlayerMovementRequest request =
                        new PlayerMovementRequest(hit.point.x, hit.point.y, hit.point.z);

                    request.Serialize();
                    request.Send();
                }
            }   
        }

        //if (targetPosition != lastTargetPosition)
        //{
        //    agent.destination = targetPosition;
        //    lastTargetPosition = targetPosition;
        //}

        if (needUpdate)
        {
            //TODO: update uma data here.
            needUpdate = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPosition, 2);   
    }
}
