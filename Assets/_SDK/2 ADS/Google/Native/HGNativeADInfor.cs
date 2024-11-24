using System;
using UnityEngine;

public class HGNativeADInfor
{
    [SerializeField] Texture2D icon;
    [SerializeField] Texture2D adIcon;
    [SerializeField] string title;
    [SerializeField] string content;
    [SerializeField] string buttonText;

    Action RegisterIconImageGameObject;
    public HGNativeADInfor()
    {
    }
    public HGNativeADInfor(Texture2D icon, Texture2D adIcon, string title, string content, string buttonText)
    {
        this.icon = icon;
        this.adIcon = adIcon;
        this.title = title;
        this.content = content;
        this.buttonText = buttonText;
    }

    public string Buttontext => buttonText;

    public void SetButtontext(string value)
    {
        buttonText = value;
    }


    public string Content => content;

    public void SetContent(string value)
    {
        content = value;
    }


    public string Title => title;

    public void SetTitle(string value)
    {
        title = value;
    }


    public Texture2D ADIcon => adIcon;

    public void SetADIcon(Texture2D value)
    {
        adIcon = value;
    }


    public Texture2D Icon => icon;

    public void SetIcon(Texture2D value)
    {
        icon = value;
    }
}
