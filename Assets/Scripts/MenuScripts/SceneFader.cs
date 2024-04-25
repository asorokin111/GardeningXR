using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    private FadeScreen fadeScreen;

    private string _sceneName;

    private void Awake()
    {
        fadeScreen = GameObject.Find("FadeScreen").GetComponent<FadeScreen>();
    }

    public void FadeIn()
    {
        StartCoroutine(TransferToScene(SceneManager.GetActiveScene().buildIndex + 1));
        fadeScreen.FadeOut();
    }

    public void FadeIn(string sceneName)
    {
        _sceneName = sceneName;
        StartCoroutine(TransferToScene());
        fadeScreen.FadeOut();
    }

    //Invokes righ after animation is played
    public IEnumerator TransferToScene()
    {
        yield return new WaitForSecondsRealtime(fadeScreen.Duration);

        SceneManager.LoadScene(_sceneName);
    }
    
    public IEnumerator TransferToScene(int index)
    {
        yield return new WaitForSecondsRealtime(fadeScreen.Duration);

        SceneManager.LoadScene(index);
    }
}
