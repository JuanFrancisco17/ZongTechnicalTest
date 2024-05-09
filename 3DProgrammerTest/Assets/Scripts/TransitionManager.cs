using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] GameObject fadeIn, fadeOut;

    private void Start()
    {
        StartFadeIn();
    }

    public void Win()
    {
        StartCoroutine(WinCoroutine());
    }

    IEnumerator WinCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        fadeIn.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(0);
    }

    private void StartFadeIn()
    {
        StartCoroutine(StartFadeInCoroutine());
    }

    IEnumerator StartFadeInCoroutine()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        fadeOut.SetActive(false);
    }
}
