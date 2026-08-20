# CSIsNull002

| Property | Value |
| --- | --- |
| Title | Use an `is` expression for non-null checks |
| Category | Usage |
| Default severity | Info |

This analyzer reports a diagnostic when C# code uses the built-in `!=` operator to compare a reference or nullable value with `null`.

Pattern syntax cannot invoke an overloaded inequality operator. It also lets the compiler reject null checks against non-nullable value types.

## Example

For example, this code produces a diagnostic:

```csharp
if (value != null)
{
}
```

The code fixes replace the inequality check with pattern matching (`is`) syntax and support Fix All.

For C# 9 or later, `is not null` is listed first:

```csharp
if (value is not null)
{
}
```

The `is object` form is also available:

```csharp
if (value is object)
{
}
```

For earlier C# language versions, and within expression trees where `is not null` is unsupported, only `is object` is offered. Both forms let the compiler reject null checks against non-nullable value types.

## Configuration and suppression

The rule has no custom options. Configure its severity with the standard `.editorconfig` key `dotnet_diagnostic.CSIsNull002.severity`, or suppress a specific occurrence with standard pragma or `SuppressMessage` mechanisms.
