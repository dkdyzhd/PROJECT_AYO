using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class TitleUI : UIBase
    {
        public void OnClickStartButton()
        {
            Main.Instance.ChangeScene(SceneType.Game);
        }

        public void OnClickExitButton()
        {
            Debug.Log("Clicked Exit Button");
        }
    }
}
