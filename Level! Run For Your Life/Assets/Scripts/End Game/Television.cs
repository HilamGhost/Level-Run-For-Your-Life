using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.TV
{
    public class Television : MonoBehaviour
    {
        [SerializeField] Transform player;
        void Update()
        {
            transform.LookAt(player);
        }
    }
}
