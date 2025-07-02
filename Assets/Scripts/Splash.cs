// /*
// Created by Darsan
// */

using System;
using System.Collections;
using MyGame;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private IEnumerator Start()
    {
        {
            SharedUIManager.ConsentPanel.Show();
            yield return new WaitUntil(() => !SharedUIManager.ConsentPanel.Showing);
        }

        GameManager.LoadScene("MainMenu");
    }
}