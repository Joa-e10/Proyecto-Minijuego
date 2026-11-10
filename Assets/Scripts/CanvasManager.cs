using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Panels UI")]
    [SerializeField] private GameObject _panelHud;
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelEnd;

    [SerializeField] private PanelHudUI _panelHudUI;

    void OnEnable()
    {
        ConnectionHandler.OnStartGame += StartGamePanels;
        TimerManager.OnEndGame += StartEndGame;
    }

    void OnDisable()
    {
        ConnectionHandler.OnStartGame -= StartGamePanels;
        TimerManager.OnEndGame -= StartEndGame;
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
        if (_panelHud.activeInHierarchy) 
        {
            _panelHud.SetActive(false);
            _panelEnd.SetActive(true);
        }
    }

    void Update()
    {
        
    }
}
