using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LuaBridge.Core.Extensions
{
    public static class AnimatorExtensions
    {
        public static List<string> GetTriggers(this Animator animator, string likeSearch = "")
        {
            return animator.GetParameters(AnimatorControllerParameterType.Trigger, likeSearch);
        }

        public static List<string> GetBools(this Animator animator, string likeSearch = "")
        {
            return animator.GetParameters(AnimatorControllerParameterType.Bool, likeSearch);
        }

        public static List<string> GetParameters(this Animator animator, AnimatorControllerParameterType parameterType, string likeSearch = "")
        {
            var originalActiveState = animator.gameObject.activeSelf;
            var originalEnabledState = animator.enabled;
            if (!Application.isPlaying)
                animator.gameObject.SetActive(true);
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                // Super dirty, but otherwise you'll get a unity bug that the animator parameters are unexposed
                if (!animator.gameObject.activeInHierarchy)
                {
                    animator.gameObject.SetActive(true);
                    animator.enabled = false;
                    animator.enabled = true;
                    animator.gameObject.SetActive(false);
                }

                animator.enabled = false;
                animator.enabled = true;
                List<string> triggers = animator.parameters
                    .Where(p => p.type == parameterType && p.name.Contains(likeSearch))
                    .Select(p => p.name).ToList();
                animator.gameObject.SetActive(originalActiveState);
                animator.enabled = originalEnabledState;
                return triggers;
            }

            animator.gameObject.SetActive(originalActiveState);
            return new List<string>();
        }

        public static void ResetTrigger(this Animator animator, string trigger)
        {
            if (animator.runtimeAnimatorController != null && animator.parameters.SingleOrDefault(p => p.name == trigger) != null) animator.ResetTrigger(trigger);
        }

        public static bool ParameterExists(this Animator animator, string trigger)
        {
            if (animator != null && animator.gameObject.activeInHierarchy && animator.runtimeAnimatorController != null)
            {
                return animator.parameters.Any(p => p.name == trigger);
            }

            return false;
        }
    }
}