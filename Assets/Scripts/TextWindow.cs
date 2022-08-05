using UnityEngine;

public class TextWindow : MonoBehaviour 
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            WindowManager.Instance.SelectWindow(FileWindow.Name);
        }
    }
}