using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module05.UI
{
    public class DiaryScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _numberOfLeafPointsText;
        [SerializeField] private TextMeshProUGUI _numberOfDeathsText;
        [SerializeField] private Button[] _levelsButtons;

        private void Awake()
        {
            UnlockOnlyFirstLevel();
        }

        public void UpdateInfos(int numberOfLeafPoints, int numberOfDeaths)
        {
            _numberOfLeafPointsText.text = numberOfLeafPoints.ToString();
            _numberOfDeathsText.text = numberOfDeaths.ToString();
        }
        
        public void UnlockLevel(int levelIndex)
        {
            _levelsButtons[levelIndex - 1].interactable = true;
        }
        
        private void UnlockOnlyFirstLevel()
        {
            _levelsButtons[0].interactable = true;
        }
        
        public void GoToLevel(int levelIndex)
        {
            GameManager.Instance.GoToLevel(levelIndex);
        }
        
        public void BackToMainMenu()
        {
            GameManager.Instance.GoToMainMenu();
        }
    }
}