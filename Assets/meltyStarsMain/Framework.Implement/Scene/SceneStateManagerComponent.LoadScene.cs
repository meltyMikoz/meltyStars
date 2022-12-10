using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace KuusouEngine
{
    public partial class SceneStateManagerComponent
    {
        private AsyncOperation LoadSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
