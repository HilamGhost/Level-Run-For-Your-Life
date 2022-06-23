using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Backrooms.Player
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMover playerMover;
        PlayerCameraSettings playerCameraSettings;
        PlayerAnimation playerAnimation;
        PlayerVault playerVault;
        PlayerAudioManager playerAudioManager;

        CharacterController playerCharacterController;

        [Header("Camera")]
        [SerializeField] GameObject virtualCameraObject;
        [Header("Move")]
        [SerializeField] bool canMove;
        [SerializeField] bool isKnockedBack;
        [SerializeField] bool canVault;
        [SerializeField] bool isOnObstacle;
        [SerializeField] SpeedType playerSpeedType;
        [SerializeField] float playerVelocity;
        [SerializeField] float walkSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] Vector2 moveInput; //I used Vector2 because I wanted to make more readable Inspector
        [Header("Sounds")]
        [SerializeField] AudioSource playerAudioSource;
        [SerializeField] AudioSource playerBreathAudioSource;
        [SerializeField] List<AudioClip> playerFootSteps;
        [SerializeField] AudioClip playerVaultSound;
        [SerializeField] AudioClip playerCrashSound;

        #region Private Fields
        //Transform
        Vector3 _lastPos = Vector3.zero;
        //Speed
        float _speed;
        float _maxSpeed;

        //Vault
        Obstacle vaultObstacle;
        #endregion        
        #region Component Properties
        public PlayerCameraSettings PlayerCamera => playerCameraSettings;
        public PlayerAudioManager PlayerAudioManager => playerAudioManager;
        public CharacterController PlayerCharacterController => playerCharacterController;
        public GameObject VirtualCameraObject => virtualCameraObject;
        public float PlayerVelocity 
        {
            get => playerVelocity;
            private set => playerVelocity = value;
        }
        public List<AudioClip> PlayerFootSteps => playerFootSteps;
        public AudioClip PlayerVaultSound => playerVaultSound;
        #endregion
        #region Field Properties
        public Vector3 MoveInput
        {
            get 
            {
                Vector3 newInput = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
                return newInput;
            }
            set 
            {

                moveInput = new Vector2(MoveInput.x, MoveInput.y);
            }
        }
        public SpeedType PlayerSpeedType 
        {
            get 
            {
                SpeedType _curSpeedType;
                if (Mathf.Approximately(PlayerVelocity, 0))
                {
                    _curSpeedType = SpeedType.Stopped;
                    PlayerVelocity = 0;
                }
                else if (playerVelocity <= walkSpeed + 1.5f)
                {
                    _curSpeedType = SpeedType.Slow;
                }
                else
                {
                    _curSpeedType = SpeedType.Fast;
                }
                playerSpeedType = _curSpeedType;
                return _curSpeedType;
            }
        }
        public bool CanMove 
        {
            get => canMove;
            set => canMove = value;
        }
        public bool IsKnockedBack 
        {
            get => isKnockedBack;
            set => isKnockedBack = value;
        }
        public bool CanVault
        {
            get => canVault;
            set => canVault = value;
        }
        public bool IsOnObstacle
        {
            get => isOnObstacle;
            set => isOnObstacle = value;
        }
        public float MoveSpeed 
        {
            get 
            {
                if (Input.GetKey(KeyCode.LeftShift)) 
                {
                    _maxSpeed = runSpeed;
                }
                else 
                {
                    _maxSpeed = walkSpeed;
                }
                if (!Mathf.Approximately(_speed, _maxSpeed)) 
                {
                    _speed = Mathf.Lerp(_speed,_maxSpeed,Time.deltaTime);
                    return _speed;
                }
                else 
                {
                    return _maxSpeed;
                }
            }           
        }
        public Obstacle VaultObstacle 
        {
            get => vaultObstacle;            
            set => vaultObstacle = value;
        }
        #endregion
        void Start()
        {
            MakeAssignment();
        }

        
        void Update()
        {
            playerCameraSettings.PlayerCameraUpdate();
            PlayAnimationAndSound();
            playerVault.DedectOBS();
            if (canMove) 
            {
                if (canVault)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        playerVault.Vault(VaultObstacle, playerSpeedType);
                    }
                }
            } 
            
        }
        private void FixedUpdate()
        {
            playerVelocity = CheckPlayerVelocity();
            if (CanMove) 
            { 
                playerMover.Move(MoveInput, MoveSpeed);
                playerCameraSettings.UnlockCamera();       
            }
            else 
            {
                playerCameraSettings.LockCamera();
                if (IsKnockedBack) 
                {
                    playerMover.KnockBack();                    
                }
            }
        }
        public float CheckPlayerVelocity() 
        {
            float speed = (transform.position - _lastPos).magnitude/Time.deltaTime;
            _lastPos = transform.position;
            return speed;
        }
        
        void PlayAnimationAndSound() 
        {
            if (CanMove)
            {
                switch (PlayerSpeedType)
                {
                    case SpeedType.Stopped:
                        playerAnimation.PlayIdleAnimation();
                       StartCoroutine(playerAudioManager.PlayFootstepSound(false));
                        break;
                    case SpeedType.Slow:
                        playerAnimation.PlayWalkAnimation();
                        StartCoroutine(playerAudioManager.PlayFootstepSound(true));
                        break;
                    case SpeedType.Fast:
                        playerAnimation.PlayRunAnimation();
                        StartCoroutine(playerAudioManager.PlayFootstepSound(true,true));
                        break;
                }
                if (PlayerSpeedType == SpeedType.Fast) 
                {
                    playerAudioManager.PlayBreathSound(true);
                }
                else if(PlayerSpeedType == SpeedType.Stopped)
                {
                    playerAudioManager.PlayBreathSound(false);
                }
            }
            else 
            {
                
            }
        }
        void MakeAssignment() 
        {
            playerMover = new PlayerMover(this);
            playerCameraSettings = new PlayerCameraSettings(this);
            playerAnimation = new PlayerAnimation(this);
            playerVault = new PlayerVault(this);
            playerAudioManager = new PlayerAudioManager(this,playerAudioSource,playerBreathAudioSource);

            playerCharacterController = GetComponent<CharacterController>();
        }

        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Crashable"))
            {
                if (playerVelocity > 4f && !isOnObstacle) 
                {
                    AudioManager.Instance.PlayOneShotSound(playerAudioSource,playerCrashSound);
                    playerCameraSettings.ShakeCamera(4f, 0.5f);
                    StartCoroutine(playerMover.Collapse());
                }               
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("End")) 
            {
                playerVelocity = 0;
                GameManager.Instance.EndGame();
            }
        }
    }
}
