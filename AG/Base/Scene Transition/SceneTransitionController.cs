using System.Collections;
using AG.Base.Addressable;
using AG.Base.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG.Base.SceneTransition
{
    public sealed class SceneTransitionController : PersistentSingleton<SceneTransitionController>
    {
        [SerializeField] private SceneTransitionModel _sceneTransitionModel;
        [SerializeField] private SceneTransitionView _sceneTransitionView;

        private void Start()
        {
            AddListeners();
            ChangeScene(AddressableSceneName.Main_Menu_Scene);
        }

        private void OnSceneLoadFailed()
        {
            Debug.Log("Scene Load Failed");
        }

        private void OnTransitionPrefabLoadFailed()
        {
            Debug.Log("Scene Transition Prefab Load Failed");
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            StartCoroutine(WaitOngoingOperations());
        }

        private IEnumerator WaitOngoingOperations()
        {
            while (AddressableManager.OngoingOperationCount > 0)
            {
                //Retry
                //Waiting All Operations That Started On Awake
                Debug.Log("Waiting Ongoing Operations... " + AddressableManager.OngoingOperationCount);
                yield return null;
            }
            _sceneTransitionView.HideLoading();
            Debug.Log("Scene Change Completed");
        }

        public void ChangeScene(AddressableSceneName sceneName)
        {
            Debug.Log("Scene Change Started");
            _sceneTransitionView.ShowLoading();
            _sceneTransitionModel.LoadScene(sceneName);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #region Listeners

        private void AddListeners()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            _sceneTransitionModel.OnSceneLoadFailed += OnSceneLoadFailed;
            _sceneTransitionView.OnTransitionPrefabLoadFailed += OnTransitionPrefabLoadFailed;
        }

        private void RemoveListeners()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            _sceneTransitionModel.OnSceneLoadFailed -= OnSceneLoadFailed;
            _sceneTransitionView.OnTransitionPrefabLoadFailed -= OnTransitionPrefabLoadFailed;
        }

        #endregion

#if UNITY_EDITOR

        private void OnValidate()
        {
            Editor.Util.ObjectFinder.FindObjectInChilderenWithType(ref _sceneTransitionModel, transform);
            Editor.Util.ObjectFinder.FindObjectInChilderenWithType(ref _sceneTransitionView, transform);
        }

#endif
    }
}