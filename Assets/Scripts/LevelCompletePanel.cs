using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class LevelCompletePanel : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _toastTxt;
        [SerializeField] private List<string> _toasts = new List<string>();

        private void OnEnable()
        {
            _toastTxt.text = _toasts.GetRandom();
            _toastTxt.gameObject.SetActive(true);
            
        }


        public void OnClickContinue()
        {
            UIManager.Instance.LoadNextLevel();
        }
    }
}