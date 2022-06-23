using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Backrooms.Room
{
    public class BaseRoom : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] GameObject player;
        [Header("Enemy")]
        [SerializeField] GameObject enemy;

        [Header("Loop")]
        [SerializeField] GameObject playerStartLoopRoom;
        [SerializeField] GameObject enemyStartLoopRoom;

        [Header("Obstacle Spawn")]
        [SerializeField] GameObject obstacleParent;
        [SerializeField, Range(65, -65)] float roomLengt;
        [SerializeField, Range(2.46f, 8.93f)] float roomWidth;
        [SerializeField] List<GameObject> obstacles;
        private void Start()
        {
            SpawnOBS();
        }
        public void TeleportPlayerToTheLoopStart(Teleporter _teleporter) 
        {
            player.transform.position = playerStartLoopRoom.transform.position;
            SpawnOBS();
            if (GameManager.Instance.CheckIsItEnd()) 
            {
                Destroy(_teleporter);
            }
        }
        public void TeleportEnemyToTheLoopStart(Teleporter _teleporter) 
        {
            enemy.transform.position = enemyStartLoopRoom.transform.position;
        }
        [ContextMenu("Create OBS")]
        public void SpawnOBS() 
        {
            foreach (Transform child in obstacleParent.transform)
            {
                Destroy(child.gameObject);
            }
            int _randomNumber = Random.Range(15,25);
            for (int i = 0; i < _randomNumber; i++)
            {
                int _randomOBS = Random.Range(0, 7);
                Vector3 _randomOBSPos = new Vector3(Random.Range(-65, 60),0,Random.Range(2.46f, 8.5f));
                GameObject _createdOBS = Instantiate(obstacles[_randomOBS], _randomOBSPos, Quaternion.identity, obstacleParent.transform);
                _createdOBS.transform.localPosition = _randomOBSPos;
                _createdOBS.GetComponent<Obstacle>().OnOBSSpawn();
            }
        }
    }
}
