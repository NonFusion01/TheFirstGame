using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTimeUI : CoreMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadText();
    }

    protected virtual void LoadText()
    {
        if (this.text != null) return;
        this.text = GetComponentInChildren<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + ": Load Text", gameObject);
    }

    protected virtual void Update()
    {
        this.UpdatePlayTime();
    }

    protected virtual void UpdatePlayTime()
    {
        this.text.text = CharManager.Instance._charStats.PlayTimeLeft.ToString();
    }
}
