using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public PlayerEntity entityData;
    private Camera cam;
    
    private void Start()
    {
        cam = NetworkManager.instance.mainCamera;
        InvokeRepeating("TransformSync", 0, PlayerManager.instance.updateFrequency);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                Packet packet = new Packet(PacketType.MovementRequest);
                packet.BeginWrite();
                packet.writer.Write(entityData.GUID);
                packet.writer.Write(hit.point.x);
                packet.writer.Write(hit.point.y);
                packet.writer.Write(hit.point.z);
                packet.EndWrite();

                NetworkManager.instance.Send(packet);
            }
        }
    }

    private void TransformSync()
    {
        PlayerManager.instance.SyncPosition(entityData.currPosition);
    }
}
