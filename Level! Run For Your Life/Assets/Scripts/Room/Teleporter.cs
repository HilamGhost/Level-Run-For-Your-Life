using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Room
{
    public class Teleporter : MonoBehaviour
    {
        BaseRoom baseRoom;
        private void Awake()
        {
            baseRoom = GetComponentInParent<BaseRoom>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                baseRoom.TeleportPlayerToTheLoopStart(this);
                baseRoom.TeleportEnemyToTheLoopStart(this);
            }
            else if (other.CompareTag("Enemy"))
            {
                baseRoom.TeleportEnemyToTheLoopStart(this);
            }
        }
    }
}
