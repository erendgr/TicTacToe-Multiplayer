using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;
    
    public event EventHandler<OnClickedOnGridPositionEventArgs> OnClickedOnGridPosition;
    public class OnClickedOnGridPositionEventArgs : EventArgs { public int x; public int y; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            Debug.Log("Game Manager has been destroyed!");
        }
        else
        {
            Instance = this;
            Debug.Log("Game Manager!");
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        _startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    public void ClickedOnGridPosition(int x, int y)
    {
        Debug.Log("ClickedOnGridPosition: " + x + ", " + y);
        OnClickedOnGridPosition?.Invoke(this, new OnClickedOnGridPositionEventArgs { x = x, y = y });
    }
}
