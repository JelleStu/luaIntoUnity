using UnityEngine.UIElements;

namespace Ui
{
    public interface IView
    {
        public T Show<T>(T component) where T : VisualElement;
    }
}