using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyGraphicsRaycaster : GraphicRaycaster {
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultList) {
        print(eventData.position);
        base.Raycast(eventData, resultList);
    }
}