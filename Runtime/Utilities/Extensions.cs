using UnityEngine;
using System.Collections;

namespace HuggingFace.API {
    public static class Extensions {
        public class CoroutineRunner : MonoBehaviour { }

        public static void RunCoroutine(this IEnumerator coroutine) {
            GameObject coroutineRunnerObject = new GameObject("HuggingFaceAPI_CoroutineRunner");
            CoroutineRunner coroutineRunner = coroutineRunnerObject.AddComponent<CoroutineRunner>();
            coroutineRunner.StartCoroutine(RunAndDestroy(coroutine, coroutineRunnerObject));
        }

        private static IEnumerator RunAndDestroy(IEnumerator coroutine, GameObject coroutineRunnerObject) {
            yield return coroutine;
#if UNITY_EDITOR
            GameObject.DestroyImmediate(coroutineRunnerObject);
#else
            GameObject.Destroy(coroutineRunnerObject);
#endif
        }
    }
}