using UnityEngine;

namespace Module05.UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        public void ResumeGame()
        {
            GameManager.Instance.ResumeGame();
        }
        
        public void StartNewGame()
        {
            GameManager.Instance.StartNewGame();
        }
        
        public void OpenDiary()
        {
            GameManager.Instance.OpenDiary();
        }
    }
}