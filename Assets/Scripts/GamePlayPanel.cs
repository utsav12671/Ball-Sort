
using System;
using UnityEngine;
using UnityEngine.UI;
using Gameplay;

public class GamePlayPanel : MonoBehaviour
{
    [SerializeField] private Button _undoBtn;
    [SerializeField] private TMPro.TextMeshProUGUI _lvlTxt;

    private void Start()
    {
        _lvlTxt.text = $"Level {LevelManager.Instance.Level.no}";
    }

    public void OnClickUndo()
    {
        LevelManager.Instance.OnClickUndo();
    }

    public void OnClickRestart()
    {
        GameManager.LoadGame(new LoadGameData
        {
            Level = LevelManager.Instance.Level,
            GameMode = LevelManager.Instance.GameMode,
        },false);
    }

    public void OnClickSkip()
    {
   

                ResourceManager.CompleteLevel(LevelManager.Instance.GameMode, LevelManager.Instance.Level.no);
                UIManager.Instance.LoadNextLevel();
          
    }

    public void OnClickMenu()
    {
       

            GameManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        _undoBtn.interactable = LevelManager.Instance.HaveUndo;
    }
}