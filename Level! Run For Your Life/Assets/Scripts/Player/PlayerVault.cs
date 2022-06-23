using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Backrooms.Player
{
    public class PlayerVault
    {
        PlayerController player;

        public PlayerVault(PlayerController _player) 
        {
            player = _player;
        }
        public void DedectOBS() 
        {
            RaycastHit _raycastHit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _raycastHit, 5, LayerMask.GetMask("OBS"))) 
            {
                Debug.Log(_raycastHit.transform.name);
                player.CanVault = true;
                if(player.VaultObstacle == null)
                    player.VaultObstacle = _raycastHit.transform.GetComponentInParent<Obstacle>();
                player.VaultObstacle.GetPlayer(player);
            }
            else 
            {
                player.VaultObstacle = null;
                player.CanVault = false;
            }
        }
        public void Vault(Obstacle _obs,SpeedType _playerSpeedType) 
        {
            float _vaultSpeed = 0;
            float _obstacleLenght = 0;
            float _maxVaultSpeed = 2.5f; //ObstacleLenghts.Long: _vaultSpeed + SpeedType.Stopped: _vaultSpeed

            switch (_obs.ObstacleLenght) 
            {
                case ObstacleLenghts.Short: _vaultSpeed += 0.5f;_obstacleLenght = 1; break;
                case ObstacleLenghts.Medium: _vaultSpeed += 1f; _obstacleLenght = 2; break;
                case ObstacleLenghts.Long: _vaultSpeed += 1.5f; _obstacleLenght = 3; break;
            }
            switch (_playerSpeedType) 
            {
                case SpeedType.Stopped: _vaultSpeed += 1f; break;
                case SpeedType.Slow: _vaultSpeed += 0.5f; break;
                case SpeedType.Fast: _vaultSpeed -= 0.5f; break;
            }

            player.PlayerAudioManager.PlayVaultSound();
            player.CanMove = false;
            player.PlayerCamera.LookObject(_obs.transform);
            player.transform.parent = _obs.transform;
            player.PlayerCamera.ShakeCamera(10f, _maxVaultSpeed - 2 - _vaultSpeed);
            player.transform.DOLocalJump(Vector3.up*_obstacleLenght, (_maxVaultSpeed-2-_vaultSpeed), 1,_vaultSpeed).OnComplete(ResetPlayer);
            
            
            
        }
        void ResetPlayer() 
        {
            player.IsOnObstacle = true;
            player.transform.parent = null;
            player.PlayerCamera.LookPlayer();
            player.CanMove = true;
            player.CanVault = false;
        }
    }
}
