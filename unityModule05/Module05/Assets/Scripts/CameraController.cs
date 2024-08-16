using UnityEngine;

namespace Module05.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _background;
        
        private bool _isFollowingPlayer;
        private float _minXPosition = 0.29f;
        
        private void Start()
        {
            _isFollowingPlayer = false;
        }
        
        private void Update()
        {
            // wait for the player to reach the center of the screen before following
            _isFollowingPlayer = _player.position.x > _minXPosition;
            
            if (!_isFollowingPlayer)
                return;

            if (transform.position.x < _minXPosition)
                return;
            
            transform.position = new Vector3(_player.position.x, transform.position.y, transform.position.z);
            _background.position = new Vector3(_player.position.x, _background.position.y, _background.position.z);
        }

        public void Reset()
        {
            _isFollowingPlayer = false;
            transform.position = new Vector3(_minXPosition, transform.position.y, transform.position.z);
            _background.position = new Vector3(_minXPosition, _background.position.y, _background.position.z);
        }
    }
}