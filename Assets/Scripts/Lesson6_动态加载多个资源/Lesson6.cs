using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AsyncOperationHandle<IList<Object>> handle = Addressables.LoadAssetsAsync<Object>("Cube", (obj) =>
        {
            //print(obj.name);
        });
        handle.Completed += (obj) =>
        {
            foreach (Object item in obj.Result)
            {
                print(item.name);
            }
        };
        List<string> strs = new List<string>() { "Cube", "Red" };
        Addressables.LoadAssetsAsync<Object>(strs, (obj) => { print(obj.name); }, Addressables.MergeMode.Union);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
