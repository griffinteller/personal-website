using UnityEngine;
using UnityEngine.EventSystems;

public class CRTInputModule : StandaloneInputModule {
    public CRTScaler crtScaler;

    protected override void Start() {
        base.Start();
        
        var crtInput = gameObject.AddComponent<CRTInput>();
        crtInput.module = this;
        inputOverride = crtInput;
    }
}