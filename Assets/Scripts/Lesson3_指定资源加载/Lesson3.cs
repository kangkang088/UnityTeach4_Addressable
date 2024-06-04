using System.Diagnostics.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetReferenceAudio : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudio(string guid) : base(guid)
    {
    }
}
public class Lesson3 : MonoBehaviour
{
    public AssetReference assetReference;
    public AssetReferenceAtlasedSprite atlasReference;
    public AssetReferenceGameObject gameObjectReference;
    public AssetReferenceSprite spriteReference;
    public AssetReferenceTexture textureReference;
    public AssetReferenceT<AudioClip> audioclip;
    public AssetReferenceT<RuntimeAnimatorController> controller;
    public AssetReferenceT<TextAsset> textAsset;
    public AssetReference sceneReference;
    public AssetReferenceT<Material> redReference;

    void Start()
    {
        AsyncOperationHandle<GameObject> handle = assetReference.LoadAssetAsync<GameObject>();
        handle.Completed += Handle_Complete;
        audioclip.LoadAssetAsync().Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Music has done");
            }
        };
        //sceneReference.LoadSceneAsync();
        gameObjectReference.InstantiateAsync();

    }

    private void Handle_Complete(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject cube = Instantiate(handle.Result);
            assetReference.ReleaseAsset();
            redReference.LoadAssetAsync().Completed += (hanldes) =>
            {
                cube.GetComponent<MeshRenderer>().material = hanldes.Result;
                redReference.ReleaseAsset();
            };
        }
    }
    void Update()
    {

    }
}
