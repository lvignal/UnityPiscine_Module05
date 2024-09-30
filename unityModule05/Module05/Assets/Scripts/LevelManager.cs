using Module05.Camera;
using Module05.Collectible;
using Module05.Player;
using Module05.UI;
using UnityEngine;

namespace Module05.LevelManager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private int _levelId;
        [SerializeField] private PlayerController _player;
        [SerializeField] private FadingScreen _fadingScreen;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private CollectibleManager _collectibleManager;
        
        private bool _isGameSaved;
        
        private void Start()
        {
            _player.OnPlayerDeath += RestartLevel;
            _player.OnEndReached += GoToNextLevel;
            _isGameSaved = false;
            UIManager.Instance.SetStageNumber(_levelId);
        }
        
        public void RestartLevel()
        {
            // remove the leaf points collected
            PlayerController.PlayerInfos playerInfos = GetPlayerInfos();
            int lastSavedLeafPoints = PlayerPrefs.GetInt("LeafPoints", 0);
            PlayerPrefs.SetInt("LeafPoints", lastSavedLeafPoints - playerInfos.LeafPoints);
            
            _fadingScreen.OnFadeComplete += ResetLevel;
            _fadingScreen.Fade(true);
        }
        
        private void ResetLevel()
        {
            _fadingScreen.OnFadeComplete -= ResetLevel;
            _player.Reset();
            _cameraController.Reset();
            _collectibleManager.Reset();
            _fadingScreen.Fade(false);
        }

        private void GoToNextLevel()
        {
            _player.OnEndReached -= GoToNextLevel;
            if (!_isGameSaved)
                SaveGame();
            GameManager.Instance.GoToNextLevel();
        }

        public PlayerController.PlayerInfos GetPlayerInfos()
        {
            return _player.GetInfos();
        }

        public void SetPlayerInfos(int healthPoints, int numberOfCollectedLeaves)
        {
            _player.SetInfos(healthPoints, numberOfCollectedLeaves);
        }
        
        public void GoToMainMenu()
        {
            SaveGame();
            GameManager.Instance.GoToMainMenu();
        }
        
        private void SaveGame()
        {
            PlayerController.PlayerInfos playerInfos = GetPlayerInfos();
            PlayerPrefs.SetInt("HealthPoints", playerInfos.HealthPoints);
            int lastSavedLeafPoints = PlayerPrefs.GetInt("LeafPoints", 0);
            PlayerPrefs.SetInt("LeafPoints", lastSavedLeafPoints + playerInfos.LeafPoints);
            int lastSavedNumberOfDeaths = PlayerPrefs.GetInt("NumberOfDeaths", 0);
            PlayerPrefs.SetInt("NumberOfDeaths", lastSavedNumberOfDeaths + playerInfos.NumberOfDeaths);
            
            if (PlayerPrefs.GetInt("LastLevelUnlocked") < _levelId)
                PlayerPrefs.SetInt("LastLevelUnlocked", _levelId);
            
            CollectibleManager collectibleManager = FindObjectOfType<CollectibleManager>();
            for(int i = 0; i < collectibleManager.NumberOfCollectibles; i++)
            {
                PlayerPrefs.SetInt("Collectible" + i, collectibleManager.IsCollectibleCollected(i) ? 1 : 0);
            }
            PlayerPrefs.Save();
            _isGameSaved = true;
        }
        
        private void OnDestroy()
        {
            _player.OnPlayerDeath -= RestartLevel;
            _fadingScreen.OnFadeComplete -= ResetLevel;
            _player.OnEndReached -= GoToNextLevel;
        }
    }
}