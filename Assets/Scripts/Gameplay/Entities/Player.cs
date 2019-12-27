using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.AI;

public partial class Player : MonoBehaviour
{
    [Header("Gameplay Info")]
    public NavMeshAgent agent;
    public DynamicCharacterAvatar avatar;

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

    private void Start()
    {
        agent.enabled = true;
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

        if (targetPosition != lastTargetPosition)
        {
            agent.SetDestination(targetPosition);
            lastTargetPosition = targetPosition;
        }

        if (needUpdate)
        {
            //TODO: update uma data here.
            string raceName = "";
            raceName = bodyType == 0 ? "HumanMale" : raceName;
            raceName = bodyType == 1 ? "HumanFemale" : raceName;
            avatar.ChangeRace(raceName);
            
            needUpdate = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPosition, 0.3f);   
    }
}
