using AG.Base.Addressable;
using AG.Base.Util;
using UnityEngine;
using UnityEngine.UI;

namespace AG.Base.SceneTransition
{
    //Simple Class To Add Change Scene Functionality To Buttons
    public class SceneChangerButton : MonoBehaviour
    {
        [SerializeField] private AddressableSceneNames _sceneName;
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
            ObjectFinder.FindObjectInChilderenWithType(ref _button, transform);
        }
#endif
    }
}