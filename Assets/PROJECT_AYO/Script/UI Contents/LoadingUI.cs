using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class LoadingUI : UIBase
    {
        /// <summary> Loading Progress Ex) 0.0f ~ 1.0f </summary>
        public float LoadingProgress
        {
            set
            {
                progressText.text = $"{value * 100.0f:0.0}%"; // "0.0~100.0 + %"
                progressBar.fillAmount = value;
            }
        }

        public TMPro.TextMeshProUGUI progressText;
        public UnityEngine.UI.Image progressBar;
    }
}
