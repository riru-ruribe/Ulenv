namespace Ulenv;

public readonly struct NullableReference
{
    public readonly bool Is;
    public readonly object Target;
    public NullableReference(object target)
    {
        Is = true;
        Target = target;
    }
}
