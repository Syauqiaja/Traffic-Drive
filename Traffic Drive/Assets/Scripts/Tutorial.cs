using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [TextArea] public List<string> dialog;

    private void Start() {
        VisualManager.Instance.tutorialList = dialog;
        VisualManager.Instance.ShowTutorial();
    }
}
