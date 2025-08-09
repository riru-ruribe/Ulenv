using Microsoft.CodeAnalysis;

namespace KeyGenerator;

public static class DiagnosticDescriptors
{
    const string Category = "UlKeyGen";

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
        title: "invalid arg",
        messageFormat: "invalid 'UlKeyGenTypes'.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor E0004 = new(
        id: Category + nameof(E0004),
        title: "invalid arg",
        messageFormat: "c[t]sv file not exist.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}
