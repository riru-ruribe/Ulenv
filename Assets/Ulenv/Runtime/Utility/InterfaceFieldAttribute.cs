#if UNITY_EDITOR
#pragma warning disable IDE0001
#pragma warning disable IDE0002
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#if UNITY_2023_1_OR_NEWER
using System.Text.RegularExpressions;
#endif
#endif

namespace Ulenv
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public sealed class InterfaceFieldAttribute : UnityEngine.PropertyAttribute { }
#if UNITY_EDITOR
#pragma warning disable UNT0007
#pragma warning disable UNT0008
    [CustomPropertyDrawer(typeof(InterfaceFieldAttribute))]
    sealed class InterfaceFieldDrawer : PropertyDrawer
    {
        static readonly Type OmitterType = typeof(SerialOmitterAttribute);
        int index;
        MonoScript[] scripts;
        List<MonoScript> omitted;
        string[] scriptNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference) return;

            if (scripts == null)
            {
                scripts = GetScripts(property);
                scriptNames = scripts.Select(x => x?.GetClass().Name ?? "None...").ToArray();
            }

            index = 0;
            var managedName = "None...";
            if (HasInstance(property))
            {
                managedName = GetClassName(property.managedReferenceFullTypename);
                index = Mathf.Max(Array.IndexOf(scriptNames, managedName), 0);

                if (index >= scripts.Length)
                {
                    Debug.LogError($"clear field because index out of range: {index} > {scripts.Length}");
                    property.managedReferenceValue = null;
                    return;
                }
            }

            var drawRect = position;
            drawRect.width = position.width / 3 * 2;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(drawRect, $"[{GetClassName(property.managedReferenceFieldTypename)}] {label.text}");

            drawRect.x += drawRect.width;
            drawRect.width = position.width / 3 * 1;
            var selected = EditorGUI.Popup(drawRect, index, scriptNames);
            if (selected != index)
            {
                index = selected;
                var script = scripts[index]?.GetClass() ?? Omittable(managedName)?.GetClass();
                property.managedReferenceValue = script == null
                    ? null
                    : Activator.CreateInstance(
#if UNITY_2023_1_OR_NEWER // can use generic interface.
                        script.IsGenericType ? script.MakeGenericType(script.GenericTypeArguments) : script
#else
                        script
#endif
                    )
                ;
            }

            var x = 12;
            drawRect.x = position.x + x;
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            drawRect.width = position.width - x;
            EditorGUI.BeginDisabledGroup(true);
            var scriptRef = scripts[index] ?? Omittable(managedName);
            EditorGUI.ObjectField(drawRect, "Script Ref", scriptRef, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (HasInstance(property))
            {
                EditorGUI.PropertyField(drawRect, property, new GUIContent(managedName), true);
            }
            else
            {
                EditorGUI.LabelField(drawRect, "None...");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 'Script Ref' and 'PropertyField'
            return 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)
                + (HasInstance(property) ? EditorGUI.GetPropertyHeight(property, true) : EditorGUIUtility.singleLineHeight);
        }

        bool HasInstance(SerializedProperty property)
        {
            return !string.IsNullOrEmpty(property.managedReferenceFullTypename);
        }

        string GetClassName(string src)
        {
#if UNITY_2023_1_OR_NEWER // can use generic interface.
            if (src.Contains('`'))
            {
                var tmp = src[src.IndexOf(' ')..];
                tmp = tmp.Replace("mscorlib, ", "");
                tmp = Regex.Replace(tmp, @"Version=.*,", "");
                tmp = Regex.Replace(tmp, @"Culture=.*,", "");
                tmp = Regex.Replace(tmp, @"PublicKeyToken=.*", "");
                tmp += "]]";
                return tmp;
            }
#endif

            var dst = src.Split(' ')[1];

            var spl = dst.Split('.');
            if (spl != null && spl.Length > 0)
            {
                dst = spl.Last();
            }

            return dst;
        }

        Type GetBaseType(SerializedProperty property)
        {
            var typename = property.managedReferenceFieldTypename;
            var spl = typename.Split(' ');
#if UNITY_2023_1_OR_NEWER // can use generic interface.
            if (typename.Contains('`'))
            {
                return Assembly.Load(spl[0]).GetType(typename[typename.IndexOf(' ')..]);
            }
#endif
            return Assembly.Load(spl[0]).GetType(spl[1]);
        }

        MonoScript[] GetScripts(SerializedProperty property)
        {
            var baseType = GetBaseType(property);
            return AssetDatabase.FindAssets("t:script", new[] { "Assets", })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<MonoScript>)
                .Where(x =>
                {
                    var type = x.GetClass();
                    return type != null // file name and class name must match.
                        && type != baseType // ignore myself.
                        && !type.IsAbstract
                        && type.GetInterfaces().Any(y => y == baseType)
                        && IsTarget(type, baseType, x);
                })
                .Prepend(null)
                .ToArray();
        }

        bool IsTarget(Type type, Type baseType, MonoScript monoScript)
        {
            if (type.GetCustomAttribute(OmitterType) is not SerialOmitterAttribute atr)
                return true;
            if (atr.Targets == null || atr.Targets.Contains(baseType))
            {
                omitted ??= new();
                omitted.Add(monoScript);
                return false;
            }
            return true;
        }

        MonoScript Omittable(string name) => omitted?.FirstOrDefault(x => x?.GetClass().Name == name);
    }
#endif
}
