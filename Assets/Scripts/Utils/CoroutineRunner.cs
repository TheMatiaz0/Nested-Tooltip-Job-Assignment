using UnityEngine;
using System.Collections;

namespace Unity.BossRoom.Utils
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner s_Instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    s_Instance = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }

                return s_Instance;
            }
        }

        public Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void StopRunningCoroutine(Coroutine coroutine)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }
    }
}
