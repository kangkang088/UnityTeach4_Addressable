using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson16 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadAsset());

        AsyncOperationHandle<Texture2D> asyncOperationHandle;
        AsyncOperationHandle noTypeTemp = Addressables.LoadAssetAsync<Texture2D>("Cube");
        asyncOperationHandle = noTypeTemp.Convert<Texture2D>();

        print("1");
        asyncOperationHandle.WaitForCompletion();
        print("2");
        print(asyncOperationHandle.Result.name);


    }
    IEnumerator LoadAsset()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Cube");
        // if (!handle.IsDone)
        //     yield return handle;
        while (!handle.IsDone)
        {
            DownloadStatus status = handle.GetDownloadStatus();
            print("进度百分比：" + status.Percent);
            print("字节下载数：" + status.DownloadedBytes);
            print("字节总数：" + status.TotalBytes);
            yield return 0;
        }
        if (handle.Status == AsyncOperationStatus.Succeeded)
            Instantiate(handle.Result);
        else
            Addressables.Release(handle);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
