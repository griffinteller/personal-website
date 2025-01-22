using UnityEngine;
using UnityEngine.EventSystems;

public class CRTInput : BaseInput {
    public CRTInputModule module;
            
    public override Vector2 mousePosition => ScreenToCanvasPoint(base.mousePosition);

    Vector2 ScreenToCanvasPoint(Vector2 screenPoint) {
        var realHeight = Screen.height * module.crtScaler.Scale;
        var realWidth = realHeight * module.crtScaler.AspectRatio;

        var x = screenPoint.x - (Screen.width - realWidth) / 2;
        var y = screenPoint.y - (Screen.height - realHeight) / 2;

        return new Vector2(x, y);
    }
}