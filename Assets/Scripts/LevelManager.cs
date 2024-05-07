using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Button _1stLevelButton;
    [SerializeField]
    private Button _2ndLevelButton;
    [SerializeField]
    private Button _3thLevelButton;
    [SerializeField]
    private Button _4thLevelButton;

    private void Awake()
    {
        _1stLevelButton.onClick.AddListener(() => LoadLevel(1));
        _2ndLevelButton.onClick.AddListener(() => LoadLevel(2));
        _3thLevelButton.onClick.AddListener(() => LoadLevel(3));
        _4thLevelButton.onClick.AddListener(() => LoadLevel(4));
    }
    public void LoadLevel(int level)
    {
        try
        {
            //SceneManager.LoadScene(number + "level");
            Debug.Log("Level " + level);
        }
        catch
        {
            Debug.LogError($"Scene Manager doesn't containd scene with name:level");
        }
    }
}
