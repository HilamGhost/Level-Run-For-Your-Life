using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Backrooms.Player;
using Backrooms.Room;
using Backrooms.Enemy;

namespace Backrooms
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Other Scripts")]
        [SerializeField] PlayerController player;
        [SerializeField] BaseRoom baseRoom;
        [SerializeField] Smiler enemy;

        [Header("Kilometer")]
        [SerializeField] float kilometer;
        [SerializeField] float wantedKilometer = 10;
        Vector3 previousPos;

        [Header("End Game")]
        [SerializeField] GameObject endDoor;
        [SerializeField] GameObject endLoop;
        [SerializeField] float endGameFadeOutTime=1;
        [SerializeField] Image endGameEffect;

        [Header("Cameras")]
        [SerializeField] Camera mainCamera;
        [SerializeField] Camera js_Camera;


        public float Kilometer => kilometer;
        void Start()
        {
            LockCursor();
            previousPos = player.transform.position;
            StartGame();
        }

        void FixedUpdate()
        {
            CalculateKilometer();
        }

        void CalculateKilometer() 
        {
            if(previousPos != player.transform.position) 
            {
                kilometer += Vector3.Distance(player.transform.position, previousPos)/1000;
                previousPos = player.transform.position;
            }          
        }
        public bool CheckIsItEnd() 
        {
            if (kilometer >= wantedKilometer) 
            {
                endDoor.SetActive(true);
                endLoop.SetActive(false);
                return true;
            }
            else 
            {
                return false;
            }
        } 
        public void EndGame() 
        {
            enemy.gameObject.SetActive(false);
            player.enabled = false;
            endGameEffect.DOFade(1, endGameFadeOutTime).OnComplete(()=> ChangeScene("EndGame"));
        }
        public void ChangeScene(string _sceneName) 
        {
            SceneManager.LoadScene(_sceneName);
        }
        public void StartGame() 
        {
            endGameEffect.enabled = true;
            endGameEffect.DOFade(0, endGameFadeOutTime);
        }
        public void DeathGame() 
        {
            StartCoroutine(RestartGame());
            js_Camera.transform.position = player.transform.position;
            enemy.gameObject.SetActive(false);
            player.enabled = false;
            mainCamera.enabled = false;
            js_Camera.gameObject.SetActive(true);
        }
        public IEnumerator RestartGame() 
        {
            yield return new WaitForSeconds(7);
            UnlockCursor();
            ChangeScene("MainMenu");
        }
        #region Cursor Methods
        void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        #endregion

    }
}
