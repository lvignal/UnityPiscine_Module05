using System;
using UnityEngine;

namespace Module05.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private DetectionZone _detectionZone;
        
        private Animator _animator;
        private AudioSource _attackSound;

        public Action OnHitPlayer;
        
        private const string IS_PLAYER_DETECTED = "isPlayerDetected";
        
        protected void Start()
        {
            _animator = GetComponent<Animator>();
            _attackSound = GetComponent<AudioSource>();
            _detectionZone.OnPlayerDetected += AttackPlayer;
        }
        
        private void AttackPlayer(bool isDetected)
        {
            _animator.SetBool(IS_PLAYER_DETECTED, isDetected);
        }
        
        // called at a precise moment in the attack animation
        public void HitPlayer()
        {
            _attackSound.Play();
            OnHitPlayer?.Invoke();
        }
        
        private void OnDestroy()
        {
            _detectionZone.OnPlayerDetected -= AttackPlayer;
        }
    }
}