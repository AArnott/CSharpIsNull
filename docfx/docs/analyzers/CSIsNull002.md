# CSIsNull002

This analyzer flags use of `!= null` to test whether a value is not `null`.

For example, this code produces a diagnostic:

```csharp
if (value != null)
{
}
```

The code fixes replace the inequality check with pattern matching (`is`) syntax, which cannot invoke an overloaded inequality operator.

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
