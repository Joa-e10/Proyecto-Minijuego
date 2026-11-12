using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PanelEndUI : MonoBehaviour
{
    [SerializeField] private CanvasManager _canvasManager;

    [Header("Texts UI")]
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private TextMeshProUGUI _quantityClients;
    [SerializeField] private TextMeshProUGUI _requiredClients;
    [Header("Buttons UI")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;

    private Player _playerObject;
    private int _quantityPlayer = 0;
    private int _pointsOwner = 0;
    private int _pointsClient = 0;

    public static event Action OnRetryGame;

    void OnEnable()
    {
        _retryButton.onClick.AddListener(RetryGame);
        _mainMenuButton.onClick.AddListener(ReturnMainMenu);

        PanelHudUI.OnEndGame += WinnerTextUpdate;
    }

    void OnDisable()
    {
        PanelHudUI.OnEndGame += WinnerTextUpdate;
    }

    public void RetryGame() 
    {

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            _requiredClients.text = NetworkManager.Singleton.ConnectedClientsList.Count.ToString();
            if (client.PlayerObject.IsOwner)
            {
                _playerObject = client.PlayerObject.GetComponent<Player>();
                _playerObject.SetPlayAgain();

                if (_playerObject.GetPlayAgain() == true)
                {
                    _quantityPlayer++;
                    _quantityClients.text = _quantityPlayer.ToString();
                }
            }
            else
            {
                _playerObject = client.PlayerObject.GetComponent<Player>();
                _playerObject.SetPlayAgain();

                if (_playerObject.GetPlayAgain() == true)
                {
                    _quantityPlayer++;
                    _quantityClients.text = _quantityPlayer.ToString();
                }
            }
        }

        if (_quantityPlayer >= 2)
        {
            _canvasManager.RetryGame();
            OnRetryGame?.Invoke();
        }

        _retryButton.onClick.RemoveListener(RetryGame);
    }

    private void WinnerTextUpdate() 
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject.IsOwner)
            {
                _playerObject = client.PlayerObject.GetComponent<Player>();
                _pointsOwner = _playerObject.GetPointPlayer();
            }
            else 
            {
                _playerObject = client.PlayerObject.GetComponent<Player>();
                _pointsClient = _playerObject.GetPointPlayer();
            }
        }

        if (_pointsClient <= _pointsOwner)
        {
            _winnerText.text = "You Win";
        }
        else 
        {
            _winnerText.text = "You Lose";
        }

    }

    public void ReturnMainMenu() 
    {
    _canvasManager.ReloadScene();
    }
}
