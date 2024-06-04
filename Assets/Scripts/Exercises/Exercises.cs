using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Exercises : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // AddressableMgr.Instance.LoadAssetAsync<GameObject>("Cube", (obj) =>
        // {
        //     Instantiate(obj.Result);
        // });
        // AddressableMgr.Instance.LoadAssetAsync<GameObject>("Cube", (obj) =>
        // {
        //     Instantiate(obj.Result, Vector3.right * 5, Quaternion.identity);
        //     AddressableMgr.Instance.Release<GameObject>("Cube");
        // });
        AddressableMgr.Instance.LoadAssetAsync<GameObject>(Addressables.MergeMode.Union, (obj) => { print(obj.name); }, "Cube", "Red");
        AddressableMgr.Instance.LoadAssetAsync<GameObject>(Addressables.MergeMode.Union, (obj) => { print(obj.name); }, "Cube", "Red");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
