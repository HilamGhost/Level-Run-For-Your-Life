using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Player
{
    public class PlayerAudioManager 
    {
        AudioSource playerAudioSource;
        AudioSource playerBreathAudioSource;
        PlayerController player;
        int footstepSound = 0;
        bool isPlaying;

        public PlayerAudioManager(PlayerController _player, AudioSource _playerAudioSource,AudioSource _playerBreathAudioSource) 
        {
            player = _player;
            playerAudioSource = _playerAudioSource;
            playerBreathAudioSource = _playerBreathAudioSource;
        }

        public IEnumerator PlayFootstepSound(bool isMoving,bool _isFast = false) 
        {
            if (isMoving && !isPlaying) 
            {
                if (!_isFast)
                {
                    isPlaying = true;
                    Debug.Log("Not Fast");
                    AudioManager.Instance.PlayOneShotSound(playerAudioSource, player.PlayerFootSteps[footstepSound]);
                    yield return new WaitForSeconds(0.5f);
                    isPlaying = false;
                    if (footstepSound == 0) footstepSound++; else if (footstepSound == 1) footstepSound--;
                }
                else
                {
                    isPlaying = true;
                    Debug.Log("Fast");
                    AudioManager.Instance.PlayOneShotSound(playerAudioSource, player.PlayerFootSteps[footstepSound]);
                    yield return new WaitForSeconds(0.35f);
                    isPlaying = false;
                    if (footstepSound == 0) footstepSound++; else if (footstepSound == 1) footstepSound--;
                }
            }    
        }
        public void PlayVaultSound() 
        {
            AudioManager.Instance.PlayOneShotSound(playerAudioSource, player.PlayerVaultSound);
        }
        public void PlayBreathSound(bool canPlay) 
        {
            if (canPlay) 
            {
                AudioManager.Instance.PlayLoopSound(playerBreathAudioSource);
            }
            else 
            {
                if (playerBreathAudioSource.isPlaying) 
                {
                    AudioManager.Instance.StopSound(playerBreathAudioSource,5);
                }
                
            }
            
        }
    }
}
