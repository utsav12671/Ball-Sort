using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public LevelCompletePanel _levelCompletePanel;
        public GameObject TutorialPanel;
        public GameObject _winEffect;


        public static bool IsFirstTime
        {
            get => PrefManager.GetBool(nameof(IsFirstTime), true);
            set => PrefManager.SetBool(nameof(IsFirstTime), value);
        }

        private void Awake()
        {
            Instance = this;
                TutorialPanel.SetActive(IsFirstTime);
            if (IsFirstTime)
            {
                IsFirstTime = false;
            }
        }

        private void OnEnable()
        {
            LevelManager.LevelCompleted += LevelManagerOnLevelCompleted;
        }


        private void OnDisable()
        {
            LevelManager.LevelCompleted -= LevelManagerOnLevelCompleted;
        }

        private void LevelManagerOnLevelCompleted()
        {
            StartCoroutine(LevelCompletedEnumerator());
        }

        private IEnumerator LevelCompletedEnumerator()
        {
            yield return new WaitForSeconds(0.2f);
            _levelCompletePanel.gameObject.SetActive(true) ;
            var point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f)).WithZ(0);
            Instantiate(_winEffect, point, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }


        public void LoadNextLevel()
        {
            var gameMode = LevelManager.Instance.GameMode;
            var levelNo = LevelManager.Instance.Level.no;
            if (!ResourceManager.HasLevel(gameMode, levelNo + 1))
            {

                GameManager.LoadScene("MainMenu");

            }

            GameManager.LoadGame(new LoadGameData
            {
                Level = ResourceManager.GetLevel(gameMode, levelNo + 1),
                GameMode = gameMode
            });
        }
    }
}