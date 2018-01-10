﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public class AlphaAtlasManager : ScriptableObject, ISerializationCallbackReceiver
{
    public const string TEXURE_ALPHA_ATLAS_PATH = "Resources/TextureAlphaAtlas";

    [SerializeField]
    public List<string> names;

    private Dictionary<string, WeakReference> nameDict = new Dictionary<string, WeakReference>();

    static T LoadAsset<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(Path.Combine(TEXURE_ALPHA_ATLAS_PATH.Substring("Resources/".Length), path));
    }

    static AlphaAtlasManager m_Instance;
    public static AlphaAtlasManager GetInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = LoadAsset<AlphaAtlasManager>("AlphaAtlasConfig");
            if (m_Instance == null)
            {
                Debug.Log("AlphaAtlasConfig.asset No Find!");
                m_Instance = ScriptableObject.CreateInstance<AlphaAtlasManager>();
            }
        }
        return m_Instance;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        nameDict = new Dictionary<string, WeakReference>();
        foreach (string name in names)
        {
            nameDict.Add(name, new WeakReference(null));
        }
    }

    public Texture2D GetAlphaTexture(string name)
    {
        if (!nameDict.ContainsKey(name))
            return null;
        
        WeakReference reference = nameDict[name];
        if (reference.Target == null)
            reference.Target = LoadAsset<Texture2D>(name + "_alpha");

        return reference.Target as Texture2D;
    }

    public Texture2D GetAlphaTexture(Sprite sprite)
    {
        return GetAlphaTexture(sprite.texture.name);
    }
}