using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : CoreMonoBehaviour
{
    [SerializeField] protected int index;
    [SerializeField] protected List<Transform> menu1List;
    [SerializeField] protected Transform pointer;
    [SerializeField] protected List<Transform> pointerPositions;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMenu();
        this.LoadPointer();
        this.LoadPointerPosition();
    }
    
    protected virtual void LoadMenu()
    {
        if (this.menu1List.Count > 0) return;
        Transform menu1 = transform.Find("Texts");
        foreach (Transform child in menu1)
        {
            this.menu1List.Add(child);
        }
        this.index = 0;
    }

    protected virtual void LoadPointer()
    {
        if (this.pointer != null) return;
        this.pointer = transform.Find("Pointer");
        Debug.LogWarning(transform.name + ": Load Pointer", gameObject);
    }

    protected virtual void LoadPointerPosition()
    {
        if (this.pointerPositions.Count > 0) return;
        Transform pointerPositions = transform.Find("PointerPositions");
        foreach (Transform transform in pointerPositions)
        {
            this.pointerPositions.Add(transform);
        }
        Debug.LogWarning(transform.name + ": Load Pointer Positions", gameObject);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.MoveToNextMenu();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.MoveToPreviousMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return)) //Press Button Enter
        {
            this.ChooseMenu();
        }
    }

    protected virtual void MoveToNextMenu()
    {
        this.index++;
        if (this.index >= this.menu1List.Count) this.index = 0;
        this.pointer.position = this.pointerPositions[this.index].position;
    }

    protected virtual void MoveToPreviousMenu()
    {
        this.index--;
        if (this.index < 0) this.index = this.menu1List.Count - 1;
        this.pointer.position = this.pointerPositions[this.index].position;
    }

    protected virtual void ChooseMenu()
    {

        if (this.index == 0)
        {
            ApplicationVariable.LoadingSceneName = "Level1";
            SceneManager.LoadScene("LoadingScene");
        }
        if (this.index == 2)
        {
            Application.Quit();
        }
    }
}
