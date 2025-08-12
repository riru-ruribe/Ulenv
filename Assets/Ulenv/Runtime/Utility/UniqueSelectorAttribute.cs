#if UNITY_EDITOR
#pragma warning disable IDE0001
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Ulenv
{
    /// <summary>
    /// required inherit 'IResolvable'.
    /// </summary>
    public sealed class UniqueSelectorAttribute : UnityEngine.PropertyAttribute
    {
        public readonly System.Type Type;
        public UniqueSelectorAttribute(System.Type type) => Type = type;
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UniqueSelectorAttribute))]
    sealed class UniqueSelectorAttributeDrawer : PropertyDrawer
    {
        MonoScript[] scripts;
        string[] scriptNames;
        Unique[] uniques;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (scripts == null)
            {
                scripts = GetScripts((attribute as UniqueSelectorAttribute).Type);
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
