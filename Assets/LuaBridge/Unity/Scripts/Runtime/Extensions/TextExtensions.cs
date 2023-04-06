using System;
using UnityEngine.UI;

namespace LuaBridge.Core.Extensions
{
    public static class TextExtensions
    {
        public static bool TrySetText(this Text target, string text)
        {
            try
            {
                target.text = text;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}