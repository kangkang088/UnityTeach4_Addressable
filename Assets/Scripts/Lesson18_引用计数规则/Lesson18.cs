using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Lesson18 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    private List<AsyncOperationHandle<GameObject>> list = new List<AsyncOperationHandle<GameObject>>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Cube");
            // handle.Completed += (obj) => { Instantiate(obj.Result); };
            // list.Add(handle);

            AddressableMgr.Instance.LoadAssetAsync<GameObject>("Cube", (obj) => { Instantiate(obj.Result); });
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // if (list.Count > 0)
            // {
            //     Addressables.Release(list[0]);
            //     list.RemoveAt(0);
            // }
            AddressableMgr.Instance.Release<GameObject>("Cube");
        }
    }
}
