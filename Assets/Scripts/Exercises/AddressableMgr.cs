using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesInfo
{
    //记录异步操作句柄
    public AsyncOperationHandle handle;
    //记录引用计数
    public uint Count = 0;
    public AddressablesInfo(AsyncOperationHandle handle)
    {
        this.handle = handle;
        Count += 1;
    }
}
public class AddressableMgr
{
    private static AddressableMgr instance = new AddressableMgr();
    public static AddressableMgr Instance => instance;
    private AddressableMgr() { }
    //异步加载容器
    private Dictionary<string, AddressablesInfo> resDic = new Dictionary<string, AddressablesInfo>();
    //异步加载资源
    public void LoadAssetAsync<T>(string name, Action<AsyncOperationHandle<T>> callback)
    {
        string keyName = name + "_" + typeof(T).Name;
        AsyncOperationHandle<T> handle;
        if (resDic.ContainsKey(keyName))
        {
            handle = resDic[keyName].handle.Convert<T>();
            resDic[keyName].Count += 1;
            //判断有没有加载结束
            if (handle.IsDone)
            {
                callback(handle);
            }
            else
            {
                handle.Completed += (obj) =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                        callback(obj);
                    else
                    {
                        Debug.LogWarning(keyName + "：资源加载失败");
                        if (resDic.ContainsKey(keyName))
                            resDic.Remove(keyName);
                    }
                };
            }
            return;
        }
        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
                callback(obj);
            else
            {
                Debug.LogWarning(keyName + "：资源加载失败");
                if (resDic.ContainsKey(keyName))
                    resDic.Remove(keyName);
            }
        };
        resDic.Add(keyName, new AddressablesInfo(handle));
    }
    //异步加载多个资源或指定资源
    public void LoadAssetAsync<T>(Addressables.MergeMode mode, Action<T> callback, params string[] key)
    {
        List<string> list = new List<string>(key);
        string keyName = "";
        foreach (string item in list)
        {
            keyName += item + "_";
        }
        keyName += typeof(T).Name;
        AsyncOperationHandle<IList<T>> handle;
        if (resDic.ContainsKey(keyName))
        {
            handle = resDic[keyName].handle.Convert<IList<T>>();
            resDic[keyName].Count += 1;
            if (handle.IsDone)
            {
                foreach (T item in handle.Result)
                {
                    callback(item);
                }
            }
            else
            {
                handle.Completed += (obj) =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        foreach (T item in obj.Result)
                        {
                            callback(item);
                        }
                    }
                };
            }
            return;
        }
        handle = Addressables.LoadAssetsAsync<T>(list, callback, mode);
        handle.Completed += (obj) =>
        {
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning(keyName + ":资源加载失败");
                if (resDic.ContainsKey(keyName))
                    resDic.Remove(keyName);
            }
        };
        resDic.Add(keyName, new AddressablesInfo(handle));
    }
    public void Release<T>(params string[] key)
    {
        List<string> list = new List<string>(key);
        string keyName = "";
        foreach (string item in list)
        {
            keyName += item + "_";
        }
        keyName += typeof(T).Name;

        if (resDic.ContainsKey(keyName))
        {
            resDic[keyName].Count -= 1;
            if (resDic[keyName].Count == 0)
            {
                AsyncOperationHandle<IList<T>> handle = resDic[keyName].handle.Convert<IList<T>>();
                Addressables.Release(handle);
                resDic.Remove(keyName);
            }
        }

    }
    //释放资源
    public void Release<T>(string name)
    {
        string keyName = name + "_" + typeof(T).Name;
        AsyncOperationHandle<T> handle;
        if (resDic.ContainsKey(keyName))
        {
            resDic[keyName].Count -= 1;
            if (resDic[keyName].Count == 0)
            {
                handle = resDic[keyName].handle.Convert<T>();
                Addressables.Release(handle);
                resDic.Remove(keyName);
            }

        }
    }
    //清空资源
    public void Clear()
    {
        foreach (var item in resDic.Values)
        {
            Addressables.Release(item);
        }
        resDic.Clear();
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
