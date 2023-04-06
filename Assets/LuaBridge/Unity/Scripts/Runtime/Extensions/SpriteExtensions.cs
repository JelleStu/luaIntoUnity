using UnityEngine;

namespace LuaBridge.Core.Extensions
{
    public static class SpriteExtensions
    {
        public static void Destroy(this Sprite sprite)
        {
            if (!sprite || !sprite.texture || sprite.texture.hideFlags != HideFlags.HideAndDontSave)
                return;

            Object.DestroyImmediate(sprite.texture);
            Object.Destroy(sprite);
        }
    }
}