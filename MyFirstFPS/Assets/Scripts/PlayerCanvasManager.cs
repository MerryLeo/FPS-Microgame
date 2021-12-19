using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasManager : MonoBehaviour {
    [SerializeField]
    GameObject playerObj;

    [SerializeField]
    GameObject[] UIElements;

    void Start() {
        Begin();
    }

    void Begin() {
        foreach (GameObject element in UIElements) {
            IPlayerUIElement interfaceElement = (IPlayerUIElement)element.GetComponent(typeof(IPlayerUIElement));
            element.SetActive(true);
            interfaceElement.Begin(playerObj);
        }
    }
}
