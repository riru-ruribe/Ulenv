#if UNITY_EDITOR
#pragma warning disable UNT0023
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Ulenv
{
    [CreateAssetMenu(menuName = "Ulenv/" + nameof(SerialMap), fileName = nameof(SerialMap))]
    public sealed class SerialMap : ScriptableObject
    {
        [Serializable]
        public sealed class Unit : IEquatable<Unit>
        {
            public string Path;
            public SerialUnique Unique;
            public bool Equals(Unit other) => other != null &&
                Path == other.Path &&
                Unique == other.Unique;
        }

        [SerializeField] internal List<Unit> units = default;

        static SerialMap _asset;
        internal static SerialMap Asset => _asset ??= AssetDatabase
            .FindAssets($"t:{nameof(SerialMap)}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<SerialMap>)
            .FirstOrDefault();

        internal static bool Staging;

        internal static bool Any(string oldPath, SerialUnique unique)
        {
            if (Asset is not { } asset)
            {
                Debug.LogWarning("not found 'SerialMap'.");
                return false;
            }
            for (int i = 0; i < asset.units.Count; i++)
                if (asset.units[i] is { } x && x.Path == oldPath)
                    return x.Unique == unique;
            return false;
        }

        internal static void Replace(string oldPath, string newPath, SerialUnique unique)
        {
            if (Asset is not { } asset)
            {
                Debug.LogWarning("not found 'SerialMap'.");
                return;
            }
            if (!string.IsNullOrEmpty(oldPath))
            {
                for (int i = 0; i < asset.units.Count; i++)
                {
                    if (asset.units[i] is { } x && x.Path == oldPath)
                    {
                        x.Path = newPath;
                        x.Unique = unique;
                        EditorUtility.SetDirty(asset);
                        return;
                    }
                }
            }
            var y = new Unit { Path = newPath, Unique = unique, };
            if (asset.units.Contains(y)) return;
            asset.units.Add(y);
            EditorUtility.SetDirty(asset);
        }

        internal static void Remove(string path)
        {
            if (Asset is not { } asset)
            {
                Debug.LogWarning("not found 'SerialMap'.");
                return;
            }
            for (int i = 0; i < asset.units.Count; i++)
            {
                if (asset.units[i] is { } x && x.Path == path)
                {
                    asset.units.RemoveAt(i);
                    EditorUtility.SetDirty(asset);
                    return;
                }
            }
        }

        [InitializeOnLoadMethod]
        static void OnLoad()
        {
            PrefabStage.prefabStageOpened += _ => Staging = true;
            PrefabStage.prefabStageClosing += _ => Staging = false;
        }
    }
}
#endif
