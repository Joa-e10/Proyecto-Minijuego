using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("GameObject UI")]
    [SerializeField] private GameObject _hostButtonObject;
    [SerializeField] private GameObject _clientButtonObject;
    [SerializeField] private GameObject _startButtonObject;
    [SerializeField] private GameObject _ipInputFieldObject;
    [SerializeField] private GameObject _waitingTextObject;

    [Header("Button UI")]
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _startButton;

    [SerializeField] private TMP_InputField _ipInputField;
    void Start()
    {
        
    }

    void OnEnable()
    {
        ConnectionHandler.OnPlayersWaiting += ShowWaitingText;

        _hostButton.onClick.AddListener(StartPlayerHost);
        _clientButton.onClick.AddListener(ShowIpInput);
        _startButton.onClick.AddListener(StartGame);
    }

    void OnDisable()
    {
        ConnectionHandler.OnPlayersWaiting -= ShowWaitingText;

        _hostButton.onClick.RemoveListener(StartPlayerHost);
        _clientButton.onClick.RemoveListener(ShowIpInput);
        _startButton.onClick.RemoveListener(StartGame);
    }

    public void StartPlayerHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ShowWaitingText() 
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count <= 1)
        {
            _waitingTextObject.SetActive(true);
            _hostButtonObject.SetActive(false);

            if (_clientButtonObject.activeInHierarchy)
            {
                _clientButtonObject.SetActive(false);
            }
            else 
            {
                _ipInputFieldObject.SetActive(false);
                _startButtonObject.SetActive(false);
            }
        }
    }

    public void ShowIpInput()
    {
        _clientButtonObject.SetActive(false);

        if (_clientButtonObject.activeInHierarchy) return;

        _startButtonObject.SetActive(true);
        _ipInputFieldObject.SetActive(true);
    }

    public void StartGame()
    {
        if (_ipInputField != null)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_ipInputField.text, (ushort)7777);
            NetworkManager.Singleton.StartClient();
        }
    }
    void Update()
    {
        
    }
}
