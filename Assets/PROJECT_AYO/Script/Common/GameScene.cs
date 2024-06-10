using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AYO
{
    public class GameScene : SceneBase
    {
        public override IEnumerator OnStartScene()
        {
            var loadingUI = UIManager.Instance.GetUI<LoadingUI>(UIList.LoadingUI);
            var asyncSceneLoad = SceneManager.LoadSceneAsync(SceneType.Game.ToString(), LoadSceneMode.Single);
            
            yield return new WaitUntil(() =>
            {
                float sceneLoadProgress = asyncSceneLoad.progress / 0.9f;
                loadingUI.LoadingProgress = sceneLoadProgress;

                return asyncSceneLoad.isDone;
            });
        }

        public override IEnumerator OnEndScene()
        {
            yield return null;
        }
    }
}
