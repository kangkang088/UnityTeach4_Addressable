using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson15 : MonoBehaviour
{
    AsyncOperationHandle<GameObject> handle;
    // Start is called before the first frame update
    void Start()
    {
        #region 事件监听
        // handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        // handle.Completed += (obj) =>
        // {
        //     print("事件创建对象");
        //     Instantiate(obj.Result);
        // };
        #endregion
        #region 协程
        //StartCoroutine(LoadAsset());
        #endregion
        #region async await
        Load();
        #endregion

    }
    IEnumerator LoadAsset()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        if (!handle.IsDone)
            yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            print("协同程序创建对象");
            Instantiate(handle.Result);
        }
        else
        {
            Addressables.Release(handle);
        }
    }
    async void Load()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        AsyncOperationHandle<IList<GameObject>> handle2 = Addressables.LoadAssetsAsync<GameObject>(new List<string> { "Cube", "SD" }, (obj) => { }, Addressables.MergeMode.Intersection);
        //await handle.Task;
        await Task.WhenAll(handle.Task, handle2.Task);
        print("异步函数形式");
        Instantiate(handle.Result);
        foreach (var item in handle2.Result)
        {
            Instantiate(item);
        }
    }
    void Update()
    {

    }
}
