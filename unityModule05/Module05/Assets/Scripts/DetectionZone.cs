using System;
using Module05.Player;
using UnityEngine;

namespace Module05.Enemy
{
    public class DetectionZone : MonoBehaviour
    {
        public Action<bool> OnPlayerDetected;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerDetected?.Invoke(true);
                other.gameObject.GetComponent<PlayerController>().OnPlayerDeath += StopDetection;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerDetected?.Invoke(false);
                other.gameObject.GetComponent<PlayerController>().OnPlayerDeath -= StopDetection;
            }
        }
        
        private void StopDetection()
        {
            OnPlayerDetected?.Invoke(false);
        }
    }
}