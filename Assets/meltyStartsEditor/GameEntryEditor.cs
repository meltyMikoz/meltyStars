using UnityEditor;
using UnityEditor.SceneManagement;

namespace SyntheticWatermelonLikeGame.Editor
{
    public class GameEntryEditor
    {
        [MenuItem("SyntheticWatermelonLikeGame/PlayViaEntry _F5")]
        private static void PlayGameEntry()
        {
            EditorSceneManager.OpenScene(@"Assets/Res/Scenes/Entry.unity");
            EditorApplication.isPlaying = true;
        }
    }
}
