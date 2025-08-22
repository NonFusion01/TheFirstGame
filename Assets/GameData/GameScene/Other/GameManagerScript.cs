using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : CoreMonoBehaviour
{
    [SerializeField] public static bool isGamePaused = false;
    protected virtual void Update()
    {
        this.CheckGameState();
    }

    protected virtual void CheckGameState()
    {
        if (isGamePaused) Time.timeScale = 0f;
        if (!isGamePaused) Time.timeScale = 1f;
    }
}
