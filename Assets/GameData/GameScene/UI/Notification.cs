using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : CoreMonoBehaviour
{
    protected static Notification instance;
    public static Notification Instance => instance;
   
    [SerializeField] protected Image image;
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected float cooldown = 1f;

    protected override void Awake()
    {
        base.Awake();
        if (Notification.instance != null) Debug.LogError("Only 1 Notification Manager is allowed to exist", gameObject);
        Notification.instance = this;
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadImage();
        this.LoadText();
    }

    protected virtual void LoadImage()
    {
        if (this.image != null) return;
        this.image = GetComponentInChildren<Image>();
        Debug.LogWarning(transform.name + ": Load Image", gameObject);
    }

    protected virtual void LoadText()
    {
        if (this.text != null) return;
        this.text = GetComponentInChildren<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + ": Load Text", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        this.image.gameObject.SetActive(false);
        this.text.gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        this.cooldown -= Time.deltaTime;
        if (this.cooldown <= 0) this.cooldown = 0;
    }

    public virtual void Show(string content)
    {    
        if (this.cooldown > 0) return;
        StartCoroutine(this.ShowNotification(content));
    }

    protected IEnumerator ShowNotification(string content)
    {
        yield return null;
        this.image.gameObject.SetActive(true);
        this.text.gameObject.SetActive(true);
        this.text.text = content;
        yield return new WaitForSeconds(2f);
        this.image.gameObject.SetActive(false);
        this.text.gameObject.SetActive(false);
        this.cooldown = 1f;
    }
}
