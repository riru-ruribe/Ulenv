using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using Ulenv;

namespace KeyGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class KeyGenerator : IIncrementalGenerator
{
    const string Ns = "Ulenv";
    const string Atr = "UlKeyGenAttribute";
    const string AtrDisp = $"{Ns}.{Atr}";
    static readonly ulong KeyGenHash = typeof(UlKeyGenTypes).ToSerialHash();
    static readonly HashSet<byte> KeyGenSet = new()
    {
        UlKeyGenTypes.Csv,
        UlKeyGenTypes.Folder,
        UlKeyGenTypes.ColorObject,
    };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource($"{Ns}.UlKeyGen.cs", PostAtr()));

        var source = context.SyntaxProvider.ForAttributeWithMetadataName(
            AtrDisp,
            static (node, token) => node is ClassDeclarationSyntax,
            static (context, token) => context
        );
        context.RegisterSourceOutput(source, Emit);
    }

    static void Emit(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
    {
        var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
        var typeNode = (TypeDeclarationSyntax)source.TargetNode;
        var className = typeSymbol.Name;

        var accessibility = typeSymbol.DeclaredAccessibility switch
        {
            Accessibility.Private => "private",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.Public => "public",
            _ => null,
        };
        if (string.IsNullOrEmpty(accessibility))
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.E0001, typeNode.Identifier.GetLocation(), typeSymbol.Name));
            return;
        }

        if (!typeNode.Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword)))
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.E0002, typeNode.Identifier.GetLocation(), typeSymbol.Name));
            return;
        }

        var sourcePath = source.TargetNode.SyntaxTree.FilePath;
        var unityRoot = sourcePath.Replace(sourcePath.Substring(sourcePath.IndexOf("Assets")), "");

        byte? type = null;
        ulong? loop = null;
        var relativePath = string.Empty;
        var filePath = string.Empty;
        HashSet<string> extensions = default!;
        foreach (var atr in typeSymbol.GetAttributes())
        {
            if (atr?.AttributeClass?.ToDisplayString() == AtrDisp)
            {
                switch (atr.ConstructorArguments.Length)
                {
                    case 3:
                        type = (byte)atr.ConstructorArguments[0].Value!;
                        if (!KeyGenSet.Contains(type.Value))
                        {
                            type = null;
                        }
                        relativePath = atr.ConstructorArguments[1].Value!.ToString();
                        filePath = unityRoot + relativePath;
                        extensions = new(atr.ConstructorArguments[2].Values.Select(x => x.Value!.ToString()));
                        break;
                    case 4:
                        type = (byte)atr.ConstructorArguments[0].Value!;
                        if (!KeyGenSet.Contains(type.Value))
                        {
                            type = null;
                        }
                        loop = (ulong)atr.ConstructorArguments[1].Value!;
                        relativePath = atr.ConstructorArguments[2].Value!.ToString();
                        filePath = unityRoot + relativePath;
                        extensions = new(atr.ConstructorArguments[3].Values.Select(x => x.Value!.ToString()));
                        break;
                    default:
                        continue;
                }
                break;
            }
        }
        if (type == null)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.E0003, typeNode.Identifier.GetLocation(), typeSymbol.Name));
            return;
        }

        var sb = new StringBuilder();
        switch (type)
        {
            case UlKeyGenTypes.Csv:
                {
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.E0004, typeNode.Identifier.GetLocation(), typeSymbol.Name));
                        return;
                    }
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    sb.Append("  // may be changed to be recognized as an XML element.");
                    var ext = Path.GetExtension(filePath) switch
                    {
                        ".tsv" => '\t',
                        _ => ',',
                    };
                    var i = 0;
                    foreach (var line in File.ReadLines(filePath))
                    {
                        if (string.IsNullOrEmpty(line) || line.StartsWith("#") || !line.Contains(ext)) continue;
                        var ary = line.Split(ext);
                        var key = ary[0].Trim();
                        var first = ary[1].Trim().Replace("</", "&lt;/"); // VSCodeだとなぜか認識できない
                        sb.Append("\n");
                        sb.Append($"  /// <summary><![CDATA[{first}]]></summary>\n  public const int {key} = {i++};");
                    }
                    if (i > 0)
                    {
                        sb.Append($"\n  public const int Length = {i};");
                    }
                    sb.Append($"\n  public static bool IsValid(int i) => i >= 0 && i < {i};");
                }
                break;
            case UlKeyGenTypes.Folder:
                {
                    var keys = new List<string>();
                    foreach (var fp in Directory.GetFiles(filePath, "*", SearchOption.AllDirectories))
                    {
                        if (string.IsNullOrEmpty(fp) || fp.Contains(".meta") || !extensions.Contains(Path.GetExtension(fp))) continue;
                        var key = Path.GetFileNameWithoutExtension(fp);
                        if (sb.Length > 0) sb.Append("\n");
                        sb.Append($"  public const string {key} = nameof({key});");
                        keys.Add(key);
                    }
                    if (keys.Count > 0) sb.Append("\n");
                    sb.Append($"  public static readonly string[] Targets =");
                    sb.Append("\n  {");
                    foreach (var key in keys)
                    {
                        sb.Append("\n");
                        sb.Append($$"""    "{{key}}",""");
                    }
                    sb.Append("\n  };");
                }
                break;
            case UlKeyGenTypes.ColorObject:
                {
                    var len = 0;
                    var sbCode = new StringBuilder();
                    foreach (var fp in Directory.GetFiles(filePath, "*", SearchOption.AllDirectories))
                    {
                        if (string.IsNullOrEmpty(fp) || fp.Contains(".meta") || Path.GetExtension(fp) != ".clo") continue;
                        var key = Path.GetFileNameWithoutExtension(fp);
                        var t = File.ReadAllText(fp);
                        var kv = t.Split(':');
                        var vs = kv[1].Trim().Split(',');
                        if (sb.Length > 0) sb.Append("\n");
                        if (sbCode.Length > 0) sbCode.Append("\n");
                        switch (kv[0].Trim())
                        {
                            case "byte":
                                var b1 = vs.Select(x => x.Trim());
                                sb.Append($$"""  public static UnityEngine.Color {{key}} => new UnityEngine.Color32({{string.Join(", ", b1)}});""");
                                var b2 = vs.Take(3).Select(x => byte.Parse(x.Trim()).ToString("X2"));
                                sbCode.Append($$"""    public const string {{key}} = "#{{string.Join("", b2)}}";""");
                                break;
                            case "float":
                                var f1 = vs.Select(x => x.Trim() + "f");
                                sb.Append($$"""  public static UnityEngine.Color {{key}} => new UnityEngine.Color({{string.Join(", ", f1)}});""");
                                var f2 = vs.Take(3).Select(x => ((byte)(255 * float.Parse(x.Trim()))).ToString("X2"));
                                sbCode.Append($$"""    public const string {{key}} = "#{{string.Join("", f2)}}";""");
                                break;
                        }
                        len++;
                    }
                    if (sbCode.Length > 0)
                    {
                        sb.Append("\n  public static class Code");
                        sb.Append("\n  {");
                        sb.Append($"\n{sbCode}");
                        sb.Append("\n  }");
                    }
                    sb.Append($"\n  public const int Length = {len};");
                }
                break;
        }

        var count = SerialHash.Get(relativePath);
        loop ??= KeyGenHash;

        var isNamespace = !typeSymbol.ContainingNamespace.IsGlobalNamespace;

        context.AddSource($"{className}.UlKeyGen.g.cs", $$"""
// <auto-generated/>
using Ulenv;
{{(isNamespace ? $$"""namespace {{typeSymbol.ContainingNamespace}} {""" : "")}}
{{accessibility}} partial class {{className}} : IResolvable
{
  public static Unique Unique => new({{count}}, {{loop}});
  Unique IResolvable.Unique => new({{count}}, {{loop}});
{{sb}}
}
{{(isNamespace ? "}" : "")}}
""");
    }

    static string PostAtr()
    {
        return $$"""
using System;
namespace {{Ns}}
{
    /// <summary>
    /// [UlKeyGenTypes.Csv]<br/>
    /// keys will be output as 'const int'. that is, index of valid records in c[t]sv file.<br/><br/>
    /// 
    /// [UlKeyGenTypes.Folder]<br/>
    /// keys will be output as 'const string'. that is, name of files in folder.<br/><br/>
    /// 
    /// [UlKeyGenTypes.ColorObject]<br/>
    /// keys will be output as 'Color' and 'const string #xxxxxx'. that is, parsed 'clo' files in folder.<br/><br/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class {{Atr}} : Attribute
    {
        /// <param name="filePath">relative path starting with 'Assets'</param>
        /// <param name="extensions">file extensions to track</param>
        public {{Atr}}(byte type, string filePath, params string[] extensions) { }

        /// <param name="loop">second argument of 'Unique' constructor.</param>
        /// <param name="filePath">relative path starting with 'Assets'</param>
        /// <param name="extensions">file extensions to track</param>
        public {{Atr}}(byte type, ulong loop, string filePath, params string[] extensions) { }
    }
}
""";
    }
}
