#if UNITY_EDITOR
#pragma warning disable IDE0001
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Ulenv
{
    public sealed class SerialMapSelector : UnityEngine.PropertyAttribute
    {
        public readonly System.Type Type;
        public SerialMapSelector(System.Type type) => Type = type;
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerialMapSelector))]
    sealed class SerialMapSelectorDrawer : PropertyDrawer
    {
        string[] pathes;
        SerialUnique[] uniques;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = (attribute as SerialMapSelector).Type;
            if (pathes == null)
            {
                /// <see cref="SerialComponent{T}.LateUpdate"/>
                var loop = type.ToSerialHash();
                var units = SerialMap.Asset.units.Where(x => x.Unique.loop == loop);
                pathes = units.Select(x => x.Path).Prepend("None...").ToArray();
                uniques = units.Select(x => x.Unique).Prepend(Unique.Zero).ToArray();
            }

            position.width /= 3;
            GUI.Label(position, type.Name);

            position.x += position.width;
            position.width *= 2;
            var u = (SerialUnique)property.boxedValue;
            var cur = u == Unique.Zero
                ? 0
                : Array.IndexOf(uniques, u);
            var next = EditorGUI.Popup(position, cur, pathes);
            if (cur != next)
            {
                property.boxedValue = uniques[next];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}
