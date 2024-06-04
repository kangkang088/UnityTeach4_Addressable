using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Lesson14 : MonoBehaviour
{
    public AssetReference assetReference;
    // Start is called before the first frame update
    void Start()
    {
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync("Cube", typeof(GameObject));
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in obj.Result)
                {
                    print(item.PrimaryKey);
                    Addressables.LoadAssetAsync<GameObject>(item).Completed += (obj) =>
                    {
                        Instantiate(obj.Result);
                    };
                }
            }
            else
            {
                Addressables.Release(handle);
            }
        };
        AsyncOperationHandle<IList<IResourceLocation>> handles = Addressables.LoadResourceLocationsAsync(new List<string> { "Cube", "Sphere", "SD" }, Addressables.MergeMode.Union, typeof(Object));
        handles.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in obj.Result)
                {
                    Addressables.LoadAssetAsync<Object>(item).Completed += (obj) =>
                    {
                        Debug.Log(obj.Result.name);
                    };
                }
            }
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
