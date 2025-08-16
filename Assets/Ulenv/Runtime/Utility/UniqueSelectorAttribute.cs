#if UNITY_EDITOR
#pragma warning disable IDE0001
#pragma warning disable IDE0002
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Ulenv
{
    public sealed class UniqueSelectorAttribute : UnityEngine.PropertyAttribute
    {
        public readonly System.Type Type, UniqueGenType;
        public readonly string RelativePath, SearchPattern;
        public readonly System.IO.SearchOption Option;
        /// <param name="type">required inherit 'IResolvable'.</param>
        public UniqueSelectorAttribute(System.Type type) => Type = type;
        public UniqueSelectorAttribute(
            string relativePath,
            string searchPattern = "*",
            System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly,
            System.Type uniqueGenType = null)
        {
            RelativePath = relativePath;
            SearchPattern = searchPattern;
            Option = option;
            UniqueGenType = uniqueGenType;
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UniqueSelectorAttribute))]
    sealed class UniqueSelectorAttributeDrawer : PropertyDrawer
    {
        string[] scriptNames;
        Unique[] uniques;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var atr = attribute as UniqueSelectorAttribute;
            if (scriptNames == null)
            {
                if (atr.Type != null)
                {
                    var scripts = GetScripts(atr.Type);
                    scriptNames = scripts
                        .Select(x => x?.GetClass().Name ?? "None...")
                        .ToArray();
                    uniques = scripts
                        .Select(x => x?.GetClass())
                        .Where(x => x != null)
                        .Select(x => ((IResolvable)Activator.CreateInstance(x)).Unique)
                        .Prepend(Unique.Zero)
                        .ToArray();
                }
                else if (!string.IsNullOrEmpty(atr.RelativePath))
                {
                    var uniqueGen = atr.UniqueGenType == null
                        ? new UlKeyUniqueGenerator()
                        : (IUlKeyUniqueGenerator)Activator.CreateInstance(atr.UniqueGenType);
                    var names = Directory
                        .GetFiles(atr.RelativePath, atr.SearchPattern, atr.Option)
                        .Where(x => Path.GetExtension(x) != ".meta")
                        .Select(x => x.Replace("\\", "/"))
                        .ToArray();
                    scriptNames = names
                        .Select(Path.GetFileNameWithoutExtension)
                        .Prepend("None...")
                        .ToArray();
                    uniques = names
                        .Select(x => (Unique)uniqueGen.Generate(x))
                        .Prepend(Unique.Zero)
                        .ToArray();
                }
            }

            position.width /= 3;
            GUI.Label(position, property.displayName);

            position.x += position.width;
            position.width *= 2;
            var u = (SerialUnique)property.boxedValue;
            var cur = u == Unique.Zero
                ? 0
                : Array.IndexOf(uniques, u);
            var next = EditorGUI.Popup(position, cur, scriptNames);
            if (cur != next)
            {
                property.boxedValue = (SerialUnique)uniques[next];
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        static MonoScript[] GetScripts(Type baseType)
        {
            return AssetDatabase.FindAssets("t:script", new[] { "Assets", })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<MonoScript>)
                .Where(x =>
                {
                    var type = x.GetClass();
                    return type != null // file name and class name must match.
                        && type != baseType // ignore myself.
                        && !type.IsAbstract
                        && type.GetInterfaces().Any(y => y == baseType);
                })
                .Prepend(null)
                .ToArray();
        }
    }
#endif
}
