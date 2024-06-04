using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson17 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Addressables.UpdateCatalogs().Completed += (obj) =>
        {
            Addressables.Release(obj);
        };
        Addressables.CheckForCatalogUpdates(true).Completed += (obj) =>
        {
            if (obj.Result.Count > 0)
            {
                Addressables.UpdateCatalogs(obj.Result, true).Completed += (objs) =>
                {
                    // Addressables.Release(objs);
                    // Addressables.Release(obj);
                };
            }
        };

        //预加载资源
        StartCoroutine(LoadAsset());
    }
    IEnumerator LoadAsset()
    {
        AsyncOperationHandle<long> handleSize = Addressables.GetDownloadSizeAsync(new List<string> { "Cube", "Sphere", "SD" });
        yield return handleSize;
        if (handleSize.Result > 0)
        {
            AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(new List<string> { "Cube", "Sphere", "SD" }, Addressables.MergeMode.Union);
            while (!handle.IsDone)
            {
                DownloadStatus downloadStatus = handle.GetDownloadStatus();
                print(downloadStatus.Percent);
                print(downloadStatus.DownloadedBytes + "_" + downloadStatus.TotalBytes);
                yield return 0;
            }
            Addressables.Release(handle);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
