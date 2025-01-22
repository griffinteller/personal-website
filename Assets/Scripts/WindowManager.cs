using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    private readonly Dictionary<string, GameObject> _windows = new ();
    private GameObject _activeWindow;

    public void Awake()
    {
        if (ReferenceEquals(Instance, null))
            Instance = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        foreach (Transform child in transform)
        {
            _windows.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }

        _activeWindow = transform.GetChild(0).gameObject;
        _activeWindow.SetActive(true);
    }

    public void SelectWindow(string name)
    {
        if (!_windows.ContainsKey(name))
        {
            throw new ArgumentException("Window not found: " + name);
        }

        _activeWindow.SetActive(false);
        _activeWindow = _windows[name];
        _activeWindow.SetActive(true);
    }
}