using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundButton:MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Sprite[] _soundEnableAndDisableSprites;
  [SerializeField]  private Image _button;

    private void Awake()
    {
        AudioManagerOnSoundStateChanged(AudioManager.IsSoundEnable);
    }

    private void OnEnable()
    {
        AudioManager.SoundStateChanged += AudioManagerOnSoundStateChanged;
    }

    private void OnDisable()
    {
        AudioManager.SoundStateChanged -= AudioManagerOnSoundStateChanged;
    }

    private void AudioManagerOnSoundStateChanged(bool b)
    {
        _button.sprite = _soundEnableAndDisableSprites[b ? 0 : 1];
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.IsSoundEnable = !AudioManager.IsSoundEnable;
    }
}