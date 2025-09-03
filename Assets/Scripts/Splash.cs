using System.Collections;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();
        GameManager.LoadScene("MainMenu");
    }
}