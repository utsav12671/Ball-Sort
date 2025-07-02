using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsentPanel : ShowHidable
{

    [SerializeField] private Button _privatePolicyBtn;


    private void Awake()
    {
        _privatePolicyBtn.gameObject.SetActive(GameSettings.Default.PrivatePolicySetting.enable);
    }


    public void OnClickYes()
    {
      
        Hide();
    }

    public void OnClickPrivacy()
    {
        Application.OpenURL("https://gameacts.wordpress.com/privatepolicy/?frame-nonce=1fdfdb8229");
    }

    public void OnClickNo()
    {
       
        Hide();
    }
}
