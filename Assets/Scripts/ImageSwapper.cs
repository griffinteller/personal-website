using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwapper : MonoBehaviour
{
    public Texture2D[] imgs;
    
    private int active = 0;
    private Image imageComponent;

    void Start () {
        if (imgs.Length == 0) {
            Debug.LogError("No images in image swapper list!");
            Destroy(gameObject);
        }  

        imageComponent = GetComponent<Image>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            active = (active + 1) % imgs.Length;
            var newImg = imgs[active];
            imageComponent.sprite = Sprite.Create(newImg, new Rect(0, 0, newImg.width, newImg.height), new Vector2(0, 0));
        }
    }
}
