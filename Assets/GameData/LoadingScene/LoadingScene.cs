using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : CoreMonoBehaviour
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadSceneAsync());
    }

    protected IEnumerator LoadSceneAsync()
    {
        AsyncOperation aSyncLoad = SceneManager.LoadSceneAsync(ApplicationVariable.LoadingSceneName);
        aSyncLoad.allowSceneActivation = false; //no loading next scene even if progress is done
        while (aSyncLoad.progress < 0.9f) 
        {
            Debug.Log("Loading Progress... " + aSyncLoad.progress);
            yield return null; 
        }
        Debug.Log("Loading Progress... " + aSyncLoad.progress);
        yield return new WaitForSeconds(2f);
        aSyncLoad.allowSceneActivation = true;
    }
}
