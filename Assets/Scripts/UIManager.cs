using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform _playerHead;

    [SerializeField] private GameObject _UICanvas;

    [SerializeField] private GameObject _nextLevelPanel;
    [SerializeField] private Button _nextLevelPanelButton;

    [SerializeField] private float _messageOffset = 2f;
    [SerializeField] private TMP_Text _messageText;

    private SceneFader _sceneFader;

    private void Awake()
    {
        _sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandler;
    }

    private void Update()
    {
        _UICanvas.transform.position = 
            _playerHead.position 
            + new Vector3(_playerHead.forward.x, 0, _playerHead.forward.z).normalized 
            * _messageOffset;

        _UICanvas.transform.LookAt(new Vector3(_playerHead.position.x, _UICanvas.transform.position.y, _playerHead.position.z));
        _UICanvas.transform.forward *= -1;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandler;
    }

    public void ShowMessage(string message)
    {
        _messageText.text = message;
    }

    public void HideMessage() => _messageText.text = "";

    private void OnGameStateChangedEventHandler(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                HideMessage();
                GetAndDisablePanel();
                break;
            case GameState.Win:
                _nextLevelPanel.SetActive(true);
                break;
        }
    }

    private void GetAndDisablePanel()
    {
        _nextLevelPanel = GameObject.Find("NextLevelPanel");
        _nextLevelPanelButton = GameObject.Find("NextLevelButton").GetComponent<Button>();
        _nextLevelPanelButton.onClick.AddListener(_sceneFader.FadeIn);
        _nextLevelPanel.SetActive(false);
    }
}
