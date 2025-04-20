using System;
using UnityEngine;

namespace AG.Base.SceneTransition
{
    public class SceneTransitionLoadingScreen : MonoBehaviour
    {
        public void PlayTransitionAnimation()
        {

        }

        public void StopTransitionAnimation(Action onTransitionAnimationComplete)
        {
            onTransitionAnimationComplete?.Invoke();
        }
    }
}