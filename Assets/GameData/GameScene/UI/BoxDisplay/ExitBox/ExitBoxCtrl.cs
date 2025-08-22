using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBoxCtrl : CoreMonoBehaviour
{
    [SerializeField] protected Transform pointer;
    [SerializeField] protected List<Transform> pointerPositions;
    protected int index = 0;
    [SerializeField] protected ExitBoxDisplay exitBoxDisplay;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadExitBoxDisplay();
        this.LoadPointer();
        this.LoadPointerPositions();
    }

    protected virtual void LoadExitBoxDisplay()
    {
        if (this.exitBoxDisplay != null) return;
        this.exitBoxDisplay = transform.parent.GetComponent<ExitBoxDisplay>();
        Debug.LogWarning(transform.name + ": Load Exit Box Display", gameObject);
    }

    protected virtual void LoadPointer()
    {
        if (this.pointer != null) return;
        this.pointer = transform.Find("Pointer");
        Debug.LogWarning(transform.name + ": Load Pointer", gameObject);
    }

    protected virtual void LoadPointerPositions()
    {
        if (this.pointerPositions.Count > 0) return;
        Transform positions = transform.Find("PointerPositions");
        foreach (Transform position in positions)
        {
            this.pointerPositions.Add(position);
        }
        Debug.LogWarning(transform.name + ": Load Pointer Positions", gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.SetPointerPosition();
    }

    protected virtual void SetPointerPosition()
    {
        this.index = 0; // Reset index to the first position
        this.pointer.position = this.pointerPositions[0].position;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.index--;
            if (this.index < 0) this.index = this.pointerPositions.Count - 1;
            this.pointer.position = this.pointerPositions[this.index].position;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.index++;
            if (this.index >= this.pointerPositions.Count) this.index = 0;
            this.pointer.position = this.pointerPositions[this.index].position;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (this.index == 0)
            {
                // Return to Menu Scene
                StartCoroutine(this.ReturnToMenuScene());
            }
            if (this.index == 1)
            {
                // Escape box
                this.exitBoxDisplay.EscapeBox();
            }
        }
    }

    protected IEnumerator ReturnToMenuScene()
    {
        GameManagerScript.isGamePaused = false;
        yield return new WaitForSeconds(0.1f); // Optional delay before returning to menu
        ApplicationVariable.LoadingSceneName = "MenuScene";
        SceneManager.LoadScene("LoadingScene");
        
    }
}
