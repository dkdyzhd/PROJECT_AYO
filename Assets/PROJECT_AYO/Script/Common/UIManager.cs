using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; } = null;

        public static T Show<T>(UIList uiPrefabName) where T : UIBase
        {
            if (Instance == null)
                return null;

            T ui = Instance.GetUI<T>(uiPrefabName);
            if (ui == null)
                return null;

            ui.Show();
            return ui;
        }

        public static T Hide<T>(UIList uiPrefabName) where T : UIBase
        {
            if (Instance == null)
                return null;

            T ui = Instance.GetUI<T>(uiPrefabName);
            if (ui == null)
                return null;

            ui.Hide();
            return ui;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public Dictionary<UIList, UIBase> loadedUIContainer = new Dictionary<UIList, UIBase>();

        [SerializeField] private Transform panelRoot;
        [SerializeField] private Transform popupRoot;

        public void Initialize()
        {
            if(panelRoot == null)
            {
                GameObject goPanelRoot = new GameObject("Panel Root");
                goPanelRoot.transform.SetParent(transform);
                goPanelRoot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                goPanelRoot.transform.localScale = Vector3.one;
                panelRoot = goPanelRoot.transform;
            }

            if(popupRoot == null)
            {
                GameObject goPopupRoot = new GameObject("Popup Root");
                goPopupRoot.transform.SetParent(transform);
                goPopupRoot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                goPopupRoot.transform.localScale= Vector3.one;
                popupRoot = goPopupRoot.transform;
            }

            for( int index = (int)UIList.SCENE_PANEL_START + 1; index < (int)UIList.SCENE_PANEL_END; index++ )
            {
                loadedUIContainer.Add((UIList)index, null);
            }

            for (int index = (int)UIList.SCENE_POPUP_START+ 1; index < (int)UIList.SCENE_POPUP_END; index++)
            {
                loadedUIContainer.Add((UIList) index, null);
    }
        }

        public T GetUI<T>(UIList uiPrefabName) where T : UIBase
        {
            if(UIList.SCENE_PANEL_START <uiPrefabName && uiPrefabName < UIList.SCENE_PANEL_END)
            {
                if (loadedUIContainer.ContainsKey(uiPrefabName))
                {
                    if (loadedUIContainer[uiPrefabName] == null)
                    {
                        string path = $"UI/Prefab/{uiPrefabName}";
                        UIBase loadedUI = Resources.Load<UIBase>(path);

                        if (loadedUI == null)
                            return null;

                        T result = loadedUI.GetComponent<T>();
                        if (result == null)
                            return null;

                        loadedUIContainer[uiPrefabName] = Instantiate(loadedUI, popupRoot).GetComponent<T>();
                        if (loadedUIContainer[uiPrefabName])
                            loadedUIContainer[uiPrefabName].gameObject.SetActive(false);

                        return loadedUIContainer[uiPrefabName].GetComponent<T>();
                    }
                    else
                    {
                        return loadedUIContainer[uiPrefabName].GetComponent<T>();
                    }
                }
            }

            return null;
        }


    }
}
