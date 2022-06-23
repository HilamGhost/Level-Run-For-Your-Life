using UnityEngine;
using Backrooms.Player;

namespace Backrooms
{
    public enum ObstacleLenghts { Short, Medium, Long }
    public class Obstacle : MonoBehaviour
    {
        PlayerController player;
        [SerializeField] ObstacleLenghts obstacleLenght;
        [SerializeField] float ySpawn;
        public ObstacleLenghts ObstacleLenght => obstacleLenght;
        private void Awake()
        {
            OnOBSSpawn();
        }
        public void OnOBSSpawn()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, ySpawn, transform.localPosition.z);
        }
        public void GetPlayer(PlayerController _player) 
        {
            player = _player;
        }
        private void OnTriggerExit(Collider other)
        {
            if(player != null) player.IsOnObstacle = false;
        }
    }

}
