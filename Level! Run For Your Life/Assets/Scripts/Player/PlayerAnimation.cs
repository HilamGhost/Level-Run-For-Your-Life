using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Player
{
    public class PlayerAnimation
    {
        PlayerController player;
        public PlayerAnimation(PlayerController _player) 
        {
            player = _player;
        }
        public void StopAnimation()
        {
            player.PlayerCamera.ShakeCameraLoop(0,0);
        }
        public void PlayIdleAnimation() 
        {
            player.PlayerCamera.ShakeCameraLoop(1f,0.3f);
        }
        public void PlayWalkAnimation()
        {
            player.PlayerCamera.ShakeCameraLoop(0.25f, 1f);
        }
        public void PlayRunAnimation()
        {
            player.PlayerCamera.ShakeCameraLoop(1f, 2f);
        }
    }
}
