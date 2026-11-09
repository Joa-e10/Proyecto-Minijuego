using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Panels UI")]
    [SerializeField] private GameObject _panelHud;
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelCrafting;

    void OnEnable()
    {
        ConnectionHandler.OnStartGame += StartGamePanels;
    }

    void OnDisable()
    {
        ConnectionHandler.OnStartGame -= StartGamePanels;
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
