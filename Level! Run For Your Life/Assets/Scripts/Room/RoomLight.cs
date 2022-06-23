using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Backrooms.Room
{
    public class RoomLight : MonoBehaviour
    {
        Light _light;
        void Start()
        {
            _light = GetComponent<Light>();
            OpenLight();
        }
        
        void OpenLight() 
        {
            _light.DOIntensity(50, 1).OnComplete(CloseLight);
        }
        void CloseLight() 
        {
            _light.DOIntensity(0, 1).OnComplete(OpenLight);
        }
    }
}
