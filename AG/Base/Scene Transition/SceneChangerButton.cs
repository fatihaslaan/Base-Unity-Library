using AG.Base.Addressable;
using UnityEngine;
using UnityEngine.UI;

namespace AG.Base.SceneTransition
{
    //Simple Class To Add Change Scene Functionality To Buttons
    internal sealed class SceneChangerButton : MonoBehaviour
    {
        [SerializeField] private AddressableSceneName _sceneName;
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnChangeSceneButtonPressed);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnChangeSceneButtonPressed);
        }

        private void OnChangeSceneButtonPressed()
        {
            SceneTransitionController.Instance.ChangeScene(_sceneName);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            Editor.Util.ObjectFinder.FindObjectInChilderenWithType(ref _button, transform);
        }

#endif
    }
}