using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module05.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private TextMeshProUGUI _healthPointsText;
        [SerializeField] private TextMeshProUGUI _leafPointsText;
        [SerializeField] private TextMeshProUGUI _notEnoughLeafPointsText;
        
        private static UIManager _instance;
        public static UIManager Instance => _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            
            SceneManager.sceneLoaded += CheckIfDisplay;
            _notEnoughLeafPointsText.gameObject.SetActive(false);
        }
        
        private void CheckIfDisplay(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu" || scene.name == "Diary")
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
        
        public void SetStageNumber(int stageNumber)
        {
            _levelNumberText.text = "Stage " + stageNumber;
        }
        
        public void UpdatePlayerHealthPoints(int healthPoints)
        {
            _healthPointsText.text = healthPoints + " HP";
        }
        
        public void UpdatePlayerLeafPoints(int leafPoints)
        {
            _leafPointsText.text = leafPoints + " LP";
        }
        
        public void DisplayNotEnoughLeafPointsText()
        {
            StartCoroutine(DisplayNotEnoughLeafPoints());
        }
        
        private IEnumerator DisplayNotEnoughLeafPoints()
        {
            _notEnoughLeafPointsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            _notEnoughLeafPointsText.gameObject.SetActive(false);
        }
        
        public void RestartLevel()
        {
            LevelManager.LevelManager levelManager = FindObjectOfType<LevelManager.LevelManager>();
            levelManager.RestartLevel();
        }
        
        public void GoToMainMenu()
        {
            LevelManager.LevelManager levelManager = FindObjectOfType<LevelManager.LevelManager>();
            levelManager.GoToMainMenu();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= CheckIfDisplay;
        }
    }
}