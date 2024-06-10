using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AYO
{
    public class TitleScene : SceneBase
    {
        public override IEnumerator OnStartScene()
        {
            var loadingUI = UIManager.Instance.GetUI<LoadingUI>(UIList.LoadingUI);
            var asyncSceneLoad = SceneManager.LoadSceneAsync(SceneType.Title.ToString(), LoadSceneMode.Single);
            yield return new WaitUntil(() =>
            {
                float sceneLoadProgress = asyncSceneLoad.progress / 0.9f;
                loadingUI.LoadingProgress = sceneLoadProgress;

                return asyncSceneLoad.isDone;
            });

            UIManager.Show<TitleUI>(UIList.TitleUI);
        }

        public override IEnumerator OnEndScene()
        {
            yield return null;

            UIManager.Hide<TitleUI>(UIList.TitleUI);
        }
    }
}
