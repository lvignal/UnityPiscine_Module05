using System.Collections;
using UnityEngine;

namespace Module05.Enemy
{
    public class CactusController : EnemyController
    {
        [SerializeField] private GameObject _jelly;
        
        private void Start()
        {
            base.Start();
            OnHitPlayer += SpawnJelly;
            _jelly.SetActive(false);
        }
        
        private void SpawnJelly()
        {
            _jelly.SetActive(true); // play animation
            StartCoroutine(WaitForAnimationEnd());
        }
        
        private IEnumerator WaitForAnimationEnd()
        {
            float animationLength = _jelly.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationLength);
            _jelly.SetActive(false);
        }
        
        private void OnDestroy()
        {
            OnHitPlayer -= SpawnJelly;
        }
    }
}