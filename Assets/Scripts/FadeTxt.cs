using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeTxt : MonoBehaviour
{
    private Text warningTxt;

    void Start()
    {
        warningTxt = GetComponent<Text>();
        transform.gameObject.SetActive(false);
    }

    public void StartFade(int r, int g, int b, float time)
    {
        transform.gameObject.SetActive(true);
        StartCoroutine(FadeOut(r, g, b, time));
    }

    IEnumerator FadeOut(int r, int g, int b, float time)
    {
        yield return new WaitForSeconds(time);
        float fadeCount = 1.0f;
        transform.gameObject.SetActive(true);
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            warningTxt.color = new Color(r, g, b, fadeCount);
        }
        transform.gameObject.SetActive(false);
        warningTxt.color = new Color(r, g, b, 1);
    }
}
