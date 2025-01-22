using System.Collections.Generic;
using UnityEngine;

public class ObjectSwapper : MonoBehaviour {
    [SerializeField] List<GameObject> objects;

    int activeIdx = 0;

    void Sync() {
        for (int i = 0; i < objects.Count; i++) {
            objects[i].SetActive(i == activeIdx);
        }
    }

    void Start() {
        Sync();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            activeIdx = (activeIdx + 1) % objects.Count;
            Sync();
        }
    }
}
