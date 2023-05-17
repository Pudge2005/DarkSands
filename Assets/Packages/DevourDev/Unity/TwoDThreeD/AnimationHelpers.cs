using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    public static class AnimationHelpers
    {
        public static void SyncAnimators(Animator from, Animator to)
        {
            //Animation animation = null;
            //animation.
            int paramsCount = from.parameterCount;

            for (int i = 0; i < paramsCount; i++)
            {
                var animParam = from.GetParameter(i);
                SyncAnimParam(ref from, ref to, ref animParam);
            }

            var state = from.GetCurrentAnimatorStateInfo(0);
            to.Play(state.fullPathHash, 0, state.normalizedTime);
        }

        private static void SyncAnimParam(ref Animator from, ref Animator to,
                                   ref AnimatorControllerParameter animParam)
        {
            var hash = animParam.nameHash;
            switch (animParam.type)
            {
                case AnimatorControllerParameterType.Float:
                    to.SetFloat(hash, from.GetFloat(hash));
                    break;
                case AnimatorControllerParameterType.Int:
                    to.SetInteger(hash, from.GetInteger(hash));
                    break;
                case AnimatorControllerParameterType.Bool:
                    to.SetBool(hash, from.GetBool(hash));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    //to.SetTrigger(hash);
                    break;
                default:
                    break;
            }
        }
    }
}
