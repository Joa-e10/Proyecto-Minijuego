using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Panels UI")]
    [SerializeField] private GameObject _panelHud;
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelCrafting;

    [SerializeField] private PanelHudUI _panelHudUI;

    void OnEnable()
    {
        ConnectionHandler.OnStartGame += StartGamePanels;
    }

    void OnDisable()
    {
        ConnectionHandler.OnStartGame -= StartGamePanels;
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

    void Update()
    {
        
    }
}
