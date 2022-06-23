using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Backrooms.Player;

namespace Backrooms.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] PlayerController player;

        [Header("Texts")]
        [SerializeField] Text velocityText;
        [SerializeField] Text kilometerText;
        [SerializeField] Text dayTimeText;
        [SerializeField] Text clockTimeText;

        [Header("Real Time Values")]
        [SerializeField] string dateTime;
        [SerializeField] string clockTime;
        private void Update()
        {
            ChangeSpeedText();
            ChangeKilometerText();
            GetDayTime();
            GetClockTime();
        }
        void ChangeSpeedText() 
        {
            velocityText.text=player.PlayerVelocity.ToString("F1") + " m/s";
        }
        void ChangeKilometerText() 
        {
            kilometerText.text = GameManager.Instance.Kilometer.ToString("F1") + " km";
        }
        void GetDayTime() 
        {
            dateTime = DateTime.UtcNow.ToString("MM-dd-yyyy");
            dayTimeText.text = dateTime;
        }
        void GetClockTime() 
        {
            clockTime = DateTime.Now.ToString("H:m:s");
            clockTimeText.text = clockTime;
        }
        
    }
}
