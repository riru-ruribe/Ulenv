using System;
using System.Collections.Generic;

namespace Ulenv
{
    /// <summary>
    /// <see cref="InterfaceFieldAttribute"/>から隠すための属性
    /// </summary>
    public sealed class SerialOmitterAttribute : Attribute
    {
        public readonly HashSet<Type> Targets;
        public SerialOmitterAttribute() { }
        public SerialOmitterAttribute(params Type[] types) => Targets = new(types);
    }
}
