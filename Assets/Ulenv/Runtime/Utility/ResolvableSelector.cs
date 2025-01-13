#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Ulenv
{
    /// <summary>
    /// IResolvableが実装するUniqueをシリアル化します
    /// </summary>
    public sealed class ResolvableSelector : UnityEngine.PropertyAttribute
    {
        public readonly System.Type BaseType;
        public ResolvableSelector() { }
        public ResolvableSelector(System.Type baseType) => BaseType = baseType;
    }
#if UNITY_EDITOR
#pragma warning disable UNT0008
    [CustomPropertyDrawer(typeof(ResolvableSelector))]
    sealed class ResolvableSelectorDrawer : PropertyDrawer
    {
        static readonly Type ParentType = typeof(IResolvable);
        static readonly Type MonoType = typeof(MonoBehaviour);
        MonoScript[] scripts;
        string[] scriptNames;
        Unique[] uniques;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var baseType = (attribute as ResolvableSelector)?.BaseType;
            if (baseType == null) baseType = ParentType;
            else if (!IsInherit(baseType, ParentType))
            {
                Debug.LogError($"{baseType} is not inherit 'IResolvable'.");
                baseType = ParentType;
            }

            if (scripts == null)
            {
                scripts = GetScripts(baseType);
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
            GUI.Label(position, "Resolvable");

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
                        && type.GetInterfaces().Any(y => y == baseType)
                        && IsParent(type, MonoType);
                })
                .Prepend(null)
                .ToArray();
        }

        static bool IsParent(Type self, Type parent)
        {
            var baseType = self.BaseType;
            while (baseType != null)
            {
                if (baseType == parent) return true;
                baseType = baseType.BaseType;
            }
            return false;
        }

        static bool IsInherit(Type self, Type inherit)
            => self.GetInterfaces().Any(x => x == inherit);
    }
#endif
}
