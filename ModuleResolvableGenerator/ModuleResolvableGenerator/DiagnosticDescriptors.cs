using Microsoft.CodeAnalysis;

namespace ModuleResolvableGenerator;

public static class DiagnosticDescriptors
{
    const string Category = "Ulenv";

    public static readonly DiagnosticDescriptor E0001 = new(
        id: Category + nameof(E0001),
        title: "invalid accessibility",
        messageFormat: "public is allowed.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor E0002 = new(
        id: Category + nameof(E0002),
        title: "invalid syntax",
        messageFormat: "'partial' class required.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}
