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
    public sealed class CsvKeySelectorAttribute : UnityEngine.PropertyAttribute
    {
        public readonly string RelativePath, SearchPattern;
        public readonly char Separator;
        public readonly System.IO.SearchOption Option;
        public readonly System.Type UniqueGenType;
        public CsvKeySelectorAttribute(
            string relativePath,
            string searchPattern = "*",
            char separator = ',',
            System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly,
            System.Type uniqueGenType = null)
        {
            RelativePath = relativePath;
            SearchPattern = searchPattern;
            Separator = separator;
            Option = option;
            UniqueGenType = uniqueGenType;
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CsvKeySelectorAttribute))]
    sealed class CsvKeySelectorAttributeDrawer : PropertyDrawer
    {
        string[] csvPathes, csvNames, keyNames = Array.Empty<string>(), texts = Array.Empty<string>();
        Unique[] csvUniques;
        static readonly GUIStyle style = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var atr = attribute as CsvKeySelectorAttribute;
            if (csvNames == null)
            {
                var uniqueGen = atr.UniqueGenType == null
                    ? new UlKeyUniqueGenerator()
                    : (IUlKeyUniqueGenerator)Activator.CreateInstance(atr.UniqueGenType);
                csvPathes = Directory
                    .GetFiles(atr.RelativePath, atr.SearchPattern, atr.Option)
                    .Where(x => Path.GetExtension(x) != ".meta")
                    .Select(x => x.Replace("\\", "/"))
                    .Prepend(null)
                    .ToArray();
                csvNames = csvPathes
                    .Skip(1)
                    .Select(Path.GetFileNameWithoutExtension)
                    .Prepend("None...")
                    .ToArray();
                csvUniques = csvPathes
                    .Skip(1)
                    .Select(x => (Unique)uniqueGen.Generate(x))
                    .Prepend(Unique.Zero)
                    .ToArray();
            }

            var w = position.width;
            position.width /= 5;
            style.normal.textColor = Color.white;
            GUI.Label(position, property.displayName, style);

            position.x += position.width;
            position.width *= 2;
            var r = (UniqueKeyResult)property.boxedValue;
            var cur = r.unique == Unique.Zero
                ? 0
                : Array.IndexOf(csvUniques, r.unique);
            var next = EditorGUI.Popup(position, cur, csvNames);
            if (cur != next)
            {
                property.boxedValue = new UniqueKeyResult(csvUniques[next], 0);
                property.serializedObject.ApplyModifiedProperties();
                keyNames = Array.Empty<string>();
            }

            position.x += position.width;
            r = (UniqueKeyResult)property.boxedValue;
            cur = r.index;
            if (r.unique != Unique.Zero && keyNames.Length == 0)
            {
                var lines = File.ReadLines(csvPathes[next]);
                keyNames = lines
                    .Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith('#') && x.Contains(atr.Separator))
                    .Select(x => x.Split(atr.Separator)[0].Trim())
                    .ToArray();
                texts = lines
                    .Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith('#') && x.Contains(atr.Separator))
                    .Select(x => x.Split(atr.Separator)[1].Trim())
                    .ToArray();
            }
            next = EditorGUI.Popup(position, cur, keyNames);
            if (cur != next)
            {
                property.boxedValue = new UniqueKeyResult(r.unique, next);
                property.serializedObject.ApplyModifiedProperties();
            }

            position.width = w;
            position.x = 0f;
            position.y += position.height * 0.5f;
            style.normal.textColor = new(1f, 1f, 1f, 0.5f);
            GUI.Label(position, next < texts.Length ? texts[next] : string.Empty, style);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2f;
        }
    }
#endif
}
