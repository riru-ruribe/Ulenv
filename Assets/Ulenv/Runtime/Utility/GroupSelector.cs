#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Ulenv
{
    /// <summary>
    /// 'IGroup'の実装を選択可能にします
    /// </summary>
    public sealed class GroupSelector : UnityEngine.PropertyAttribute
    {
    }
#if UNITY_EDITOR
#pragma warning disable UNT0008
    [CustomPropertyDrawer(typeof(GroupSelector))]
    sealed class GroupSelectorDrawer : PropertyDrawer
    {
        static readonly Type BaseType = typeof(IGroup);
        MonoScript[] scripts;
        string[] scriptNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (scripts == null)
            {
                scripts = GetScripts();
                scriptNames = scripts.Select(x => x?.GetClass().Name ?? "None...").ToArray();
            }

            position.width /= 3;
            GUI.Label(position, "Group");

            position.x += position.width;
            position.width *= 2;
            var cur = string.IsNullOrEmpty(property.stringValue)
                ? 0
                : Array.IndexOf(scriptNames, property.stringValue);
            var next = EditorGUI.Popup(position, cur, scriptNames);
            if (cur != next)
            {
                property.stringValue = scriptNames[next];
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        static MonoScript[] GetScripts()
        {
            return AssetDatabase.FindAssets("t:script", new[] { "Assets", })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<MonoScript>)
                .Where(x =>
                {
                    var type = x?.GetClass();
                    return type != null // file name and class name must match.
                        && type != BaseType // ignore myself.
                        && !type.IsAbstract
                        && type.GetInterfaces().Any(y => y == BaseType);
                })
                .Prepend(null)
                .ToArray();
        }
    }
#endif
}
