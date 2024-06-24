using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace AYO
{
    public enum UIList 
    {
        None = 0,
        SCENE_PANEL_START, //panel Start
        // To do : Include Panel UI List

        TitleUI,
        LoadingUI,
        InventoryUI,

        SCENE_PANEL_END, //Panel End
        SCENE_POPUP_START, //Popup Start
        // To do : Include Popup UI List

        //Popup
        GameMenuPopup,

        SCENE_POPUP_END, // Popup End
        End,

    }
}
