using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkButton : MonoBehaviour
{
    public string url;

    // Update is called once per frame
    public void Open() {
        print("hello");
        Application.OpenURL(url);
    }
}
