using System;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    
    private void OnMouseDown()
    {
        GameManager.Instance.ClickedOnGridPositionRpc(gridX, gridY, GameManager.Instance.GetLocalPlayerType());
    }
}
