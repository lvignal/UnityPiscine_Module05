using System;
using Module05.Enemy;
using Module05.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Module05.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _startPosition;
        
        [Header ("Sounds")]
        [SerializeField] private AudioSource _walkSound;
        [SerializeField] private AudioSource _jumpSound;
        [SerializeField] private AudioSource _hurtSound;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private AudioSource _respawnSound;
        [SerializeField] private AudioSource _collectibleSound;
        
        private bool _isOnGround;
        private bool _isTouchingWall;
        private float _jumpForce = 9;
        private float _speed = 8;
        private float _minXPosition = -7f;

        private int _healthPoints = 3;
        public int HealthPoints
        {
            get => _healthPoints;
            private set
            {
                _healthPoints = value;
                UIManager.Instance.UpdatePlayerHealthPoints(_healthPoints);
            }
        }

        private int _numberOfCollectedLeaves;
        public int NumberOfCollectedLeaves
        {
            get => _numberOfCollectedLeaves;
            private set
            {
                _numberOfCollectedLeaves = value;
                UIManager.Instance.UpdatePlayerLeafPoints(_numberOfCollectedLeaves * 5);
            }
        }
        
        private int _numberOfDeaths;
        
        private Rigidbody2D _rigidBody;
        private Animator _animator;
        private bool _isGoingRight = true;

        public Action OnPlayerDeath;
        public Action OnEndReached;
        
        private const string HURT = "Hurt";
        private const string DEATH = "Death";
        private const string JUMP = "Jump";
        private const string TURN = "Turn";
        private const string RESPAWN = "Respawn";
        private const string XVELOCITY = "xVelocity";

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            transform.position = _startPosition.position;
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            // avoid to go out of the screen on the left
            if (transform.position.x >= _minXPosition)
                _rigidBody.velocity = new Vector2(horizontalInput * _speed, _rigidBody.velocity.y);
            else
                transform.position = new Vector3(_minXPosition, transform.position.y, transform.position.z);
            
            // turn
            bool isGoingRight = horizontalInput > 0;
            if (_isGoingRight != isGoingRight && horizontalInput != 0)
            {
                _isGoingRight = isGoingRight;
                _animator.SetTrigger(TURN);
            }
            
            // jump
            if (Input.GetKeyDown(KeyCode.Space) && _isOnGround && !_isTouchingWall)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
                _jumpSound.Play();
                _animator.SetTrigger(JUMP);
                _isOnGround = false;
            }
            
            // if not moving, it is "idle" animation, else it is "walk"
            _animator.SetFloat(XVELOCITY, Mathf.Abs(_rigidBody.velocity.x));
            
            // play "idle" animation when touching a wall to avoid passing through it
            if (_isTouchingWall)
                _animator.SetFloat(XVELOCITY, 0);
            
            
            if (Mathf.Abs(_rigidBody.velocity.x) > 0 && _isOnGround && !_isTouchingWall && !_walkSound.isPlaying)
                _walkSound.Play();
            else if (Mathf.Abs(_rigidBody.velocity.x) == 0 || _isTouchingWall)
                _walkSound.Stop();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<TilemapCollider2D>())
            {
                if (other.gameObject.tag == "Ground")
                    _isOnGround = true;
                else if (other.gameObject.tag == "Wall")
                    _isTouchingWall = true;
            }
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<TilemapCollider2D>())
            {
                if (other.gameObject.tag == "Ground")
                    _isOnGround = false;
                
                else if (other.gameObject.tag == "Wall")
                    _isTouchingWall = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<DetectionZone>())
            {
                other.gameObject.GetComponentInParent<EnemyController>().OnHitPlayer -= ReactToHit;
                other.gameObject.GetComponentInParent<EnemyController>().OnHitPlayer += ReactToHit;
            }
            
            if (other.tag == "Collectible")
            {
                NumberOfCollectedLeaves++;
                _collectibleSound.Play();
                other.gameObject.SetActive(false);
            }

            if (other.tag == "Finish")
            {
                if (NumberOfCollectedLeaves >= 5)
                    OnEndReached?.Invoke();
                else
                    UIManager.Instance.DisplayNotEnoughLeafPointsText(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<DetectionZone>())
                other.gameObject.GetComponentInParent<EnemyController>().OnHitPlayer -= ReactToHit;
        }

        private void ReactToHit()
        {
            HealthPoints--;
            if (_healthPoints > 0)
                _animator.SetTrigger(HURT);
            _hurtSound.Play();

            if (HealthPoints <= 0)
            {
                _animator.SetTrigger(DEATH);
                _deathSound.Play();
                _numberOfDeaths++;
                OnPlayerDeath?.Invoke();
                enabled = false;
            }
        }
        
        public void Reset()
        {
            _animator.ResetTrigger(DEATH);
            _animator.ResetTrigger(HURT);
            transform.position = _startPosition.position;
            if (transform.localScale.x < 0)
                FlipSprite();
            HealthPoints = 3;
            NumberOfCollectedLeaves = 0;
            _respawnSound.Play();
            enabled = true;
            _animator.SetTrigger(RESPAWN);
        }

        // called at the end of the "Turn" animation
        public void FlipSprite()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public PlayerInfos GetInfos()
        {
            return new PlayerInfos()
            {
                HealthPoints = HealthPoints,
                LeafPoints = NumberOfCollectedLeaves * 5,
                NumberOfDeaths = _numberOfDeaths
            };
        }
        
        public void SetInfos(int healthPoints, int numberOfCollectedLeaves)
        {
            HealthPoints = healthPoints;
            NumberOfCollectedLeaves = numberOfCollectedLeaves;
        }

        public struct PlayerInfos
        {
            public int HealthPoints;
            public int LeafPoints;
            public int NumberOfDeaths;
        }
    }
}