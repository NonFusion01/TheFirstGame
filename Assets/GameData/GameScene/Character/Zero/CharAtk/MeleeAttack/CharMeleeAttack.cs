using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMeleeAttack : CoreMonoBehaviour
{
    [SerializeField] public Atk1Range atk1Range;
    [SerializeField] public Atk2Range atk2Range;
    [SerializeField] public Atk3Range atk3Range;
    [SerializeField] public AirAtk1Range airAtk1Range;
    [SerializeField] public AirAtk2Range airAtk2Range;
    [SerializeField] public AirAtk3Range airAtk3Range;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadAtk1Range();
        this.LoadAtk2Range();
        this.LoadAtk3Range();
        this.LoadAirAtk1Range();
        this.LoadAirAtk2Range();
        this.LoadAirAtk3Range();
    }

    protected virtual void LoadAtk1Range()
    {
        if (this.atk1Range != null) return;
        this.atk1Range = GetComponentInChildren<Atk1Range>();
        Debug.LogWarning(transform.name + ": Load Atk1Range", gameObject);
    }

    protected virtual void LoadAtk2Range()
    {
        if (this.atk2Range != null) return;
        this.atk2Range = GetComponentInChildren<Atk2Range>();
        Debug.LogWarning(transform.name + ": Load Atk2Range", gameObject);
    }

    protected virtual void LoadAtk3Range()
    {
        if (this.atk3Range != null) return;
        this.atk3Range = GetComponentInChildren<Atk3Range>();
        Debug.LogWarning(transform.name + ": Load Atk3Range", gameObject);
    }

    protected virtual void LoadAirAtk1Range()
    {
        if (this.airAtk1Range != null) return;
        this.airAtk1Range = GetComponentInChildren<AirAtk1Range>();
        Debug.LogWarning(transform.name + ": Load AirAtk1Range", gameObject);
    }

    protected virtual void LoadAirAtk2Range()
    {
        if (this.airAtk2Range != null) return;
        this.airAtk2Range = GetComponentInChildren<AirAtk2Range>();
        Debug.LogWarning(transform.name + ": Load AirAtk2Range", gameObject);
    }

    protected virtual void LoadAirAtk3Range()
    {
        if (this.airAtk3Range != null) return;
        this.airAtk3Range = GetComponentInChildren<AirAtk3Range>();
        Debug.LogWarning(transform.name + ": Load AirAtk3Range", gameObject);
    }
}
