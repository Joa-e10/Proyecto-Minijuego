using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
public class CanvasManager : MonoBehaviour
{
    [Header("Panels UI")]
    [SerializeField] private GameObject _panelHud;
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelEnd;

    [SerializeField] private PanelHudUI _panelHudUI;

    [SerializeField] GameObject _timerPrefab;

    void OnEnable()
    {
        ConnectionHandler.OnStartGame += StartGamePanels;
        PanelHudUI.OnEndGame += StartEndGame;
    }

    void OnDisable()
    {
        ConnectionHandler.OnStartGame -= StartGamePanels;
        PanelHudUI.OnEndGame -= StartEndGame;
    }

    public PanelHudUI GetPanelHud() 
    {
        return _panelHudUI;
    }
    public void StartGamePanels() 
    {
        if (_panelMainMenu.activeInHierarchy)
        {
            _panelMainMenu.SetActive(false);
            _panelHud.SetActive(true);
        }
    }

    public void StartEndGame() 
    {
            _panelHud.SetActive(false);
            _panelEnd.SetActive(true);
    }

    public void StartRetryGame() 
    {
        if (_panelEnd.activeInHierarchy)
        {
            _panelHud.SetActive(true);
            _panelEnd.SetActive(false);
        }
    }

    public void RetryGame() 
    {
        StartRetryGame();

            GameObject _timerSpawned = Instantiate(_timerPrefab);
            _timerSpawned.GetComponent<NetworkObject>().Spawn();

    }

    public void ReloadScene()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Cargamos una nueva scene");
    }

    void Update()
    {
        
    }
}
