using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public float loadTime = 5f; // 15 seconds to fill the slider

    void Start()
    {
        StartCoroutine(FillSlider());
    }

    IEnumerator FillSlider()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadTime)
        {
            elapsedTime += Time.deltaTime;
            loadingSlider.value = elapsedTime / loadTime;
            yield return null; // Wait for the next frame
        }

        // Load the next scene after filling the slider
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
