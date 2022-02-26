using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneBar : MonoBehaviour
{
    private Image progressImg = null;

    private void Start()
    {
        progressImg = GetComponent<Image>();
        progressImg.fillAmount = 0;
        StartCoroutine(nameof(NextLoadingScene));
        GameManager.Clear();
    }

    IEnumerator NextLoadingScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(PotalSceneLoad.StaticMoveSceneName);
        op.allowSceneActivation = false;

        while(op.progress < 0.9f)
        {
            yield return null;
            progressImg.fillAmount = Mathf.Lerp(progressImg.fillAmount,1, 1f * Time.deltaTime);
        }

        while (progressImg.fillAmount <= 0.99f)
        {
            yield return null;
            progressImg.fillAmount = Mathf.Lerp(progressImg.fillAmount, 1, 1f * Time.deltaTime);
        }

        SceneManager.LoadScene(PotalSceneLoad.StaticMoveSceneName);
    }
}
