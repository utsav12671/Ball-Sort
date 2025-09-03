using UnityEngine;

namespace MainMenu
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager i;
        [SerializeField] GameObject _HomePanel;
        [SerializeField] GameObject _GameModePanel;
        [SerializeField] LevelsPanel _LevelPanel;

        private void Awake()
        {
            i = this;
        }

        public void Play_ButtonClick()
        {
            _GameModePanel.SetActive(true);
            _LevelPanel.gameObject.SetActive(false);
            _HomePanel.SetActive(false);
        }

        public void ModeSelection_ButtonClick(int i)
        {
            var levelsPanel = _LevelPanel;
            levelsPanel.GameMode = (GameMode)i;
            _LevelPanel.gameObject.SetActive(true);
            _GameModePanel.SetActive(false);
            _HomePanel.SetActive(false);
        }



    }
}
