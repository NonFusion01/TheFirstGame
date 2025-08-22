using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponent();
    }

    protected virtual void Start()
    {
        //For override
    }

    protected virtual void Reset()
    {
        this.LoadComponent();
        this.ResetValue();
    }

    protected virtual void OnEnable()
    {
        //For override
    }

    protected virtual void OnDisable()
    {
        //For override
    }

    protected virtual void LoadComponent()
    {
        //For override
    }

    protected virtual void ResetValue()
    {
        //For override
    }
}
