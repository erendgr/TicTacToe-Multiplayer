using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;
    
    public event EventHandler<OnClickedOnGridPositionEventArgs> OnClickedOnGridPosition;

    public class OnClickedOnGridPositionEventArgs : EventArgs
    {
        public int x; 
        public int y; 
        public PlayerType playerType; 
        
    }

    public event EventHandler OnGameStarted;
    public event EventHandler OnCurrentPlayablePlayerTypeChanged;
    
    public enum PlayerType
    {
        None,
        Cross,
        Circle
    }

    private PlayerType _localPlayerType;
    private NetworkVariable<PlayerType> _currentPlayablePlayerType = new NetworkVariable<PlayerType>();
    
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
            //Debug.Log(NetworkManager.Singleton.LocalClientId);
        });
        _startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //Debug.Log(NetworkManager.Singleton.LocalClientId);
        });
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn: " + NetworkManager.Singleton.LocalClientId);
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            _localPlayerType = PlayerType.Cross;
        }
        else
        {
            _localPlayerType = PlayerType.Circle;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallBack;
        }

        _currentPlayablePlayerType.OnValueChanged += (PlayerType oldPlayerType, PlayerType newPlayerType) =>
        {
            OnCurrentPlayablePlayerTypeChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private void NetworkManager_OnClientConnectedCallBack(ulong obj)
    {
        if (Unity.Netcode.NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            //game started
            _currentPlayablePlayerType.Value = PlayerType.Cross;
            TriggerOnGameStartedRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerOnGameStartedRpc()
    {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }
    
    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x, int y, PlayerType playerType)
    {
        Debug.Log("ClickedOnGridPosition: " + x + ", " + y);

        if (playerType != _currentPlayablePlayerType.Value)
        {
            return;
        }
        
        OnClickedOnGridPosition?.Invoke(this, new OnClickedOnGridPositionEventArgs
        {
            x = x, 
            y = y,
            playerType = playerType,
        });

        switch (_currentPlayablePlayerType.Value)
        {
            default:
            case PlayerType.Cross:
                _currentPlayablePlayerType.Value = PlayerType.Circle;
                break;
            case PlayerType.Circle:
                _currentPlayablePlayerType.Value = PlayerType.Cross;
                break;
        }
    }

    
    
    public PlayerType GetLocalPlayerType()
    {
        return _localPlayerType;
    }

    public PlayerType GetCurrentPlayablePlayerType()
    {
        return _currentPlayablePlayerType.Value;
    }
}
