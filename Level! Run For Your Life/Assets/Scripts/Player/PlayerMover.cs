using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Player
{
    public class PlayerMover
    {
        PlayerController player;
        Vector3 impact;
        public PlayerMover(PlayerController _player) 
        {
            player = _player;
        }
        

        public void Move(Vector3 _moveVector,float _moveSpeed) 
        {
            Vector3 _moveVectorWithGravity = (player.transform.right*_moveVector.x)+ (player.transform.forward * _moveVector.z) + Physics.gravity;
            player.PlayerCharacterController.Move(_moveVectorWithGravity*_moveSpeed*Time.deltaTime);
        }
        public void KnockBack() 
        {
            impact = Vector3.Lerp(impact,Vector3.zero,5*Time.deltaTime);
            player.PlayerCharacterController.Move(impact);
        }
        public void AddImpact(Vector3 dir) 
        {
            dir.Normalize();
            impact = -dir * Time.deltaTime * player.PlayerVelocity*2;
        }
        public IEnumerator Collapse() 
        {           
            player.CanMove = false;
            player.IsKnockedBack = true;
            AddImpact(player.PlayerCharacterController.velocity);
            yield return new WaitForSeconds(0.5f);
            player.IsKnockedBack = false;
            yield return new WaitForSeconds(0.05f);
            player.CanMove = true;

        }
    }
}
