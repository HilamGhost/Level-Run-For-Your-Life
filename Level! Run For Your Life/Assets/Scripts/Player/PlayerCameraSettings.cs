using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Backrooms.Player
{
    public class PlayerCameraSettings
    {
        PlayerController player;
        CinemachineVirtualCamera playerCamera;
        CinemachinePOV playerPOV;
        CinemachineBasicMultiChannelPerlin playerCMShaker;

        float shakeTimer;
        float shakeTimerTotal;
        float startingIntensity;
        public PlayerCameraSettings(PlayerController _player) 
        {
            player = _player;

            playerCamera = player.VirtualCameraObject.GetComponent<CinemachineVirtualCamera>();
            playerPOV = playerCamera.GetCinemachineComponent<CinemachinePOV>();
            playerCMShaker = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        }
        public float XRotation 
        {
            get => playerPOV.m_HorizontalAxis.Value;
        }
        
        public void PlayerCameraUpdate()
        {
            ChangePlayerLookDirection();
           
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                StopCameraShake();
            }
            

        }
        public void ShakeCamera(float _amplitudeGain,float _shakeTime) 
        {
            playerCMShaker.m_FrequencyGain = 1;
            playerCMShaker.m_AmplitudeGain = _amplitudeGain;
            shakeTimer = _shakeTime;
            shakeTimerTotal = shakeTimer;
            startingIntensity =_amplitudeGain;
        }
        public void ShakeCameraLoop(float _amplitudeGain,float _frequencyGain)
        {
            playerCMShaker.m_FrequencyGain = _frequencyGain;
            playerCMShaker.m_AmplitudeGain = _amplitudeGain;
        }
        void StopCameraShake()
        {          
            playerCMShaker.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
        public void LookObject(Transform _object) 
        {
            playerCamera.LookAt = _object;
        }
        public void LookPlayer() 
        {           
            playerCamera.LookAt = player.transform;
        }
        public void LockCamera() 
        {
            playerPOV.m_HorizontalAxis.m_MaxSpeed = 0;
            playerPOV.m_VerticalAxis.m_MaxSpeed = 0;
        }
        public void UnlockCamera() 
        {
            playerPOV.m_HorizontalAxis.m_MaxSpeed = 300;
            playerPOV.m_VerticalAxis.m_MaxSpeed = 300;
        }
        public void ChangePlayerLookDirection() 
        {
            player.transform.localRotation = Quaternion.Euler(Vector3.up * XRotation);
        }
    }
}
