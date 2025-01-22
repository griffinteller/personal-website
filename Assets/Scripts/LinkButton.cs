using UnityEngine;
using UnityEngine.UI;

public class LinkButton : MonoBehaviour {
    [SerializeField] string url;

    public void Start() {
        GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
    }
}