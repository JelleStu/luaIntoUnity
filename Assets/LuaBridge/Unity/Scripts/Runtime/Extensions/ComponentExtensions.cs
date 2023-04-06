using UnityEngine;

namespace LuaBridge.Core.Extensions
{
	public static class ComponentExtensions
	{
		#region Methods

		public static T GetOrAddComponent<T>(this Component siblingComponent) where T : Component
		{
			return siblingComponent.TryGetComponent(out T component) ? component : siblingComponent.gameObject.AddComponent<T>();
		}

		#endregion
	}
}
