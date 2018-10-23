using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {

    public BasePanel[] basePanels = null;

    private void Awake()
    {
        if (basePanels!=null) {
            for (int i = 0; i < basePanels.Length; i++)
            {
                basePanels[i].RegistAction();
            }
        }
    }


    private void OnDestroy()
    {
        if (basePanels != null)
        {
            for (int i = 0; i < basePanels.Length; i++)
            {
                basePanels[i].RemoveAction();
            }
        }
    }
}
