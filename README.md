# Unity Base Library - AG

A Unity library containing tools and utilities for fast and clean game development that will be used in my games. Includes features like; addressables usage, scene transitions, singleton management, and a util to find objects. For now.

> 🛠️ **Will Be Updated As I Develop My Own Games**

---

## 📚 Table of Contents

- 📥 [Installation](#installation)
- 🧩 [Introduction](#introduction)
- 📦 [Addressable System](#addressable-system)
- 🌇 [Scene Transitions](#scene-transitions)
- 1️⃣ [Singleton System](#singleton-system)
- 🧰 [Other Utilities](#other-utilities)
- 🤝 [Contributing](#contributing)
- 🙌 [Credits](#credits)

---

<a name="installation"></a>
## 📥 Installation

1. Install 'Addressables' package to your project first!
2. Use 'AG.unitypackage' in your project.

---

<a name="introduction"></a>
## 🧩 Introduction

This library is built to provide a solid foundation for Unity projects, helping developers avoid repetitive setup and annoying codes. Designed with scalability, reusability and safety in mind.

Features include:
- Easy Addressable downloads without worrying about releasing and bugs too much
- Smooth scene transitions
- Custom utilities to speed up development and ensure safety

---
<a name="addressable-system"></a>
## 📦 Addressable System

Load Addressables in your 'MonoBehaviour' classes just by a single inheritance.

Let's dive in

If you want to load an addressable by 'Assetreference' normally you would do something similar to this;
```bash
public AssetReference addressableReference;
private AsyncOperationHandle _currentHandle;

private ScriptableObject _result;

private void Start()
{
   _currentHandle = LoadAndGetAddressable();
}

private void Release()
{
   if (_currentHandle.IsValid())
   {
      _result = null;
      Addressables.Release(_currentHandle);
      _currentHandle = default;
   }
}

private AsyncOperationHandle LoadAndGetAddressable()
{
   AsyncOperationHandle<ScriptableObject> handle = Addressables.LoadAssetAsync<ScriptableObject>(addressableReference);
   handle.Completed += operation =>
   {
      if (operation.Status == AsyncOperationStatus.Succeeded)
      {
         OnOperationCompleted(operation.Result);
      }
      else
      {
         OnOperationFailed();
      }
   };

   return handle;

   void OnOperationCompleted(ScriptableObject obj)
   {
      _result = obj;
   }

   void OnOperationFailed() { }
}

private void OnDestroy()
{
   Release();
}
```
And this is the simple one. You have to write something similiar for almost every operation like 'Addressables.LoadAssetsAsync', 'InstantiateAsync', etc...

And this could still fire **Null Referance Error** from a destroyed object which started addressable load! (Addressable Loads Can't Be Stopped!)

ANNOYING!

Instead... You can do this;

```bash
public class Loader : AddressableLoader<ScriptableObject>
{
    private void Start()
    {
        //Or Use AssetReference
        InitiateAddressableOperation("Key");
    }

    private void Release()
    {
        ReleaseOperation();
    }

    //No Need To Call OnDestroy

    protected override void OnOperationSucceed()
    {
        //operationResult.DoThing();
    }

    protected override void OnOperationFailed(){}
}
```
Or this to load multiple items;
```bash
public class LabelLoader : AddressablesLoader<ScriptableObject>
{
    private void Start()
    {
        InitiateAddressableOperation(AddressableLabelNames.JustALabelName);
    }

    private void Release()
    {
        ReleaseOperation();
    }

    protected override void OnOperationSucceed()
    {
        //It is IList<ScriptableObject>
        //Do something!
        operationResult.Clear();
    }

    protected override void OnOperationFailed(){}
}
```
With this inheritances you no longer need to rewrite same codes over and over again! It also handles **Null Referance Errors** from destroyed objects and doesn't fire their OnSucced Methods.

There is also 
- 'AddressableInstantiator' Which uses Addressables.InstantiateAsync as function
- 'AddressableTypeInstantiator' Which is same as 'AddressableInstantiator' but also gets desired component
- 'AddressableSceneLoader' Which uses Addressables.LoadSceneAsync

**Things To Keep In Mind**
- When you call 'InitiateAddressableOperation' It automatically releases previous handle, so you don't have to worry about it
- You don't have to call 'ReleaseOperation()' on 'OnDestroy'. Parent class handles that
- You can use your desired type after 'OnOperationSucceed'. To use it simply write 'operationResult' (If you use it somewhere else, you should know it is only setted after 'OnOperationSucced')
- You can change settings of parent classes like; override 'GetInstantiatedObjectParent' and set it to something different than 'return null;' for setting parent, override 'AllowComponentAddition' and set 'return true;' if you want to add component to instantiated object if 'TryGetComponent' fails while using 'AddressableTypeInstantiator' or override 'GetLoadSceneMode' to change LoadMode of Scenes
- You can override 'FinalizeOperationResult' for anything before setting 'operationResult'
- You should override 'ClearOperationResult' and clear all references if you duplicated 'operationResult' for unload that operation from memory. (Even if you release addressable, if a reference is still exist it will stick in memory)

> **This Classes Written After Reading 'Unity Addressable Documentation'**

---

<a name="scene-transitions"></a>
## 🌇 Scene Transitions

Change your scenes with this libraries addressable system and show loading screens if you want to.
```bash
//From 'SceneChangerButton' Class But You Can Change Scene From Anywhere!
private void OnChangeSceneButtonPressed()
{
    SceneTransitionController.Instance.ChangeScene(_sceneName);
}

//From 'SceneTransitionController' Class
private void OnSceneChanged(Scene arg0, Scene arg1)
{
   StartCoroutine(WaitOngoingOperations());
}

private IEnumerator WaitOngoingOperations()
{
   while (AddressableManager.OngoingOperations.Count > 0)
   {
      //Waiting All Operations That Started On Awake
      Debug.Log("Waiting Ongoing Operations... " + AddressableManager.OngoingOperations.Count);
      yield return null;
   }
   _sceneTransitionView.HideLoading();
   Debug.Log("Scene Change Completed");
}


internal sealed class SceneTransitionModel : AddressableSceneLoader
{
    internal void LoadScene(AddressableSceneNames sceneName, Action onSceneLoadStart)
    {
        //Starts Scene Load Process
        InitiateAddressableOperation(sceneName);
    }

    protected override void OnOperationFailed()
    {
        Debug.Log("Scene Load Failed");
    }

    protected override void OnOperationSucceed()
    {
        Debug.Log("Scene Load Completed");
        if (GetLoadSceneMode() == LoadSceneMode.Additive)
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
```

---

<a name="singleton-system"></a>
## 1️⃣ Singleton System

Lightweight, and easy to use singleton and persistent singleton (Doesn't destroyed on scene changes) base classes.
```bash
public class SingletonClass : Singleton<SingletonClass>{}
public class PeristentSingletonClass : PersistentSingleton<PeristentSingletonClass>{}

public class AnyClass
{
   private void Start()
   {
      //Access from anywhere
      PersistentSingletonClass.Instance;
   }
}
``` 

---

<a name="other-utilities"></a>
## 🧰 Other Utilities

Set your serializefields in 'OnValidate' to make sure you assigned necessary variables.
```bash
[SerializeField] private SceneTransitionView _sceneTransitionView;

#if UNITY_EDITOR

private void OnValidate()
{
    //Will call 'Debug.Log' if it fails to set
    ObjectFinder.FindObjectInChilderenWithType(ref _sceneTransitionView, transform);
}

#endif
```

---

<a name="contributing"></a>
## 🤝 Contributing

If you have suggestions or improvements, feel free to open an issue or submit a pull request!

---

<a name="credits"></a>
## 🙌 Credits

Created by Fatih Aslan

Uses Unity's official Addressables system.

Inspired by common development patterns across Unity projects.
