using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject placeSfxPrefab;
    [SerializeField] private GameObject winSfxPrefab;
    [SerializeField] private GameObject loseSfxPrefab;
    [SerializeField] private AudioSource backgroundMusic;
    
    private void Start()
    {
        GameManager.Instance.OnPlacedObject += GameManager_OnPlacedObject;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        GameManager.Instance.OnRematch += GameManager_OnRematch;
    }

    private void GameManager_OnRematch(object sender, EventArgs e)
    {
        backgroundMusic.Play();
    }

    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinEventArgs e)
    {
        backgroundMusic.Stop();
        
        if (GameManager.Instance.GetLocalPlayerType() == e.winPlayerType)
        {
            GameObject winSfxInstance = Instantiate(winSfxPrefab);
            Destroy(winSfxInstance, 5f);
        }
        else
        {
            GameObject loseSfxInstance = Instantiate(loseSfxPrefab);
            Destroy(loseSfxInstance, 5f);
        }
    }

    private void GameManager_OnPlacedObject(object sender, EventArgs e)
    {
        GameObject sfxInstance = Instantiate(placeSfxPrefab);
        Destroy(sfxInstance, 5f);
    }
}
