using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    private const float GRID_SIZE = 3f;
    
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject lineCompletePrefab;

    private void Start()
    {
        GameManager.Instance.OnClickedOnGridPosition += GameManager_OnClickedOnGridPosition;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
    }
    
    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinEventArgs e)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        float eulerZ = 0f;
        switch (e.line.orientation)
        {
            default:
            case GameManager.Orientation.Horizontal: eulerZ = 0f; break; 
            case GameManager.Orientation.Vertical: eulerZ = 90f; break;
            case GameManager.Orientation.DiagonalA: eulerZ = 45f; break;
            case GameManager.Orientation.DiagonalB: eulerZ = -45f; break;
        }
        // GameObject lineCompleteInstance = Instantiate(lineCompletePrefab, 
        //     GetGridWorldPosition(e.centerGridPosition.x, e.centerGridPosition.y), Quaternion.identity);
        // lineCompleteInstance.GetComponent<NetworkObject>().Spawn(true);
        InstantiateLineComplete(e.line.centerGridPosition.x, e.line.centerGridPosition.y, eulerZ);
    }
    
    private void InstantiateLineComplete(int x, int y, float eulerZ)
    {
        GameObject lineCompleteInstance = Instantiate(lineCompletePrefab, 
            GetGridWorldPosition(x, y), Quaternion.Euler(0f, 0f, eulerZ));
        lineCompleteInstance.GetComponent<NetworkObject>().Spawn(true);
    }
    private void GameManager_OnClickedOnGridPosition(object sender, GameManager.OnClickedOnGridPositionEventArgs e)
    {
        Debug.Log("gamemanager: OnClickedOnGridPosition");
        SpawnObjectRpc(e.x, e.y, e.playerType);
        Debug.Log("player type: " + e.playerType);
    }

    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(int x, int y, GameManager.PlayerType playerType)
    {
        GameObject prefab;
        switch (playerType)
        {
            default:
            case GameManager.PlayerType.Cross:
                prefab = crossPrefab;
                break;
            case GameManager.PlayerType.Circle:
                prefab = circlePrefab;
                break;
        }
        Debug.Log("spawn object");
        GameObject spawnedCrossInstance = Instantiate(prefab, GetGridWorldPosition(x, y), Quaternion.identity);
        spawnedCrossInstance.GetComponent<NetworkObject>().Spawn(true);
    }
    
    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
    }
}
