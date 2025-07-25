using Microsoft.CodeAnalysis;

namespace UlCallbackGenerator;

public static class DiagnosticDescriptors
{
    const string Category = "UlCallback";

    public static readonly DiagnosticDescriptor E0001 = new(
        id: Category + nameof(E0001),
        title: "invalid accessibility",
        messageFormat: "'public' or 'protected' or 'internal' or 'private' is allowed.",
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

    public static readonly DiagnosticDescriptor E0003 = new(
        id: Category + nameof(E0003),
        title: "invalid syntax",
        messageFormat: "method attribute is empty.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}
