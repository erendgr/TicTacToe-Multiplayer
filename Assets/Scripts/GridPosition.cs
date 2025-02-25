using System;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    
    private void OnMouseDown()
    {
        GameManager.Instance.ClickedOnGridPosition(gridX, gridY );
    }
}
