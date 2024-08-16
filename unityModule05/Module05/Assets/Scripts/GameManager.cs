using Module05.Collectible;
using Module05.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module05
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;
        public int LastLevelUnlocked { get; private set; } = 1;
        private int _numberOfLevels = 3;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
        
        public void ResumeGame()
        {
            LastLevelUnlocked = PlayerPrefs.GetInt("LastLevelUnlocked", 1);
            Debug.Log("Resuming game at level " + LastLevelUnlocked);
            SceneManager.LoadScene("Stage" + LastLevelUnlocked);
            SceneManager.sceneLoaded += ReturnToSavedState;
        }

        private void ReturnToSavedState(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Stage" + LastLevelUnlocked)
                return;
            
            SceneManager.sceneLoaded -= ReturnToSavedState;

            CollectibleManager collectibleManager = FindObjectOfType<CollectibleManager>();
            int numberOfCollectedLeaves = 0;
            for (int i = 0; i < collectibleManager.NumberOfCollectibles; i++)
            {
                if (PlayerPrefs.GetInt("Collectible" + i, 0) == 1)
                {
                    collectibleManager.Collect(i);
                    numberOfCollectedLeaves++;
                }
            }
            
            LevelManager.LevelManager levelManager = FindObjectOfType<LevelManager.LevelManager>();
            levelManager.SetPlayerInfos(PlayerPrefs.GetInt("HealthPoints", 3), numberOfCollectedLeaves);
        }
        
        public void StartNewGame()
        {
            PlayerPrefs.DeleteAll();
            LastLevelUnlocked = 1;
            GoToLevel(LastLevelUnlocked);
        }

        private void InitializePlayerInfos(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= InitializePlayerInfos;
            LevelManager.LevelManager levelManager = FindObjectOfType<LevelManager.LevelManager>();
            levelManager.SetPlayerInfos(3, 0);
        }

        public void OpenDiary()
        {
            SceneManager.LoadScene("Diary");
            SceneManager.sceneLoaded += UpdateDiary;
        }

        private void UpdateDiary(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Diary")
                return;
            
            SceneManager.sceneLoaded -= UpdateDiary;
            DiaryScreen diary= FindObjectOfType<DiaryScreen>();
            diary.UpdateInfos(PlayerPrefs.GetInt("LeafPoints", 0), PlayerPrefs.GetInt("NumberOfDeaths", 0));
            
            for (int i = 1; i <= _numberOfLevels; i++)
            {
                if (i <= LastLevelUnlocked)
                    diary.UnlockLevel(i);
            }
        }
        
        public void GoToNextLevel()
        {
            LastLevelUnlocked++;
            if (LastLevelUnlocked <= _numberOfLevels)
                GoToLevel(LastLevelUnlocked);
            else
                GoToMainMenu();
        }
        
        public void GoToLevel(int level)
        {
            SceneManager.LoadScene("Stage" + level);
            SceneManager.sceneLoaded += InitializePlayerInfos;
        }
        
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}