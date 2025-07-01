using System;

namespace App.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SingletonServiceAttribute : Attribute
    {
    }
}
