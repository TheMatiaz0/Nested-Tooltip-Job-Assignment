using UnityEngine;

namespace Unity.BossRoom.Utils
{
    public static class UiExtensions
    {
        public static Canvas GetRootCanvas(this Transform rect)
        {
            Transform current = rect.transform;

            while (current != null)
            {
                Canvas canvas = current.GetComponent<Canvas>();

                if (canvas != null && canvas.isRootCanvas)
                {
                    return canvas;
                }

                current = current.parent;
            }

            return null;
        }
    }

}
