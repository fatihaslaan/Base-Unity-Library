using System;
using UnityEngine;

namespace AG.Base.SceneTransition
{
    internal sealed class SceneTransitionLoadingScreen : MonoBehaviour
    {
        internal void PlayTransitionAnimation()
        {

        }

        internal void StopTransitionAnimation(Action onTransitionAnimationComplete)
        {
            onTransitionAnimationComplete?.Invoke();
        }
    }
}