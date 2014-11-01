namespace SBL.Common.Annotations
{
    using System;

    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field,
        AllowMultiple = false,
        Inherited = true)]
    public sealed class CanBeNullAttribute : Attribute { }

    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field,
        AllowMultiple = false,
        Inherited = true)]
    public sealed class NotNullAttribute : Attribute { }
}